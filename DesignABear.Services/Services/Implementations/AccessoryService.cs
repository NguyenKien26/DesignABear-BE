using AutoMapper;
using DesignABear.Repositories.Models;
using DesignABear.Repositories.UnitOfWork;
using DesignABear.Services.DTOs.Requests;
using DesignABear.Services.DTOs.Responses;
using DesignABear.Services.Services.Interfaces;
using DesignABear.Services.Common;
using Microsoft.EntityFrameworkCore;
using DesignABear.Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DesignABear.Services.Services.Implementations
{
    public class AccessoryService : IAccessoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AccessoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<AccessoryResponse>>> GetAllAsync()
        {
            var accessories = await _unitOfWork.Accessories.GetQueryable()
                .AsNoTracking()
                .ToListAsync();

            var responses = _mapper.Map<List<AccessoryResponse>>(accessories);
            await PopulateStocksAsync(responses);

            return Result<IEnumerable<AccessoryResponse>>.Success(responses);
        }

        public async Task<Result<AccessoryResponse>> GetByIdAsync(Guid id)
        {
            var accessory = await _unitOfWork.Accessories.GetQueryable()
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.AccessoryId == id);

            if (accessory == null)
                return Result<AccessoryResponse>.Failure(new Error("NotFound", "Accessory not found"));

            var response = _mapper.Map<AccessoryResponse>(accessory);
            await PopulateStocksAsync(new List<AccessoryResponse> { response });

            return Result<AccessoryResponse>.Success(response);
        }

        public async Task<Result<IEnumerable<AccessoryResponse>>> GetByProductIdAsync(Guid productId)
        {
            var productAccessories = await _unitOfWork.ProductAccessories.GetQueryable()
                .Include(pa => pa.Accessory)
                .Where(pa => pa.ProductId == productId)
                .ToListAsync();

            var accessories = productAccessories
                .Select(pa => pa.Accessory)
                .Where(a => a != null)
                .ToList();

            var responses = _mapper.Map<List<AccessoryResponse>>(accessories);
            await PopulateStocksAsync(responses);

            return Result<IEnumerable<AccessoryResponse>>.Success(responses);
        }

        private async Task PopulateStocksAsync(IList<AccessoryResponse> responses)
        {
            if (responses.Count == 0) return;

            var ids = responses.Select(x => x.AccessoryId).Distinct().ToList();

            var stockMap = await _unitOfWork.Inventories.GetQueryable()
                .AsNoTracking()
                .Where(i => i.IsAccessory && ids.Contains(i.IdentityId))
                .GroupBy(i => i.IdentityId)
                .Select(g => new
                {
                    AccessoryId = g.Key,
                    OnHand = g.Sum(x => x.OnHand),
                    Reserved = g.Sum(x => x.Reserved)
                })
                .ToDictionaryAsync(x => x.AccessoryId, x => new { x.OnHand, x.Reserved });

            foreach (var item in responses)
            {
                if (stockMap.TryGetValue(item.AccessoryId, out var stock))
                {
                    item.OnHand = stock.OnHand;
                    item.Reserved = stock.Reserved;
                }
                else
                {
                    item.OnHand = 0;
                    item.Reserved = 0;
                }
            }
        }

        public async Task<Result<AccessoryResponse>> CreateAsync(CreateAccessoryRequest request)
        {
            // SKU Uniqueness Check across system (Robust)
            await ValidateAccessorySkuUniquenessAsync(request.Sku);

            var accessory = _mapper.Map<Accessory>(request);
            accessory.CreatedAt = DateTime.UtcNow;
            accessory.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Accessories.AddAsync(accessory);

            // Sync inventory for accessory
            var defaultLocId = await GetOrCreateDefaultLocationIdAsync();
            await _unitOfWork.Inventories.AddAsync(new Inventory
            {
                InventoryId = Guid.NewGuid(),
                LocationId = defaultLocId,
                IdentityId = accessory.AccessoryId,
                IsAccessory = true,
                OnHand = request.StockQuantity,
                UpdatedAt = DateTime.UtcNow
            });

            await _unitOfWork.SaveChangesAsync();
            return Result<AccessoryResponse>.Success(_mapper.Map<AccessoryResponse>(accessory));
        }

        public async Task<Result<AccessoryResponse>> UpdateAsync(Guid id, UpdateAccessoryRequest request)
        {
            var accessory = await _unitOfWork.Accessories.GetByIdAsync(id);
            if (accessory == null)
                return Result<AccessoryResponse>.Failure(new Error("NotFound", "Accessory not found"));

            // Pricing validation: Relaxed
            /*
            decimal totalCost = request.BaseCost + request.AssemblyCost;
            decimal expectedPrice = Math.Ceiling(totalCost * 1.2m / 100000m) * 100000m;

            if (expectedPrice > 0 && Math.Abs(request.TargetPrice - expectedPrice) > 1.0m)
            {
                throw new BadRequestException($"Price mismatch. Expected {expectedPrice}.");
            }
            */

            if (accessory.Sku != request.Sku)
            {
                await ValidateAccessorySkuUniquenessAsync(request.Sku, id);
            }

            _mapper.Map(request, accessory);
            accessory.UpdatedAt = DateTime.UtcNow;
            
            _unitOfWork.Accessories.Update(accessory);

            // Sync inventory for accessory
            var defaultLocId = await GetOrCreateDefaultLocationIdAsync();
            var inv = await _unitOfWork.Inventories.GetQueryable()
                .FirstOrDefaultAsync(i => i.IdentityId == id && i.IsAccessory == true && i.LocationId == defaultLocId);
            
            if (inv != null)
            {
                inv.OnHand = request.StockQuantity;
                inv.UpdatedAt = DateTime.UtcNow;
            }
            else if (request.StockQuantity > 0)
            {
                await _unitOfWork.Inventories.AddAsync(new Inventory
                {
                    InventoryId = Guid.NewGuid(),
                    LocationId = defaultLocId,
                    IdentityId = id,
                    IsAccessory = true,
                    OnHand = request.StockQuantity,
                    UpdatedAt = DateTime.UtcNow
                });
            }

            await _unitOfWork.SaveChangesAsync();
            return Result<AccessoryResponse>.Success(_mapper.Map<AccessoryResponse>(accessory));
        }

        public async Task<Result<bool>> DeleteAsync(Guid id)
        {
            var accessory = await _unitOfWork.Accessories.GetByIdAsync(id);
            if (accessory == null)
                return Result<bool>.Failure(new Error("NotFound", "Accessory not found"));

            _unitOfWork.Accessories.Remove(accessory);
            await _unitOfWork.SaveChangesAsync();
            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> AddToProductAsync(Guid productId, Guid accessoryId)
        {
            var exists = await _unitOfWork.ProductAccessories
                .AnyAsync(pa => pa.ProductId == productId && pa.AccessoryId == accessoryId);

            if (exists) return Result<bool>.Success(true);

            var productAccessory = new ProductAccessory
            {
                ProductId = productId,
                AccessoryId = accessoryId
            };

            // Using context directly or repository if available
            await _unitOfWork.ProductAccessories.AddAsync(productAccessory); 
            await _unitOfWork.SaveChangesAsync();
            
            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> RemoveFromProductAsync(Guid productId, Guid accessoryId)
        {
            var paList = await _unitOfWork.ProductAccessories
                .FindAsync(pa => pa.ProductId == productId && pa.AccessoryId == accessoryId);
            var pa = paList.FirstOrDefault();

            if (pa == null) return Result<bool>.Success(true);

            _unitOfWork.ProductAccessories.Remove(pa);
            await _unitOfWork.SaveChangesAsync();
            
            return Result<bool>.Success(true);
        }

        public async Task<Result<AccessoryResponse>> GetSmartChipAsync()
        {
            var chip = await _unitOfWork.Accessories.GetQueryable()
                .FirstOrDefaultAsync(a => a.Sku == "SMART-CHIP-001");
            
            if (chip == null)
                return Result<AccessoryResponse>.Failure(new Error("Accessory.SmartChipNotFound", "Smart Chip not configured in database."));

            return Result<AccessoryResponse>.Success(_mapper.Map<AccessoryResponse>(chip));
        }

        private async Task ValidateAccessorySkuUniquenessAsync(string sku, Guid? excludeAccessoryId = null)
        {
            if (string.IsNullOrWhiteSpace(sku)) return;
            var trimmedSku = sku.Trim();

            // 1. Check against Product base SKUs
            var productExists = await _unitOfWork.Products.AnyAsync(p => EF.Functions.ILike(p.Sku.Trim(), trimmedSku));
            if (productExists)
                throw new BadRequestException($"Mã SKU '{trimmedSku}' đã trùng với mã định danh của một Sản phẩm chính.");

            // 2. Check against Accessory SKUs
            var accessoryExists = excludeAccessoryId.HasValue
                ? await _unitOfWork.Accessories.AnyAsync(a => EF.Functions.ILike(a.Sku.Trim(), trimmedSku) && a.AccessoryId != excludeAccessoryId.Value)
                : await _unitOfWork.Accessories.AnyAsync(a => EF.Functions.ILike(a.Sku.Trim(), trimmedSku));

            if (accessoryExists)
                throw new BadRequestException($"Mã SKU '{trimmedSku}' đã trùng với mã của một Phụ kiện khác.");

            // 3. Check against ProductVariant SKUs
            var variantExists = await _unitOfWork.ProductVariants.AnyAsync(v => EF.Functions.ILike(v.Sku.Trim(), trimmedSku));
            if (variantExists)
                throw new BadRequestException($"Mã SKU '{trimmedSku}' đã tồn tại ở một biến thể sản phẩm.");
        }

        private async Task<Guid> GetOrCreateDefaultLocationIdAsync()
        {
            var warehouse = await _unitOfWork.Locations.GetQueryable()
                .FirstOrDefaultAsync(l => l.LocationType.ToUpper() == "WAREHOUSE");
            if (warehouse != null) return warehouse.LocationId;

            var first = await _unitOfWork.Locations.GetQueryable().FirstOrDefaultAsync(l => true);
            if (first != null) return first.LocationId;

            var defaultLoc = new Location
            {
                LocationId = Guid.NewGuid(),
                Name = "Kho Tổng (Mặc định)",
                LocationType = "WAREHOUSE",
                AddressText = "Thiết lập sau"
            };
            await _unitOfWork.Locations.AddAsync(defaultLoc);
            return defaultLoc.LocationId;
        }
    }
}
