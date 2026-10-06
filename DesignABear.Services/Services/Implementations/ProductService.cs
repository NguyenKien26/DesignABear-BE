using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using DesignABear.Repositories.Models;
using DesignABear.Repositories.Paging;
using DesignABear.Repositories.UnitOfWork;
using DesignABear.Services.DTOs.Requests;
using DesignABear.Services.DTOs.Responses;
using DesignABear.Services.Exceptions;
using DesignABear.Services.Services.Interfaces;

namespace DesignABear.Services.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly ICacheService _cache;
        
        // Keyed Semaphore to prevent Cache Stampede at product level (avoiding global bottleneck)
        private static readonly System.Collections.Concurrent.ConcurrentDictionary<string, SemaphoreSlim> _locks = new();

        public ProductService(IUnitOfWork uow, IMapper mapper, ICacheService cache)
        {
            _uow = uow;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<PagedResult<ProductListResponse>> GetProductsAsync(int pageIndex, int pageSize, string? sortBy = null, int? roleId = null)
        {
            string cacheKey = $"products:page:{pageIndex}:size:{pageSize}:sort:{sortBy ?? "default"}:role:{roleId ?? 0}";
            
            // Cache only the first 5 pages with 10 items per page
            bool shouldCache = pageIndex <= 5 && pageSize == 10;

            if (shouldCache)
            {
                var cached = await _cache.GetAsync<PagedResult<ProductListResponse>>(cacheKey);
                if (cached != null) return cached;
            }

            bool onlyActive = roleId == 2; // Staff only sees active products

            // FORCED FLUSH: Clear stale data to fix 0 price issue in UI
            if (roleId == 1) 
            {
                 await _cache.FlushAllCachedDataAsync();
            }

            var pagedProducts = await _uow.Products.GetPagedProductsAsync(pageIndex, pageSize, sortBy, onlyActive);

            var result = new PagedResult<ProductListResponse>
            {
                PageIndex = pagedProducts.PageIndex,
                PageSize = pagedProducts.PageSize,
                TotalCount = pagedProducts.TotalCount,
                Items = _mapper.Map<List<ProductListResponse>>(pagedProducts.Items)
            };

            if (shouldCache)
            {
                await _cache.SetAsync(cacheKey, result, TimeSpan.FromMinutes(10));
                
            }

            return result;
        }

        public async Task<List<ProductListResponse>> GetTopProductsAsync(int count = 20)
        {
            string cacheKey = $"products:top:{count}";
            var cached = await _cache.GetAsync<List<ProductListResponse>>(cacheKey);
            if (cached != null) return cached;

            var pagedResult = await _uow.Products.GetPagedProductsAsync(1, count, sortBy: "views_desc");
            var result = _mapper.Map<List<ProductListResponse>>(pagedResult.Items);

            await _cache.SetAsync(cacheKey, result, TimeSpan.FromHours(1));
            return result;
        }

        public async Task<ProductDetailResponse?> GetBySlugAsync(string slug)
        {
            string cacheKey = $"product:slug:{slug}";
            var cached = await _cache.GetAsync<ProductDetailResponse>(cacheKey);
            if (cached != null)
            {
                await IncrementViewCountAsync(cached.ProductId);
                return cached;
            }

            var semaphore = _locks.GetOrAdd(cacheKey, _ => new SemaphoreSlim(1, 1));
            await semaphore.WaitAsync();
            try
            {
                // Double-check cache inside the lock
                cached = await _cache.GetAsync<ProductDetailResponse>(cacheKey);
                if (cached != null) return cached;

                var product = await _uow.Products.GetBySlugAsync(slug);
                if (product == null) throw new NotFoundException("Product", slug);
                
                var response = _mapper.Map<ProductDetailResponse>(product);
                await PopulateAccessoryStocksAsync(response.Accessories);
                await _cache.SetAsync(cacheKey, response, TimeSpan.FromHours(1));
                
                // Also cache by ID for consistency
                await _cache.SetAsync($"product:id:{response.ProductId}", response, TimeSpan.FromHours(1));

                await IncrementViewCountAsync(response.ProductId);
                return response;
            }
            finally
            {
                semaphore.Release();
                // Optionally prune the dictionary if it grows too large, 
                // but Semaphores are light and product count is usually manageable.
            }
        }

        public async Task<ProductDetailResponse?> GetByIdAsync(Guid productId)
        {
            string cacheKey = $"product:id:{productId}";
            var cached = await _cache.GetAsync<ProductDetailResponse>(cacheKey);
            if (cached != null)
            {
                await IncrementViewCountAsync(productId);
                return cached;
            }

            var semaphore = _locks.GetOrAdd(cacheKey, _ => new SemaphoreSlim(1, 1));
            await semaphore.WaitAsync();
            try
            {
                // Double-check cache inside the lock
                cached = await _cache.GetAsync<ProductDetailResponse>(cacheKey);
                if (cached != null) return cached;

                var product = await _uow.Products.GetWithDetailsAsync(productId);
                if (product == null) throw new NotFoundException("Product", productId);
                
                var response = _mapper.Map<ProductDetailResponse>(product);
                await PopulateAccessoryStocksAsync(response.Accessories);
                await _cache.SetAsync(cacheKey, response, TimeSpan.FromHours(1));
                
                // Also cache by Slug for consistency
                await _cache.SetAsync($"product:slug:{response.Slug}", response, TimeSpan.FromHours(1));

                await IncrementViewCountAsync(productId);
                return response;
            }
            finally
            {
                semaphore.Release();
            }
        }

        public async Task<ProductFullDetailResponse?> GetFullByIdAsync(Guid productId)
        {
            var product = await _uow.Products.GetFullDetailsAsync(productId);
            if (product == null) return null;

            var response = _mapper.Map<ProductFullDetailResponse>(product);
            await PopulateAccessoryStocksAsync(response.Accessories);
            return response;
        }



        public async Task<ProductDetailResponse> CreateAsync(CreateProductRequest request)
        {
            // 0. Automatic Slug Generation if missing
            if (string.IsNullOrWhiteSpace(request.Slug))
            {
                request.Slug = GenerateSlug(request.Name);
            }
            else
            {
                request.Slug = request.Slug.Trim().ToLowerInvariant();
            }


            // 0a. Handle Slug Collisions (Append counter if needed)
            int counter = 1;
            string originalSlug = request.Slug;
            while (!await IsSlugUniqueAsync(request.Slug))
            {
                request.Slug = $"{originalSlug}-{counter++}";
            }

            // 1. Batch fetch Categories & Characters to resolve N+1 Query
            var categoryIds = request.CategoryIds?.Distinct().ToList() ?? new List<Guid>();
            var characterIds = request.CharacterIds?.Distinct().ToList() ?? new List<Guid>();

            var existingCategories = categoryIds.Any() 
                ? (await _uow.Categories.FindAsync(c => categoryIds.Contains(c.CategoryId))).ToDictionary(c => c.CategoryId)
                : new Dictionary<Guid, Category>();

            var existingCharacters = characterIds.Any() 
                ? (await _uow.Characters.FindAsync(c => characterIds.Contains(c.CharacterId))).ToDictionary(c => c.CharacterId)
                : new Dictionary<Guid, Character>();

            // 0b. Automatic SKU Generation if missing
            if (string.IsNullOrWhiteSpace(request.Sku))
            {
                request.Sku = request.Name.ToUpperInvariant().Replace(" ", "-");
                // Ensure the generated SKU isn't too long (Max 40 per DB)
                if (request.Sku.Length > 30) request.Sku = request.Sku.Substring(0, 30);
            }
            else
            {
                request.Sku = request.Sku.Trim().ToUpperInvariant();
            }

            // Handle SKU Collisions for the base product
            int skuCounter = 1;
            string baseSku = request.Sku;
            while (await _uow.Products.AnyAsync(p => p.Sku == request.Sku) || 
                   await _uow.Accessories.AnyAsync(a => a.Sku == request.Sku))
            {
                request.Sku = $"{baseSku}-{skuCounter++}";
            }

            var product = _mapper.Map<Product>(request);
            product.ProductId = Guid.NewGuid();
            product.Sku = request.Sku; // Guaranteed unique and upper-case
            product.CreatedAt = DateTime.UtcNow;
            product.UpdatedAt = DateTime.UtcNow;

            // 1b. Handle Variants with Robust SKU Checking
            if (request.Variants == null || !request.Variants.Any())
            {
                 throw new BadRequestException("At least one product variant (size) is required.");
            }

            // Initialization: Ensure the list is empty before manual addition
            product.Variants = new List<ProductVariant>();

            // Internal Request Check
            var duplicateTags = request.Variants.GroupBy(v => v.SizeTag.ToUpperInvariant()).Where(g => g.Count() > 1).Select(g => g.Key).ToList();
            if (duplicateTags.Any())
            {
                throw new BadRequestException($"Trùng lặp kích cỡ ({string.Join(", ", duplicateTags)}) trong yêu cầu.");
            }

            var usedSkusInRequest = new HashSet<string>();

            foreach (var vReq in request.Variants)
            {
                var variantSku = $"{request.Sku}-{vReq.SizeTag}".ToUpperInvariant();
                
                // Check within the current request list
                if (usedSkusInRequest.Contains(variantSku))
                {
                    throw new BadRequestException($"Mã SKU '{variantSku}' bị trùng lặp ngay trong danh sách biến thể gửi lên.");
                }
                usedSkusInRequest.Add(variantSku);

                // Check against Database
                await ValidateVariantSkuUniquenessAsync(variantSku);

                var variantId = Guid.NewGuid();
                product.Variants.Add(new ProductVariant 
                {
                    VariantId = variantId,
                    ProductId = product.ProductId,
                    SizeTag = vReq.SizeTag.ToUpperInvariant(),
                    SizeDescription = vReq.SizeDescription ?? "",
                    Sku = variantSku,
                    Price = vReq.Price,
                    BaseCost = vReq.BaseCost,
                    AssemblyCost = vReq.AssemblyCost,
                    WeightGram = vReq.WeightGram,
                    IsActive = true
                });

                // Auto-sync inventory for each variant
                var defaultLocId = await GetOrCreateDefaultLocationIdAsync();
                await _uow.Inventories.AddAsync(new Inventory {
                    InventoryId = Guid.NewGuid(),
                    LocationId = defaultLocId,
                    IdentityId = variantId,
                    IsAccessory = false,
                    ProductId = product.ProductId,
                    OnHand = vReq.StockQuantity,
                    UpdatedAt = DateTime.UtcNow
                });
            }

            // Sync inventory for base product (if no variants or specific case)
            if (request.StockQuantity > 0) {
                var defaultLocId = await GetOrCreateDefaultLocationIdAsync();
                await _uow.Inventories.AddAsync(new Inventory {
                    InventoryId = Guid.NewGuid(),
                    LocationId = defaultLocId,
                    ProductId = product.ProductId,
                    IdentityId = product.ProductId, // Unified: Base product is its own identity
                    IsAccessory = false,
                    OnHand = request.StockQuantity,
                    UpdatedAt = DateTime.UtcNow
                });
            }

            // Handle Categories
            foreach (var catId in categoryIds)
            {
                if (!existingCategories.ContainsKey(catId)) 
                    throw new BadRequestException($"Category with ID {catId} does not exist.");

                product.ProductCategories.Add(new ProductCategory
                {
                    ProductId = product.ProductId,
                    CategoryId = catId
                });
            }

            // Handle Characters
            foreach (var charId in characterIds)
            {
                if (!existingCharacters.ContainsKey(charId)) 
                    throw new BadRequestException($"Character with ID {charId} does not exist.");

                product.ProductCharacters.Add(new ProductCharacter
                {
                    ProductId = product.ProductId,
                    CharacterId = charId
                });
            }

            // Handle Accessories
            if (request.AccessoryIds != null && request.AccessoryIds.Any())
            {
                var accIds = request.AccessoryIds.Distinct().ToList();
                var existingAccessoriesCount = (await _uow.Accessories.FindAsync(a => accIds.Contains(a.AccessoryId))).Count();
                
                if (existingAccessoriesCount != accIds.Count)
                    throw new BadRequestException("One or more Accessory IDs are invalid.");

                foreach (var accId in accIds)
                {
                    product.ProductAccessories.Add(new ProductAccessory
                    {
                        ProductId = product.ProductId,
                        AccessoryId = accId
                    });
                }
            }



            // Handle Media
            if (request.Media != null && request.Media.Any())
            {
                foreach (var mReq in request.Media)
                {
                    product.Media.Add(new ProductMedia
                    {
                        MediaId = Guid.NewGuid(),
                        ProductId = product.ProductId,
                        Url = mReq.Url,
                        AltText = mReq.AltText,
                        SortOrder = mReq.SortOrder
                    });
                }
            }

            // Persist combo images for accessory combination matrix
            if (request.ComboImages != null && request.ComboImages.Any())
            {
                foreach (var ci in request.ComboImages)
                {
                    product.ComboImages.Add(new ProductComboImage
                    {
                        ComboId = Guid.NewGuid(),
                        BaseProductId = product.ProductId,
                        CombinationKey = ci.CombinationKey,
                        ImageUrl = ci.ImageUrl
                    });
                }
            }

            await _uow.Products.AddAsync(product);
            await _uow.SaveChangesAsync();

            await InvalidateProductRelatedCacheAsync(product.ProductId, product.Slug);

            // Fetch the fully populated product to ensure the response includes all details (categories, accessories, etc.)
            var fullyPopulatedProduct = await _uow.Products.GetWithDetailsAsync(product.ProductId);
            var response = _mapper.Map<ProductDetailResponse>(fullyPopulatedProduct ?? product);
            await PopulateAccessoryStocksAsync(response.Accessories);
            return response;
        }

        public async Task UpdateAsync(Guid productId, UpdateProductRequest request)
        {
            await _uow.ExecuteInTransactionAsync(async () =>
            {
                var product = await _uow.Products.GetWithDetailsAsync(productId);
                if (product == null) throw new NotFoundException("Product", productId);

                string oldSlug = product.Slug;

                // Server-side Log
                Console.WriteLine($"[DEBUG] --- UpdateAsync Started for Product: {productId} ---");
                Console.WriteLine($"[DEBUG] Incoming Media Count: {request.Media?.Count ?? 0}");
                Console.WriteLine($"[DEBUG] Incoming ComboImages Count: {request.ComboImages?.Count ?? 0}");

            // 1. Manual Top-Level Update (Zero Side Effects)
            if (product.Slug != request.Slug)
            {
                if (!await IsSlugUniqueAsync(request.Slug, productId))
                {
                    throw new BadRequestException($"Slug '{request.Slug}' is already in use by another product.");
                }
            }

            product.Name = request.Name;
            product.Slug = request.Slug;
            product.Description = request.Description;
            product.Model3DUrl = request.Model3DUrl;
            product.IsPersonalizable = request.IsPersonalizable;
            product.IsActive = request.IsActive;
            product.Sku = (request.Sku ?? product.Sku).Trim().ToUpperInvariant();
            product.UpdatedAt = DateTime.UtcNow;

            // 2. Sync Variants - Surgical Merge (Preserve IDs for Inventory/Cart integrity)
            var currentVariants = product.Variants.ToList();
            var incomingVariants = request.Variants ?? new List<ProductVariantInlineRequest>();

            // Validate incoming for duplicates in request
            var duplicateTagsInReq = incomingVariants.GroupBy(v => v.SizeTag.ToUpperInvariant()).Where(g => g.Count() > 1).Select(g => g.Key).ToList();
            if (duplicateTagsInReq.Any())
            {
                throw new BadRequestException($"Trùng lặp kích cỡ ({string.Join(", ", duplicateTagsInReq)}) trong yêu cầu.");
            }

            var incomingTags = incomingVariants.Select(v => v.SizeTag.ToUpperInvariant()).ToHashSet();

            // 2a. Remove variants no longer in request
            foreach (var variant in currentVariants)
            {
                if (!incomingTags.Contains(variant.SizeTag.ToUpperInvariant()))
                {
                    _uow.ProductVariants.Remove(variant);
                }
            }

            // 2b. Add or Update existing
            foreach (var vReq in incomingVariants.Where(v => !string.IsNullOrWhiteSpace(v.SizeTag)))
            {
                var normTag = vReq.SizeTag.Trim().ToUpperInvariant();
                var existing = product.Variants.FirstOrDefault(v => v.SizeTag.Trim().ToUpperInvariant() == normTag);

                var variantSku = $"{product.Sku}-{normTag}".ToUpperInvariant();
                // Pass current productId to exclude other products' SKUs
                await ValidateVariantSkuUniquenessAsync(variantSku, productId);

                var defaultLocId = await GetOrCreateDefaultLocationIdAsync();
                if (existing != null)
                {
                    // Update fields, preserving VariantId
                    existing.Price = vReq.Price;
                    existing.BaseCost = vReq.BaseCost;
                    existing.AssemblyCost = vReq.AssemblyCost;
                    existing.WeightGram = vReq.WeightGram;
                    existing.Sku = variantSku;
                    existing.SizeDescription = vReq.SizeDescription ?? "";
                    existing.UpdatedAt = DateTime.UtcNow;

                    // Sync Inventory for existing variant
                    var inv = await _uow.Inventories.GetQueryable()
                        .FirstOrDefaultAsync(i => i.IdentityId == existing.VariantId && i.IsAccessory == false && i.LocationId == defaultLocId);
                    if (inv != null) {
                        inv.OnHand = vReq.StockQuantity;
                        inv.UpdatedAt = DateTime.UtcNow;
                    } else {
                        await _uow.Inventories.AddAsync(new Inventory {
                            InventoryId = Guid.NewGuid(),
                            LocationId = defaultLocId,
                            IdentityId = existing.VariantId,
                            IsAccessory = false,
                            ProductId = productId,
                            OnHand = vReq.StockQuantity,
                            UpdatedAt = DateTime.UtcNow
                        });
                    }
                }
                else
                {
                    // Add new
                    var variantId = Guid.NewGuid();
                    product.Variants.Add(new ProductVariant
                    {
                        VariantId = variantId,
                        ProductId = productId,
                        SizeTag = normTag,
                        SizeDescription = vReq.SizeDescription ?? "",
                        Sku = variantSku,
                        Price = vReq.Price,
                        BaseCost = vReq.BaseCost,
                        AssemblyCost = vReq.AssemblyCost,
                        WeightGram = vReq.WeightGram,
                        IsActive = true
                    });

                    // Add Inventory for new variant
                    await _uow.Inventories.AddAsync(new Inventory {
                        InventoryId = Guid.NewGuid(),
                        LocationId = defaultLocId,
                        IdentityId = variantId,
                        IsAccessory = false,
                        ProductId = productId,
                        OnHand = vReq.StockQuantity,
                        UpdatedAt = DateTime.UtcNow
                    });
                }
            }

            // Sync inventory for base product
            if (request.StockQuantity > 0 || (product.Variants.Count == 0)) {
                var defaultLocId = await GetOrCreateDefaultLocationIdAsync();
                var baseInv = await _uow.Inventories.GetQueryable()
                    .FirstOrDefaultAsync(i => i.ProductId == productId && i.IdentityId == productId && i.IsAccessory == false && i.LocationId == defaultLocId);
                if (baseInv != null) {
                    baseInv.OnHand = request.StockQuantity;
                    baseInv.UpdatedAt = DateTime.UtcNow;
                } else {
                    await _uow.Inventories.AddAsync(new Inventory {
                        InventoryId = Guid.NewGuid(),
                        LocationId = defaultLocId,
                        ProductId = productId,
                        IdentityId = productId,
                        IsAccessory = false,
                        OnHand = request.StockQuantity,
                        UpdatedAt = DateTime.UtcNow
                    });
                }
            }

            // 2. Batch validate Category/Character existence to avoid N+1 and connection leaks
            var incomingCatIds = request.CategoryIds?.Distinct().ToList() ?? new List<Guid>();
            var incomingCharIds = request.CharacterIds?.Distinct().ToList() ?? new List<Guid>();

            if (incomingCatIds.Any())
            {
                var existingCatsCount = (await _uow.Categories.FindAsync(c => incomingCatIds.Contains(c.CategoryId))).Count();
                if (existingCatsCount != incomingCatIds.Count)
                    throw new BadRequestException("One or more Category IDs are invalid.");
            }

            if (incomingCharIds.Any())
            {
                var existingCharsCount = (await _uow.Characters.FindAsync(c => incomingCharIds.Contains(c.CharacterId))).Count();
                if (existingCharsCount != incomingCharIds.Count)
                    throw new BadRequestException("One or more Character IDs are invalid.");
            }

            // 3. Sync Junction Tables (Categories)
            var currentPCats = product.ProductCategories.ToList();
            var incomingCatSet = incomingCatIds.ToHashSet();
            foreach (var pc in currentPCats)
            {
                if (!incomingCatSet.Contains(pc.CategoryId))
                {
                    // For junction tables, we might need a composite delete or remove from collection
                    // If the UOW has a specific way for junction tables, use it.
                    // Assuming we can remove from collection safely for junction tables if IDs are not tracked sensitive
                    product.ProductCategories.Remove(pc); 
                }
            }
            var existingCatIds = product.ProductCategories.Select(pc => pc.CategoryId).ToHashSet();
            foreach (var catId in incomingCatSet)
            {
                if (!existingCatIds.Contains(catId))
                    product.ProductCategories.Add(new ProductCategory { ProductId = productId, CategoryId = catId });
            }

            // 4. Sync Junction Tables (Characters)
            var currentPChars = product.ProductCharacters.ToList();
            var incomingCharSet = incomingCharIds.ToHashSet();
            foreach (var pc in currentPChars)
            {
                if (!incomingCharSet.Contains(pc.CharacterId))
                {
                    product.ProductCharacters.Remove(pc);
                }
            }
            var existingCharIds = product.ProductCharacters.Select(pc => pc.CharacterId).ToHashSet();
            foreach (var charId in incomingCharSet)
            {
                if (!existingCharIds.Contains(charId))
                    product.ProductCharacters.Add(new ProductCharacter { ProductId = productId, CharacterId = charId });
            }

            // 4b. Sync Junction Tables (Accessories) - High-Stability Merge
            var currentPAccs = product.ProductAccessories.ToList();
            foreach (var pa in currentPAccs)
            {
                _uow.ProductAccessories.Remove(pa);
            }
            product.ProductAccessories.Clear();

            var incomingAccIds = request.AccessoryIds?.Distinct().ToList() ?? new List<Guid>();
            if (incomingAccIds.Any())
            {
                var existingAccsCount = (await _uow.Accessories.FindAsync(a => incomingAccIds.Contains(a.AccessoryId))).Count();
                if (existingAccsCount != incomingAccIds.Count)
                    throw new BadRequestException("One or more Accessory IDs are invalid.");

                foreach (var accId in incomingAccIds)
                {
                    // Adding via repository for maximum stability in junction tables
                    await _uow.ProductAccessories.AddAsync(new ProductAccessory 
                    { 
                        ProductId = productId, 
                        AccessoryId = accId 
                    });
                }
            }



            // 5. Sync Media - High-Stability Merge
            var existingMedia = product.Media.ToList();
            foreach (var m in existingMedia)
            {
                _uow.ProductMedias.Remove(m);
            }
            product.Media.Clear();

            if (request.Media != null)
            {
                foreach (var mReq in request.Media.Where(m => !string.IsNullOrWhiteSpace(m.Url)))
                {
                    // Add directly to repository to bypass collection tracking issues
                    await _uow.ProductMedias.AddAsync(new ProductMedia
                    {
                        ProductId = productId,
                        Url = mReq.Url.Trim(),
                        AltText = mReq.AltText,
                        SortOrder = mReq.SortOrder
                    });
                }
            }

            // 6. Sync ComboImages - High-Stability Merge
            var existingCombos = product.ComboImages.ToList();
            foreach (var ci in existingCombos)
            {
                _uow.ProductComboImages.Remove(ci);
            }
            product.ComboImages.Clear();

            if (request.ComboImages != null)
            {
                foreach (var ci in request.ComboImages.Where(c => !string.IsNullOrWhiteSpace(c.ImageUrl)))
                {
                    // Add directly to repository with a fresh Guid
                    await _uow.ProductComboImages.AddAsync(new ProductComboImage
                    {
                        ComboId = Guid.NewGuid(),
                        BaseProductId = productId,
                        CombinationKey = ci.CombinationKey.Trim(),
                        ImageUrl = ci.ImageUrl.Trim()
                    });
                }
            }

            // Save all changes in a single transaction
            await _uow.SaveChangesAsync();
            Console.WriteLine($"[DEBUG] UpdateAsync Finished with Direct Repo Sync for Product: {productId}");
            Console.WriteLine($"[DEBUG] UpdateAsync Finished and Committed for Product: {productId}");

            // Invalidate cache
            await InvalidateProductRelatedCacheAsync(productId, oldSlug);
            if (oldSlug != product.Slug)
            {
                await InvalidateProductRelatedCacheAsync(slug: product.Slug);
            }
        });
    }

        public async Task IncrementViewCountAsync(Guid productId)
        {
            await _cache.IncrementViewCountAsync(productId);
        }

        public async Task UpdateTotalSalesAsync(Guid productId, int totalSales)
        {
            var product = await _uow.Products.GetByIdAsync(productId);
            if (product == null) throw new NotFoundException("Product", productId);

            product.TotalSales = totalSales;
            product.UpdatedAt = DateTime.UtcNow;

            await _uow.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid productId)
        {
            var product = await _uow.Products.GetByIdAsync(productId);
            if (product == null) throw new NotFoundException("Product", productId);
            
            string slug = product.Slug;
            _uow.Products.Remove(product);
            await _uow.SaveChangesAsync();

            // Invalidate cache
            await InvalidateProductRelatedCacheAsync(productId, slug);
        }

        private async Task<Guid> GetOrCreateDefaultLocationIdAsync()
        {
            var warehouse = await _uow.Locations.GetQueryable().FirstOrDefaultAsync(l => l.LocationType.ToUpper() == "WAREHOUSE");
            if (warehouse != null) return warehouse.LocationId;

            var first = await _uow.Locations.GetQueryable().FirstOrDefaultAsync(l => true);
            if (first != null) return first.LocationId;

            // Create default if none exists
            var defaultLoc = new Location
            {
                LocationId = Guid.NewGuid(),
                Name = "Kho Tổng (Mặc định)",
                LocationType = "WAREHOUSE",
                AddressText = "Thiết lập sau"
            };
            await _uow.Locations.AddAsync(defaultLoc);
            // We don't save yet, it will be saved with the product transaction
            return defaultLoc.LocationId;
        }

        private async Task InvalidateProductRelatedCacheAsync(Guid? productId = null, string? slug = null)
        {
            if (productId.HasValue)
            {
                await _cache.RemoveAsync($"product:id:{productId.Value}");
            }
            if (!string.IsNullOrEmpty(slug))
            {
                await _cache.RemoveAsync($"product:slug:{slug}");
            }

            // Clear all list-related caches
            await _cache.RemoveByPrefixAsync("products:page:");
            await _cache.RemoveByPrefixAsync("products:top:");
        }
        public async Task<bool> IsSlugUniqueAsync(string slug, Guid? excludeProductId = null)
        {
            if (string.IsNullOrWhiteSpace(slug)) return true;

            // Use AnyAsync for performance and avoid ToLower() inside expression to keep Index usage
            // Standardization: Ensure slug is compared exactly. 
            // If DB is case-sensitive, we should normalize the slug to lowercase before saving and querying.
            var normalizedSlug = slug.Trim().ToLowerInvariant();
            
            if (excludeProductId.HasValue)
            {
                return !await _uow.Products.AnyAsync(p => p.Slug == normalizedSlug && p.ProductId != excludeProductId.Value);
            }

            return !await _uow.Products.AnyAsync(p => p.Slug == normalizedSlug);
        }

        private static string GenerateSlug(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return Guid.NewGuid().ToString()[..8];

            var normalizedString = text.ToLowerInvariant().Trim();

            // Replace accented Vietnamese characters
            string[] vietnameseSigns = new string[]
            {
                "aAeEoOuUiIdDyY",
                "áàạảãâấầậẩẫăắằặẳẵ",
                "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
                "éèẹẻẽêếềệểễ",
                "ÉÈẸẺẼÊẾỀỆỂỄ",
                "óòọỏõôốồộổỗơớờợởỡ",
                "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
                "úùụủũưứừựửữ",
                "ÚÙỤỦŨƯỨỪỰỬỮ",
                "íìịỉĩ",
                "ÍÌỊỈĨ",
                "đ",
                "Đ",
                "ýỳỵỷỹ",
                "ÝỲỴỶỸ"
            };

            for (int i = 1; i < vietnameseSigns.Length; i++)
            {
                for (int j = 0; j < vietnameseSigns[i].Length; j++)
                {
                    normalizedString = normalizedString.Replace(vietnameseSigns[i][j], vietnameseSigns[0][i - 1]);
                }
            }

            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                if (char.IsLetterOrDigit(c))
                {
                    stringBuilder.Append(c);
                }
                else if (c == ' ' || c == '-' || c == '_')
                {
                    stringBuilder.Append('-');
                }
            }

            return stringBuilder.ToString()
                .Replace("--", "-")
                .Trim('-');
        }

        private async Task PopulateAccessoryStocksAsync(IEnumerable<AccessoryResponse> accessories)
        {
            var accessoryList = accessories.ToList();
            if (!accessoryList.Any()) return;

            var accessoryIds = accessoryList.Select(a => a.AccessoryId).Distinct().ToList();
            var stockSummary = await _uow.Inventories.GetStockSummaryBatchAsync(accessoryIds);

            foreach (var accessory in accessoryList)
            {
                if (stockSummary.TryGetValue(accessory.AccessoryId, out var stock))
                {
                    accessory.OnHand = stock.OnHand;
                    accessory.Reserved = stock.Reserved;
                }
            }
        }

        private async Task ValidateVariantSkuUniquenessAsync(string sku, Guid? excludeProductId = null)
        {
            if (string.IsNullOrWhiteSpace(sku)) return;
            var trimmedSku = sku.Trim();

            // 1. Check against Product base SKUs (using ILike for case-insensitivity in Postgres)
            var productExists = await _uow.Products.AnyAsync(p => EF.Functions.ILike(p.Sku.Trim(), trimmedSku));
            if (productExists)
                throw new BadRequestException($"Mã SKU '{trimmedSku}' đã trùng với mã định danh của một Sản phẩm chính.");

            // 2. Check against Accessory SKUs
            var accessoryExists = await _uow.Accessories.AnyAsync(a => EF.Functions.ILike(a.Sku.Trim(), trimmedSku));
            if (accessoryExists)
                throw new BadRequestException($"Mã SKU '{trimmedSku}' đã trùng với mã của một Phụ kiện.");

            // 3. Check against ProductVariant SKUs
            var variantExists = excludeProductId.HasValue
                ? await _uow.ProductVariants.AnyAsync(v => EF.Functions.ILike(v.Sku.Trim(), trimmedSku) && v.ProductId != excludeProductId.Value)
                : await _uow.ProductVariants.AnyAsync(v => EF.Functions.ILike(v.Sku.Trim(), trimmedSku));

            if (variantExists)
                throw new BadRequestException($"Mã SKU '{trimmedSku}' đã tồn tại ở một biến thể sản phẩm khác.");
        }
    }
}
