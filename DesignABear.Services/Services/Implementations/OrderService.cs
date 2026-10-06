using AutoMapper;
using DesignABear.Repositories.Models;
using DesignABear.Repositories.UnitOfWork;
using DesignABear.Services.DTOs.Requests;
using DesignABear.Services.DTOs.Responses;
using DesignABear.Services.Exceptions;
using DesignABear.Services.Services.Interfaces;
using DesignABear.Repositories.Paging;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;
using DesignABear.Services.Configurations;
using Microsoft.EntityFrameworkCore;

namespace DesignABear.Services.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IInventoryService _inventoryService;
        private readonly IPromotionService _promotionService;
        private readonly ICartService _cartService;
        private readonly IGhtkShippingService _ghtkShippingService;
        private readonly ILogger<OrderService> _logger;
        private readonly BusinessSettings _businessSettings;

        public OrderService(
            IUnitOfWork uow, 
            IMapper mapper, 
            IInventoryService inventoryService,
            IPromotionService promotionService,
            ICartService cartService,
            IGhtkShippingService ghtkShippingService, 
            ILogger<OrderService> logger,
            IOptions<BusinessSettings> businessSettings)
        {
            _uow = uow;
            _mapper = mapper;
            _inventoryService = inventoryService;
            _promotionService = promotionService;
            _cartService = cartService;
            _ghtkShippingService = ghtkShippingService;
            _logger = logger;
            _businessSettings = businessSettings.Value;
        }

        public async Task<PagedResult<OrderResponse>> GetOrdersAsync(int pageIndex, int pageSize, int? roleId = null)
        {
            string[]? statuses = null;

            if (roleId == RoleConstants.Staff) // Staff
            {
                statuses = new[] 
                { 
                    OrderStatusConstants.Pending, 
                    OrderStatusConstants.Processing, 
                    OrderStatusConstants.Printing,
                    OrderStatusConstants.ReadyForPickup, 
                    OrderStatusConstants.Shipping, 
                    OrderStatusConstants.Cancelled, 
                    OrderStatusConstants.Rejected 
                };
            }

            var pagedOrders = await _uow.Orders.GetPagedOrdersAsync(pageIndex, pageSize, statuses);
            var responses = _mapper.Map<List<OrderResponse>>(pagedOrders.Items);
            
            return new PagedResult<OrderResponse>
            {
                PageIndex = pagedOrders.PageIndex,
                PageSize = pagedOrders.PageSize,
                TotalCount = pagedOrders.TotalCount,
                Items = responses
            };
        }

        public async Task<OrderResponse?> GetByIdAsync(Guid orderId)
        {
            var order = await _uow.Orders.GetByIdAsync(orderId);
            if (order == null) throw new NotFoundException("Order", orderId);
            return _mapper.Map<OrderResponse>(order);
        }

        public async Task<OrderResponse?> GetByOrderNumberAsync(string orderNumber)
        {
            var order = await _uow.Orders.GetByOrderNumberAsync(orderNumber);
            if (order == null) throw new NotFoundException("Order", orderNumber);
            return _mapper.Map<OrderResponse>(order);
        }

        public async Task<OrderResponse?> GetWithDetailsAsync(Guid orderId)
        {
            var order = await _uow.Orders.GetWithDetailsAsync(orderId);
            if (order == null) throw new NotFoundException("Order", orderId);
            return _mapper.Map<OrderResponse>(order);
        }

        public async Task<IEnumerable<OrderResponse>> GetByUserIdAsync(Guid userId)
        {
            var orders = await _uow.Orders.GetByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<OrderResponse>>(orders);
        }

        public async Task<IEnumerable<OrderResponse>> GetByStatusAsync(string status)
        {
            var orders = await _uow.Orders.GetByStatusAsync(status);
            return _mapper.Map<IEnumerable<OrderResponse>>(orders);
        }

        public async Task<OrderResponse> CreateFromCartAsync(Guid cartId, CreateOrderRequest request)
        {
            var cart = await _uow.Carts.GetWithItemsAsync(cartId);
            if (cart is null) throw new NotFoundException("Cart", cartId);

            // Use transaction for atomic operation
            Order orderResult = null!;
            await _uow.ExecuteInTransactionAsync(async () =>
            {
                var warehouseId = _businessSettings.WarehouseGuid;

                // 1. Prepare Batch Stock Reservation List
                var stockItems = new List<StockReservationItem>();
                foreach (var ci in cart.CartItems)
                {
                    var hasAiProcessor = ResolveHasAiProcessor(ci);
                    // 1.a Direct Variant Item
                    if (ci.VariantId.HasValue)
                    {
                        stockItems.Add(new StockReservationItem
                        {
                            ProductId = ci.Variant?.ProductId ?? Guid.Empty,
                            IdentityId = ci.VariantId.Value,
                            IsAccessory = false, // Standalone item in cart
                            Quantity = ci.Quantity,
                            ProductName = ci.Variant?.Product?.Name ?? ci.Variant?.Sku
                        });
                    }

                    // 1.b Custom Build Item
                    if (ci.BuildId.HasValue && ci.Build != null)
                    {
                        // Base product (Variant)
                        if (ci.Build.BaseVariantId.HasValue)
                        {
                            stockItems.Add(new StockReservationItem
                            {
                                ProductId = ci.Build.BaseProductId,
                                IdentityId = ci.Build.BaseVariantId.Value,
                                IsAccessory = false,
                                Quantity = ci.Quantity,
                                ProductName = $"Base for {ci.Build.BuildName}"
                            });
                        }

                        // Components (Usually accessories)
                        if (ci.Build.BuildComponents != null)
                        {
                            foreach (var comp in ci.Build.BuildComponents)
                            {
                                var identityId = comp.OptionVariantId ?? comp.OptionProductId;
                                stockItems.Add(new StockReservationItem
                                {
                                    ProductId = comp.OptionProductId,
                                    IdentityId = identityId,
                                    IsAccessory = true, // Components are identities in the accessory sense
                                    Quantity = comp.Quantity * ci.Quantity,
                                    ProductName = $"Component: {comp.OptionProduct?.Name}"
                                });
                            }
                        }
                    }

                    // 1.c Smart Chip (If included)
                    if (hasAiProcessor)
                    {
                        var smartChip = await _uow.Accessories.GetQueryable()
                            .FirstOrDefaultAsync(a => a.Sku == "SMART-CHIP-001" && a.IsActive);
                        if (smartChip != null)
                        {
                            stockItems.Add(new StockReservationItem
                            {
                                IdentityId = smartChip.AccessoryId,
                                IsAccessory = true,
                                Quantity = ci.Quantity,
                                ProductName = "AI Smart Chip"
                            });
                        }
                    }
                }

                // 2. Batch "Check & Reserve" - Atomic and efficient
                await _inventoryService.ReserveStockBatchAsync(warehouseId, stockItems);

                // 3. Create Order
                var order = _mapper.Map<Order>(request);
                order.OrderId = Guid.NewGuid();
                order.OrderNumber = $"ORD-{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid().ToString()[..4].ToUpper()}";
                order.UserId = request.UserId;
                order.Status = OrderStatusConstants.Pending;
                order.ShippingAddressId = request.ShippingAddressId;
                order.BillingAddressId = request.BillingAddressId;
                order.CreatedAt = DateTime.UtcNow;
                order.UpdatedAt = DateTime.UtcNow;

                decimal calculatedSubtotal = 0;

                foreach (var ci in cart.CartItems)
                {
                    var hasAiProcessor = ResolveHasAiProcessor(ci);
                    var variant = ci.Variant;
                    var basePrice = ci.Variant?.Price ?? 0;
                    var unitPrice = ci.UnitPriceSnapshot ?? basePrice;
                    
                    if (hasAiProcessor && !ci.BuildId.HasValue && variant != null && variant.Product != null)
                    {
                        var smartChip = await _uow.Accessories.GetQueryable()
                            .FirstOrDefaultAsync(a => a.Sku == "SMART-CHIP-001" && a.IsActive);
                        unitPrice += smartChip?.TargetPrice ?? 999000;
                    }

                    var lineTotal = unitPrice * ci.Quantity;
                    
                    float itemWeight = 0;
                    if (ci.BuildId.HasValue && ci.Build != null)
                    {
                        itemWeight = ci.Build.TotalWeightGram ?? 500f;
                    }
                    else if (ci.Variant != null)
                    {
                        itemWeight = ci.Variant.WeightGram == 0 ? 500f : ci.Variant.WeightGram;
                    }
                    else
                    {
                        itemWeight = 500f;
                    }

                    calculatedSubtotal += lineTotal;
                    var orderItem = new OrderItem
                    {
                        OrderItemId = Guid.NewGuid(),
                        VariantId = ci.VariantId,
                        BuildId = ci.BuildId,
                        Quantity = ci.Quantity,
                        UnitPrice = unitPrice,
                        LineTotal = lineTotal,
                        WeightSnapshot = itemWeight,
                        IncludesSmartChip = hasAiProcessor,
                        ProductNameSnapshot = ci.Variant != null && ci.Variant.Product != null 
                            ? $"{ci.Variant.Product.Name} ({ci.Variant.SizeTag ?? ci.Variant.Sku})" 
                            : "Custom Design",
                        PersonalizationSnapshot = ci.Build?.PersonalizationNote 
                    };

                    decimal assemblyCost = ci.Variant?.AssemblyCost ?? ci.Build?.BaseVariant?.AssemblyCost ?? 0m;
                    if (assemblyCost == 0) assemblyCost = 50000m;

                    for (int i = 0; i < ci.Quantity; i++)
                    {
                        orderItem.ProductionJobs.Add(new ProductionJob
                        {
                            JobId = Guid.NewGuid(),
                            OrderItemId = orderItem.OrderItemId,
                            BuildId = orderItem.BuildId,
                            SerialNumber = $"SN-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..8].ToUpper()}",
                            Status = ProductionJobStatusConstants.PendingPayment,
                            HasSmartChip = hasAiProcessor,
                            Priority = PriorityConstants.Normal,
                            UpdatedAt = DateTime.UtcNow,
                            TotalAssemblyCost = assemblyCost,
                            CraftsmanCommission = assemblyCost * 0.8m,
                            QCCommission = assemblyCost * 0.2m,
                            PrintData = JsonSerializer.Serialize(new { personalization = orderItem.PersonalizationSnapshot, aiProcessor = hasAiProcessor })
                        });
                    }

                    order.OrderItems.Add(orderItem);
                }

                order.Subtotal = calculatedSubtotal;

                if (request.ShippingAddressId.HasValue)
                {
                    var shippingResult = await _ghtkShippingService.CalculateFeeForCartAsync(cartId, request.ShippingAddressId.Value);
                    if (shippingResult.IsFailure)
                    {
                        throw new BadRequestException($"Shipping calculation failed: {shippingResult.Error.Description}");
                    }
                    order.ShippingTotal = (decimal)(shippingResult.Value?.Fee?.Fee ?? 0);
                }
                else
                {
                    order.ShippingTotal = 0;
                }

                decimal aggregateDiscount = 0;
                var validatedCodes = new List<(string code, decimal discount)>();

                if (request.PromoCodes != null && request.PromoCodes.Any())
                {
                    if (!order.UserId.HasValue) throw new BadRequestException("User ID is required to apply promotions.");

                    foreach (var code in request.PromoCodes.Distinct())
                    {
                        try
                        {
                            var res = await _promotionService.ApplyPromotionAsync(code, order.UserId.Value, order.Subtotal, order.ShippingTotal);
                            _logger.LogInformation("Applied promotion '{Code}' to Order {OrderNumber}: ProductDiscount={ProductDiscount}, ShippingDiscount={ShippingDiscount}, Total={Total}", 
                                code, order.OrderNumber, res.ProductDiscount, res.ShippingDiscount, res.TotalDiscount);
                            aggregateDiscount += res.TotalDiscount;
                            validatedCodes.Add((code, res.TotalDiscount));
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning("Failed to apply promotion code '{Code}' to Order {OrderNumber}: {Message}", code, order.OrderNumber, ex.Message);
                            throw new BadRequestException($"Could not apply promotion code '{code}': {ex.Message}");
                        }
                    }
                    order.DiscountTotal = aggregateDiscount;
                }
                else
                {
                    _logger.LogInformation("No promo codes provided for Order {OrderNumber}. Using request DiscountTotal: {DiscountTotal}", order.OrderNumber, request.DiscountTotal);
                    order.DiscountTotal = request.DiscountTotal;
                }

                _logger.LogInformation("Final calculation for Order {OrderNumber}: Subtotal={Subtotal}, Shipping={Shipping}, Discount={Discount}, GrandTotal={GrandTotal}", 
                    order.OrderNumber, order.Subtotal, order.ShippingTotal, order.DiscountTotal, 
                    order.Subtotal + order.ShippingTotal + order.TaxTotal - order.DiscountTotal);

                order.GrandTotal = order.Subtotal + order.ShippingTotal + order.TaxTotal - order.DiscountTotal;

                await _uow.Orders.AddAsync(order);
                // _uow.CartItems.RemoveRange(cart.CartItems); // Keep items until successful payment
                await _uow.SaveChangesAsync();

                if (validatedCodes.Any() && order.UserId.HasValue)
                {
                    foreach (var (code, discount) in validatedCodes)
                    {
                        await _promotionService.UsePromotionAsync(code, order.UserId.Value, order.OrderId, discount);
                    }
                }
                orderResult = order;
            });

            return _mapper.Map<OrderResponse>(orderResult);

        }

        private bool ResolveHasAiProcessor(CartItem item)
        {
            if (item.IncludesSmartChip || item.Build?.IncludesSmartChip == true)
            {
                return true;
            }

            if (item.Build?.BuildComponents == null)
            {
                return false;
            }

            return item.Build.BuildComponents.Any(c =>
                c.OptionProductId == _businessSettings.AiProcessorGuid ||
                string.Equals(c.OptionProduct?.Sku, "CORE-ESP32-AI", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(c.OptionProduct?.Name, "AI Processor", StringComparison.OrdinalIgnoreCase));
        }

        public async Task UpdateStatusAsync(Guid orderId, UpdateOrderRequest request, Guid? updatedBy = null)
        {
            var order = await _uow.Orders.GetWithDetailsAsync(orderId);
            if (order is null) throw new NotFoundException("Order", orderId);

            var oldStatus = order.Status;
            order.Status = request.Status;
            order.Notes = request.Notes;
            order.UpdatedAt = DateTime.UtcNow;
            order.UpdatedBy = updatedBy;

            // Handle stock release on cancellation or rejection if it was previously reserved
            if ((request.Status == OrderStatusConstants.Cancelled || request.Status == OrderStatusConstants.Rejected) 
                && oldStatus != OrderStatusConstants.Cancelled && oldStatus != OrderStatusConstants.Rejected)
            {
                var warehouseId = _businessSettings.WarehouseGuid;

                foreach (var item in order.OrderItems)
                {
                    if (item.VariantId.HasValue)
                    {
                        await _inventoryService.ReleaseReservationAsync(item.VariantId.Value, false, item.Quantity, warehouseId);
                    }

                    if (item.BuildId.HasValue && item.Build != null)
                    {
                        if (item.Build.BaseVariantId.HasValue)
                            await _inventoryService.ReleaseReservationAsync(item.Build.BaseVariantId.Value, false, item.Quantity, warehouseId);

                        foreach (var comp in item.Build.BuildComponents)
                        {
                            var identityId = comp.OptionVariantId ?? comp.OptionProductId;
                            await _inventoryService.ReleaseReservationAsync(identityId, true, comp.Quantity * item.Quantity, warehouseId);
                        }
                    }
                }
            }

            _uow.Orders.Update(order);
            await _uow.SaveChangesAsync();
        }

        public async Task ApplyPromotionAsync(Guid orderId, string promoCode)
        {
            var order = await _uow.Orders.GetWithDetailsAsync(orderId);
            if (order is null) throw new NotFoundException("Order", orderId);
            
            if (!order.UserId.HasValue) throw new BadRequestException("User ID is required.");
            
            var res = await _promotionService.ApplyPromotionAsync(promoCode, order.UserId.Value, order.Subtotal, order.ShippingTotal);

            await _promotionService.UsePromotionAsync(promoCode, order.UserId.Value, orderId, res.TotalDiscount);

            order.DiscountTotal += res.TotalDiscount;
            order.GrandTotal = order.Subtotal - order.DiscountTotal + order.TaxTotal + order.ShippingTotal;
            order.UpdatedAt = DateTime.UtcNow;
            
            _uow.Orders.Update(order);
            await _uow.SaveChangesAsync();
        }

        public async Task CancelAsync(Guid orderId, Guid? updatedBy = null)
        {
            await UpdateStatusAsync(orderId, new UpdateOrderRequest { Status = OrderStatusConstants.Cancelled }, updatedBy);
        }

        public async Task ReorderAsync(Guid orderId, Guid userId)
        {
            var order = await _uow.Orders.GetWithDetailsAsync(orderId);
            if (order == null) throw new NotFoundException("Order", orderId);

            foreach (var item in order.OrderItems)
            {
                if (!item.VariantId.HasValue) continue;

                // Check if the item (or its build components) is still in stock before adding to cart
                bool isInStock = true;
                
                var available = await _inventoryService.GetTotalAvailableAsync(item.VariantId.Value, false);
                if (available < item.Quantity) isInStock = false;

                if (isInStock && item.BuildId.HasValue && item.Build != null)
                {
                    foreach (var comp in item.Build.BuildComponents)
                    {
                        var identityId = comp.OptionVariantId ?? comp.OptionProductId;
                        var accessoryAvailable = await _inventoryService.GetTotalAvailableAsync(identityId, true);
                        if (accessoryAvailable < (comp.Quantity * item.Quantity)) 
                        {
                            isInStock = false;
                            break;
                        }
                    }
                }

                if (!isInStock)
                {
                    // Skip out-of-stock items
                    _logger.LogInformation("Reorder: Skipping out-of-stock item {VariantId} for user {UserId}", item.VariantId, userId);
                    continue;
                }

                var addRequest = new CreateCartItemRequest
                {
                    VariantId = item.VariantId.Value,
                    BuildId = item.BuildId,
                    Quantity = item.Quantity
                };

                try
                {
                    await _cartService.AddItemToActiveCartAsync(userId, addRequest);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Reorder: Failed to add item {VariantId} to cart for user {UserId}", item.VariantId, userId);
                    // We continue to try other items
                }
            }
        }

        public async Task ValidateOrderStockAsync(Guid orderId)
        {
            var order = await _uow.Orders.GetWithDetailsAsync(orderId);
            if (order == null) throw new NotFoundException("Order", orderId);

            await EnsureStockAvailableAsync(order.OrderItems.Select(oi => new StockCheckItem
            {
                IdentityId = oi.VariantId ?? Guid.Empty,
                IsAccessory = false,
                Quantity = oi.Quantity,
                ProductName = oi.ProductNameSnapshot ?? "Product",
                Build = oi.Build
            }));
        }

        private async Task EnsureStockAvailableAsync(IEnumerable<StockCheckItem> items)
        {
            var identities = new HashSet<(Guid id, bool isAccessory)>();
            
            // 1. Gather all required Identities (Variants & Accessories)
            foreach (var item in items)
            {
                if (item.IdentityId != Guid.Empty) identities.Add((item.IdentityId, item.IsAccessory));
                
                foreach (var comp in item.Build?.BuildComponents ?? Enumerable.Empty<BuildComponent>())
                {
                    identities.Add((comp.OptionVariantId ?? comp.OptionProductId, true));
                }
            }

            // 2. Fetch stock levels in one batch
            var stockMap = await _uow.Inventories.GetTotalAvailableBatchAsync(identities);

            // 3. Validate requested quantities against available stock
            var stockErrors = new List<string>();
            foreach (var item in items)
            {
                if (item.IdentityId != Guid.Empty)
                {
                    var available = stockMap.GetValueOrDefault(item.IdentityId, 0);
                    if (available < item.Quantity)
                    {
                        stockErrors.Add($"{item.ProductName} is low on stock (Requested: {item.Quantity}, Available: {available})");
                    }
                }

                foreach (var comp in item.Build?.BuildComponents ?? Enumerable.Empty<BuildComponent>())
                {
                    var id = comp.OptionVariantId ?? comp.OptionProductId;
                    var available = stockMap.GetValueOrDefault(id, 0);
                    var required = comp.Quantity * item.Quantity;
                    
                    if (available < required)
                    {
                        stockErrors.Add($"Accessory '{comp.OptionProduct?.Name ?? "Component"}' for {item.ProductName} is low on stock (Requested: {required}, Available: {available})");
                    }
                }
            }

            if (stockErrors.Any())
            {
                throw new BadRequestException($"Insufficient stock for one or more items: {string.Join("; ", stockErrors)}");
            }
        }

        private class StockCheckItem
        {
            public Guid IdentityId { get; set; }
            public bool IsAccessory { get; set; } = false;
            public int Quantity { get; set; }
            public string ProductName { get; set; } = "";
            public Build? Build { get; set; }
        }
    }
}
