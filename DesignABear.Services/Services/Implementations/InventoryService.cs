using AutoMapper;
using DesignABear.Repositories.Models;
using DesignABear.Repositories.UnitOfWork;
using DesignABear.Services.DTOs.Responses;
using DesignABear.Services.Exceptions;
using DesignABear.Services.Services.Interfaces;
using Microsoft.Extensions.Logging;
using DesignABear.Services.DTOs.Requests;
using Microsoft.EntityFrameworkCore;

namespace DesignABear.Services.Services.Implementations
{
    public class InventoryService : IInventoryService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<InventoryService> _logger;
        private static readonly Guid DEFAULT_WAREHOUSE_ID = new Guid("55555555-5555-5555-5555-555555555555");

        public InventoryService(IUnitOfWork uow, IMapper mapper, ILogger<InventoryService> logger)
        {
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<InventoryResponse?> GetAsync(Guid locationId, Guid identityId, bool isAccessory)
        {
            var inv = await _uow.Inventories.GetByLocationAndIdentityAsync(locationId, identityId, isAccessory);
            if (inv == null) throw new NotFoundException($"Inventory for Location {locationId} and Identity {identityId} not found");
            return _mapper.Map<InventoryResponse>(inv);
        }

        public async Task<IEnumerable<InventoryResponse>> GetByProductIdAsync(Guid productId)
        {
            var inventories = await _uow.Inventories.GetByProductIdAsync(productId);
            var responses = _mapper.Map<List<InventoryResponse>>(inventories);

            // Populate variant info for Bear products
            var variants = await _uow.ProductVariants.GetQueryable()
                .Where(v => v.ProductId == productId)
                .ToListAsync();
            var variantDict = variants.ToDictionary(v => v.VariantId);

            var product = await _uow.Products.GetByIdAsync(productId);

            foreach (var res in responses)
            {
                if (variantDict.TryGetValue(res.IdentityId, out var v))
                {
                    res.SizeTag = v.SizeTag;
                    res.SizeDescription = v.SizeDescription;
                    res.Sku = v.Sku;
                }
                else if (res.IdentityId == productId && product != null)
                {
                    res.Sku = product.Sku;
                }
            }

            return responses;
        }

        public async Task<IEnumerable<InventoryResponse>> GetByAccessoryIdAsync(Guid accessoryId)
        {
            var inventories = await _uow.Inventories.GetByIdentityIdAsync(accessoryId, true);
            var responses = _mapper.Map<List<InventoryResponse>>(inventories);

            var accessory = await _uow.Accessories.GetByIdAsync(accessoryId);
            if (accessory != null)
            {
                foreach (var res in responses)
                {
                    res.Sku = accessory.Sku;
                    res.ProductName = accessory.Name;
                    res.ImageUrl = accessory.ImageUrl;
                }
            }

            return responses;
        }

        public async Task<int> GetTotalAvailableAsync(Guid identityId, bool isAccessory)
            => await _uow.Inventories.GetTotalAvailableAsync(identityId, isAccessory);

        public async Task AdjustStockAsync(Guid identityId, bool isAccessory, int delta, Guid locationId)
        {
            var affected = await _uow.Inventories.AtomicAdjustOnHandAsync(identityId, isAccessory, delta, locationId);
            if (affected == 0)
            {
                if (delta > 0)
                {
                    // For new inventory, we might need a ProductId if it's a variant.
                    // But for simplicity in this atomic call, we assume the record exists or we create a minimal one.
                    // Usually ProductId is handled at a higher level during first creation.
                    Guid? productId = null;
                    if (!isAccessory)
                    {
                        var variant = await _uow.ProductVariants.GetByIdAsync(identityId);
                        if (variant != null)
                        {
                            productId = variant.ProductId;
                        }
                        else
                        {
                            // If not a variant, check if identityId is the productId itself
                            var product = await _uow.Products.GetByIdAsync(identityId);
                            if (product != null) productId = product.ProductId;
                        }
                    }

                    var inv = new Inventory { 
                        InventoryId = Guid.NewGuid(),
                        LocationId = locationId, 
                        IdentityId = identityId,
                        IsAccessory = isAccessory,
                        ProductId = productId,
                        OnHand = delta 
                    };
                    await _uow.Inventories.AddAsync(inv);
                    await _uow.SaveChangesAsync();
                }
                else
                {
                    throw new InvalidOperationException("Inventory record not found or insufficient stock for adjustment.");
                }
            }
        }

        public async Task AdjustStockByIdAsync(Guid identityId, bool isAccessory, int delta, Guid? locationId = null)
        {
            var locId = locationId ?? DEFAULT_WAREHOUSE_ID;
            await AdjustStockAsync(identityId, isAccessory, delta, locId);
        }

        public async Task ReserveStockAsync(Guid identityId, bool isAccessory, int quantity, Guid locationId)
        {
            var affected = await _uow.Inventories.AtomicReserveAsync(identityId, isAccessory, quantity, locationId);
            if (affected == 0)
            {
                throw new InvalidOperationException("Stock reservation failed: Item not found or insufficient stock.");
            }
        }

        public async Task ReleaseReservationAsync(Guid identityId, bool isAccessory, int quantity, Guid locationId)
        {
            var affected = await _uow.Inventories.AtomicReleaseAsync(identityId, isAccessory, quantity, locationId);
            if (affected == 0)
            {
                _logger.LogWarning("ReleaseReservation failed: Inventory record not found for Location {LocationId}", locationId);
            }
        }

        public async Task ConfirmDeductionAsync(Guid identityId, bool isAccessory, int quantity, Guid locationId)
        {
            var affected = await _uow.Inventories.AtomicDeductAsync(identityId, isAccessory, quantity, locationId);
            if (affected == 0)
            {
                // Inventory record does not exist yet — this can happen when products/variants
                // were created without seeding an Inventory row. Instead of crashing the entire
                // payment webhook (which would leave the order stuck in PENDING after the customer
                // already paid), we log a warning and continue. The admin can reconcile stock later.
                _logger.LogWarning(
                    "ConfirmDeduction skipped: No inventory record found for IdentityId={IdentityId}, IsAccessory={IsAccessory}, LocationId={LocationId}. " +
                    "The product may not have been stocked yet. Quantity={Quantity}",
                    identityId, isAccessory, locationId, quantity);
            }
        }

        public async Task ReserveStockBatchAsync(Guid locationId, IEnumerable<StockReservationItem> items)
        {
            if (items == null || !items.Any()) return;

            await _uow.ExecuteInTransactionAsync(async () =>
            {
                foreach (var item in items)
                {
                    var inv = await _uow.Inventories.GetByLocationAndIdentityAsync(locationId, item.IdentityId, item.IsAccessory);

                    if (inv == null)
                    {
                        throw new BadRequestException($"Inventory record not found for {item.ProductName}.");
                    }

                    if ((inv.OnHand - inv.Reserved) < item.Quantity)
                    {
                        throw new BadRequestException($"Insufficient stock for {item.ProductName}. (Available: {inv.OnHand - inv.Reserved}, Requested: {item.Quantity})");
                    }

                    inv.Reserved += item.Quantity;
                    inv.UpdatedAt = DateTime.UtcNow;
                    _uow.Inventories.Update(inv);
                }

                await _uow.SaveChangesAsync();
            });
        }
    }
}
