using AutoMapper;
using DesignABear.Repositories.Models;
using DesignABear.Repositories.UnitOfWork;
using DesignABear.Services.DTOs.Requests;
using DesignABear.Services.DTOs.Responses;
using DesignABear.Services.Exceptions;
using DesignABear.Services.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Npgsql;
using Microsoft.EntityFrameworkCore;

namespace DesignABear.Services.Services.Implementations
{
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private static readonly TimeSpan _lockTimeout = TimeSpan.FromSeconds(10);

        public CartService(IUnitOfWork uow, IMapper mapper, IMemoryCache cache)
        {
            _uow = uow;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<CartResponse?> GetWithItemsAsync(Guid cartId)
        {
            var cart = await _uow.Carts.GetWithItemsAsync(cartId);
            if (cart == null) throw new NotFoundException("Cart", cartId);
            
            var response = _mapper.Map<CartResponse>(cart);
            await EnrichStocksAsync(cart, response);
            
            return response;
        }

        public async Task<CartResponse?> GetActiveCartByUserAsync(Guid userId)
        {
            var cart = await _uow.Carts.GetActiveCartByUserAsync(userId);
            if (cart == null) return null;

            // Diagnostics log
            if (cart.CartItems != null)
            {
                foreach (var item in cart.CartItems)
                {
                    Console.WriteLine($"[BackendDebug] Item {item.CartItemId}: Variant={item.VariantId != null}, ProductLoaded={item.Variant?.Product != null}, Build={item.BuildId != null}, BaseProductLoaded={item.Build?.BaseProductId != null}");
                }
            }

            var response = _mapper.Map<CartResponse>(cart);
            
            // Manual Enrichment Fallback (Final Stand against null names)
            if (response.CartItems != null && cart.CartItems != null)
            {
                foreach (var itemResponse in response.CartItems)
                {
                    var sourceItem = cart.CartItems.FirstOrDefault(ci => ci.CartItemId == itemResponse.CartItemId);
                    if (sourceItem != null)
                    {
                        // 1. Ensure BuildDetails is definitely mapped as an object
                        if (itemResponse.BuildDetails == null && sourceItem.Build != null)
                        {
                            itemResponse.BuildDetails = _mapper.Map<BuildResponse>(sourceItem.Build);
                        }

                        // 2. Fallback: If ProductName is STILL null, force-fetch from database by ID
                        if (string.IsNullOrEmpty(itemResponse.ProductName))
                        {
                            // Try to get ProductId from variant or build
                            var productId = sourceItem.Variant?.ProductId ?? sourceItem.Build?.BaseProductId;
                            
                            if (productId.HasValue)
                            {
                                var product = await _uow.Products.GetWithDetailsAsync(productId.Value);
                                if (product != null)
                                {
                                    Console.WriteLine($"[BackendDebug] Force-fetched product {product.Name}, MediaCount={product.Media?.Count ?? 0}");
                                    
                                    itemResponse.ProductName = product.Name;
                                    itemResponse.ProductImageUrl = product.Media?.FirstOrDefault()?.Url;
                                    itemResponse.ProductSlug = product.Slug;

                                    if (itemResponse.BuildDetails != null)
                                    {
                                        itemResponse.BuildDetails.BaseProductName = product.Name;
                                        itemResponse.BuildDetails.BaseProductImageUrl = product.Media?.FirstOrDefault()?.Url;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            await EnrichStocksAsync(cart, response);

            return response;
        }

        private async Task EnrichStocksAsync(Cart cart, CartResponse response)
        {
            if (cart.CartItems == null || response.CartItems == null || !cart.CartItems.Any()) return;

            var identities = new HashSet<(Guid id, bool isAccessory)>();
            
            // Optimization: Fetch Smart Chip ID once if needed
            Guid? smartChipId = null;
            if (cart.CartItems.Any(i => i.IncludesSmartChip))
            {
                var chip = await _uow.Accessories.GetQueryable().FirstOrDefaultAsync(a => a.Sku == "SMART-CHIP-001");
                smartChipId = chip?.AccessoryId;
            }

            foreach (var item in cart.CartItems)
            {
                if (item.VariantId.HasValue) identities.Add((item.VariantId.Value, false));
                else if (item.ProductId.HasValue) identities.Add((item.ProductId.Value, false));
                
                if (item.Build != null)
                {
                    if (item.Build.BaseVariantId.HasValue) identities.Add((item.Build.BaseVariantId.Value, false));
                    else identities.Add((item.Build.BaseProductId, false)); // Fallback to aggregate product stock

                    foreach (var comp in item.Build.BuildComponents ?? Enumerable.Empty<BuildComponent>())
                    {
                        identities.Add((comp.OptionVariantId ?? comp.OptionProductId, true));
                    }
                }

                if (item.IncludesSmartChip && smartChipId.HasValue)
                {
                    identities.Add((smartChipId.Value, true));
                }
            }

            // 2. Fetch total available stock for all identities in one batch fetch
            var stockMap = identities.Any() 
                ? await _uow.Inventories.GetTotalAvailableBatchAsync(identities)
                : new Dictionary<Guid, int>();

            // 3. Map stock totals back to the response items, calculating bottleneck capacity for builds
            foreach (var respItem in response.CartItems)
            {
                var originalItem = cart.CartItems.FirstOrDefault(ci => ci.CartItemId == respItem.CartItemId);
                if (originalItem == null) continue;

                if (originalItem.VariantId.HasValue && !originalItem.BuildId.HasValue)
                {
                    // Standalone Product Variant
                    respItem.AvailableStock = stockMap.GetValueOrDefault(originalItem.VariantId.Value, 0);
                }
                else if (originalItem.ProductId.HasValue && !originalItem.BuildId.HasValue)
                {
                    // Standalone Product (No specific variant)
                    respItem.AvailableStock = stockMap.GetValueOrDefault(originalItem.ProductId.Value, 0);
                }
                else if (originalItem.Build != null)
                {
                    // Custom Build: Bottleneck equals the scarcest component's fulfillment capacity
                    var baseIdentity = originalItem.Build.BaseVariantId ?? originalItem.Build.BaseProductId;
                    int minAvailable = stockMap.GetValueOrDefault(baseIdentity, 0);

                    foreach (var comp in originalItem.Build.BuildComponents ?? Enumerable.Empty<BuildComponent>())
                    {
                        var identityId = comp.OptionVariantId ?? comp.OptionProductId;
                        int totalInStock = stockMap.GetValueOrDefault(identityId, 0);
                        int canFulfill = comp.Quantity > 0 ? totalInStock / comp.Quantity : 0;
                        
                        if (canFulfill < minAvailable) minAvailable = canFulfill;
                    }
                    
                    respItem.AvailableStock = Math.Max(0, minAvailable);
                }
            }
        }

        public async Task<CartResponse> CreateCartAsync(CreateCartRequest request)
        {
            var cart = _mapper.Map<Cart>(request);
            cart.CartId = Guid.NewGuid();
            cart.CreatedAt = DateTime.UtcNow;
            cart.UpdatedAt = DateTime.UtcNow;
            
            await _uow.Carts.AddAsync(cart);
            await _uow.SaveChangesAsync();
            
            return _mapper.Map<CartResponse>(cart);
        }

        public async Task<CartItemResponse> AddItemAsync(CreateCartItemRequest request)
        {
            if (request.CartId == null || request.CartId == Guid.Empty)
            {
                throw new ArgumentException("CartId is required for this operation.");
            }

            // IMPORTANT:
            // Keep add-to-cart permissive: user can add item even when no accessory is selected.
            // Do not enforce accessory-required validation here.

            // Memory-Safe Keyed Lock to prevent double-clicks/race conditions at the app level
            string lockKey = $"lock:cart:{request.CartId}:{request.VariantId ?? request.BuildId}";
            var semaphore = _cache.GetOrCreate(lockKey, entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromMinutes(2);
                entry.RegisterPostEvictionCallback((key, value, reason, state) =>
                {
                    if (value is SemaphoreSlim s) s.Dispose();
                });
                return new SemaphoreSlim(1, 1);
            })!;

            if (!await semaphore.WaitAsync(_lockTimeout))
            {
                throw new TimeoutException("Could not acquire cart lock. Please try again.");
            }

            try
            {
                // --- NEW: Normalize IDs BEFORE checking for existing items ---
                // This is critical because the incoming request might have IDs in the wrong fields
                // but the database has them normalized.
                var resolvedItem = new CartItem 
                { 
                    ProductId = request.ProductId, 
                    VariantId = request.VariantId, 
                    BuildId = request.BuildId 
                };
                var resolvedProductId = await ResolveValidProductIdAsync(resolvedItem);
                var resolvedVariantId = resolvedItem.VariantId;

                // 1. Initial Attempt: Fetch and Update (Optimistic case)
                var existingItems = await _uow.CartItems.FindAsync(ci =>
                    ci.CartId == request.CartId &&
                    ci.ProductId == resolvedProductId &&
                    ci.VariantId == resolvedVariantId &&
                    ci.BuildId == request.BuildId &&
                    ci.IncludesSmartChip == request.IncludesSmartChip);

                var existingItem = existingItems.FirstOrDefault();

                if (existingItem != null)
                {
                    existingItem.Quantity += request.Quantity;
                    _uow.CartItems.Update(existingItem);

                    await _uow.Carts.UpdateTimestampAsync(request.CartId.Value);
                    await _uow.SaveChangesAsync();

                    var enrichedItem = await _uow.CartItems.GetWithEnrichmentAsync(existingItem.CartItemId);
                    var sourceItem = enrichedItem ?? existingItem;
                    var response = _mapper.Map<CartItemResponse>(sourceItem);
                    await ManualEnrichResponseAsync(new[] { sourceItem }, new[] { response });
                    await EnrichItemStockAsync(sourceItem, response);
                    return response;
                }

                // 2. Add Attempt with Race Condition Protection (via Unique Index)
                var newItem = _mapper.Map<CartItem>(request);
                newItem.CartItemId = Guid.NewGuid();

                // Use the IDs we already resolved at the top
                newItem.ProductId = resolvedProductId;
                newItem.VariantId = resolvedVariantId;

                // If ProductId is Guid.Empty (e.g. sent by frontend as 0000...), force it to null 
                // to avoid FK violations (since 0000... is unlikely to exist in the products table).
                if (newItem.ProductId == Guid.Empty) newItem.ProductId = null;

                try
                {
                    await _uow.CartItems.AddAsync(newItem);
                    await _uow.Carts.UpdateTimestampAsync(request.CartId.Value);
                    await _uow.SaveChangesAsync();

                    var enrichedItem = await _uow.CartItems.GetWithEnrichmentAsync(newItem.CartItemId);
                    var sourceItem = enrichedItem ?? newItem;
                    var response = _mapper.Map<CartItemResponse>(sourceItem);
                    await ManualEnrichResponseAsync(new[] { sourceItem }, new[] { response });
                    await EnrichItemStockAsync(sourceItem, response);
                    return response;
                }
                catch (DbUpdateException ex) when (ex.InnerException is PostgresException { SqlState: "23505" })
                {
                    // Another thread/request inserted first. 
                    // CRITICAL: Detach the failed entity to avoid Change Tracker conflicts
                    _uow.CartItems.Detach(newItem);

                    var raceConditionItem = (await _uow.CartItems.FindAsync(ci =>
                        ci.CartId == request.CartId &&
                        ci.ProductId == resolvedProductId &&
                        ci.VariantId == resolvedVariantId &&
                        ci.BuildId == request.BuildId &&
                        ci.IncludesSmartChip == request.IncludesSmartChip)).First();

                    raceConditionItem.Quantity += request.Quantity;
                    _uow.CartItems.Update(raceConditionItem);
                    await _uow.SaveChangesAsync();

                    var enrichedItem = await _uow.CartItems.GetWithEnrichmentAsync(raceConditionItem.CartItemId);
                    var sourceItem = enrichedItem ?? raceConditionItem;
                    var response = _mapper.Map<CartItemResponse>(sourceItem);
                    await ManualEnrichResponseAsync(new[] { sourceItem }, new[] { response });
                    await EnrichItemStockAsync(sourceItem, response);
                    return response;
                }
            }
            finally
            {
                semaphore.Release();
            }
        }

        public async Task<CartItemResponse> AddItemToActiveCartAsync(Guid userId, CreateCartItemRequest request)
        {
            var cart = await _uow.Carts.GetActiveCartByUserAsync(userId);
            
            if (cart == null)
            {
                cart = new Cart
                {
                    CartId = Guid.NewGuid(),
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                await _uow.Carts.AddAsync(cart);
                // We need to save here so we have a valid CartId for the item
                await _uow.SaveChangesAsync();
            }

            cart.UpdatedAt = DateTime.UtcNow;
            
            // If the item has a BuildId, we should ensure the Build record has the requested size details
            if (request.BuildId.HasValue)
            {
                var build = await _uow.Builds.GetByIdAsync(request.BuildId.Value);
                if (build != null)
                {
                    bool isModified = false;
                    if (!string.IsNullOrEmpty(request.SizeTag) && build.SizeTag != request.SizeTag)
                    {
                        build.SizeTag = request.SizeTag;
                        isModified = true;
                    }
                    if (!string.IsNullOrEmpty(request.SizeDescription) && build.SizeDescription != request.SizeDescription)
                    {
                        build.SizeDescription = request.SizeDescription;
                        isModified = true;
                    }

                    // Attempt to resolve BaseVariantId if missing or mismatched
                    if (!string.IsNullOrEmpty(build.SizeTag) && (build.BaseVariantId == null || build.BaseVariantId == Guid.Empty))
                    {
                        var product = await _uow.Products.GetFullDetailsAsync(build.BaseProductId);
                        if (product != null && product.Variants != null)
                        {
                            var matchingVariant = product.Variants.FirstOrDefault(v => 
                                v.SizeTag?.Equals(build.SizeTag, StringComparison.OrdinalIgnoreCase) == true);
                            
                            if (matchingVariant != null)
                            {
                                build.BaseVariantId = matchingVariant.VariantId;
                                isModified = true;
                            }
                        }
                    }

                    if (isModified)
                    {
                        build.UpdatedAt = DateTime.UtcNow;
                        _uow.Builds.Update(build);
                    }
                }
            }

            request.CartId = cart.CartId;
            return await AddItemAsync(request);
        }

        public async Task UpdateItemQuantityAsync(Guid cartItemId, UpdateCartItemRequest request)
        {
            var item = await _uow.CartItems.GetByIdAsync(cartItemId);
            if (item == null) throw new NotFoundException("CartItem", cartItemId);
            
            if (request.Quantity <= 0)
            {
                _uow.CartItems.Remove(item);
            }
            else
            {
                item.Quantity = request.Quantity;
                _uow.CartItems.Update(item);
            }
            
            await _uow.Carts.UpdateTimestampAsync(item.CartId);
            await _uow.SaveChangesAsync();
        }

        public async Task RemoveItemAsync(Guid cartItemId)
        {
            var item = await _uow.CartItems.GetByIdAsync(cartItemId);
            if (item is not null)
            {
                _uow.CartItems.Remove(item);
                await _uow.Carts.UpdateTimestampAsync(item.CartId);
                await _uow.SaveChangesAsync();
            }
        }

        public async Task ClearCartAsync(Guid cartId)
        {
            var items = await _uow.CartItems.FindAsync(ci => ci.CartId == cartId);
            if (items != null && items.Any())
            {
                _uow.CartItems.RemoveRange(items);
                await _uow.Carts.UpdateTimestampAsync(cartId);
                await _uow.SaveChangesAsync();
            }
        }

        private async Task EnrichItemStockAsync(CartItem sourceItem, CartItemResponse response)
        {
            var identities = new HashSet<(Guid id, bool isAccessory)>();

            if (sourceItem.VariantId.HasValue) identities.Add((sourceItem.VariantId.Value, false));
            else if (sourceItem.ProductId.HasValue) identities.Add((sourceItem.ProductId.Value, false));
            
            if (sourceItem.Build != null)
            {
                if (sourceItem.Build.BaseVariantId.HasValue) identities.Add((sourceItem.Build.BaseVariantId.Value, false));
                else identities.Add((sourceItem.Build.BaseProductId, false)); // Fallback

                foreach (var comp in sourceItem.Build.BuildComponents ?? Enumerable.Empty<BuildComponent>())
                {
                    identities.Add((comp.OptionVariantId ?? comp.OptionProductId, true));
                }
            }

            var stockMap = await _uow.Inventories.GetTotalAvailableBatchAsync(identities);

            if (sourceItem.VariantId.HasValue && !sourceItem.BuildId.HasValue)
            {
                response.AvailableStock = stockMap.GetValueOrDefault(sourceItem.VariantId.Value, 0);
                return;
            }

            if (sourceItem.ProductId.HasValue && !sourceItem.BuildId.HasValue)
            {
                response.AvailableStock = stockMap.GetValueOrDefault(sourceItem.ProductId.Value, 0);
                return;
            }

            if (sourceItem.Build != null)
            {
                var baseId = sourceItem.Build.BaseVariantId ?? sourceItem.Build.BaseProductId;
                int min = stockMap.GetValueOrDefault(baseId, 0);

                foreach (var comp in sourceItem.Build.BuildComponents ?? Enumerable.Empty<BuildComponent>())
                {
                    var stock = stockMap.GetValueOrDefault(comp.OptionVariantId ?? comp.OptionProductId, 0);
                    int canFulfill = comp.Quantity > 0 ? stock / comp.Quantity : 0;
                    if (canFulfill < min) min = canFulfill;
                }
                response.AvailableStock = Math.Max(0, min);
                return;
            }

            response.AvailableStock = 0;
        }

        private async Task<Guid?> ResolveValidProductIdAsync(CartItem newItem)
        {
            // Case 1: Resolve from VariantId field
            if (newItem.VariantId.HasValue && newItem.VariantId.Value != Guid.Empty)
            {
                var id = newItem.VariantId.Value;
                
                // 1a. Check if it's a real Variant
                var variant = await _uow.ProductVariants.GetByIdAsync(id);
                if (variant != null) return variant.ProductId;

                // 1b. Check if it's actually a Product ID sent in the wrong field
                var product = await _uow.Products.GetByIdAsync(id);
                if (product != null)
                {
                    newItem.ProductId = product.ProductId;
                    newItem.VariantId = null; // Clean up the entity
                    return product.ProductId;
                }
            }

            // Case 2: Resolve from ProductId field (Double Check)
            if (newItem.ProductId.HasValue && newItem.ProductId.Value != Guid.Empty)
            {
                var id = newItem.ProductId.Value;

                // 2a. Check if it's a real Product
                var product = await _uow.Products.GetByIdAsync(id);
                if (product != null) return product.ProductId;

                // 2b. Check if it's actually a Variant ID sent in the ProductId field
                var variant = await _uow.ProductVariants.GetByIdAsync(id);
                if (variant != null)
                {
                    newItem.VariantId = variant.VariantId;
                    newItem.ProductId = variant.ProductId;
                    return variant.ProductId;
                }
            }

            // Case 3: Resolve from BuildId
            if (newItem.BuildId.HasValue && newItem.BuildId.Value != Guid.Empty)
            {
                var build = await _uow.Builds.GetByIdAsync(newItem.BuildId.Value);
                if (build != null) return build.BaseProductId;
            }

            return null;
        }
        private async Task ManualEnrichResponseAsync(IEnumerable<CartItem> sourceItems, IEnumerable<CartItemResponse> responses)
        {
            var sourceList = sourceItems.ToList();
            var responseList = responses.ToList();

            for (int i = 0; i < sourceList.Count; i++)
            {
                var src = sourceList[i];
                var resp = responseList[i];

                // If mapping failed to populate Name/Image due to missing navigation, force-fetch it
                if (string.IsNullOrEmpty(resp.ProductName) || string.IsNullOrEmpty(resp.ProductImageUrl) || resp.ProductId == null)
                {
                    // Use the resolved ProductId if available, fallback to entity's ProductId
                    var targetProductId = resp.ProductId ?? src.ProductId;

                    if (targetProductId.HasValue && targetProductId.Value != Guid.Empty)
                    {
                        var product = await _uow.Products.GetWithDetailsAsync(targetProductId.Value);
                        if (product != null)
                        {
                            resp.ProductId ??= product.ProductId;
                            resp.ProductName ??= product.Name;
                            resp.ProductSlug ??= product.Slug;
                            resp.ProductImageUrl ??= product.Media?.FirstOrDefault()?.Url;
                            resp.Sku ??= product.Sku;
                        }
                    }
                }
            }
        }
    }
}
