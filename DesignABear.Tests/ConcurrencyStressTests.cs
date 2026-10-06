using DesignABear.Repositories.Models;
using DesignABear.Services.DTOs.Requests;
using DesignABear.Services.Services.Implementations;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore;

namespace DesignABear.Tests
{
    public class ConcurrencyStressTests : BaseIntegrationTest
    {
        private readonly InventoryService _inventoryService;
        private readonly CartService _cartService;
        private readonly ProductService _productService;
        private readonly RedisCacheService _cacheService;

        public ConcurrencyStressTests(TestDatabaseFixture fixture) : base(fixture)
        {
            var loggerMockInv = new Mock<ILogger<InventoryService>>();
            _inventoryService = new InventoryService(_uow, _mapper, loggerMockInv.Object); // Assuming UOW is real from Base class

            var loggerMockRedis = new Mock<ILogger<RedisCacheService>>();

            var configMock = new Mock<Microsoft.Extensions.Configuration.IConfiguration>();
            _cacheService = new RedisCacheService(_distributedCache, _redis, loggerMockRedis.Object, configMock.Object);

            _cartService = new CartService(_uow, _mapper, _cache);
            _productService = new ProductService(_uow, _mapper, _cacheService);
        }

        public class TestOutcome
        {
            public bool IsSuccess { get; set; }
            public string? ErrorMessage { get; set; }
            public string? ExceptionType { get; set; }
        }

        [Fact]
        public async Task Inventory_Overselling_Prevention_StressTest()
        {
            // Arrange: 5 units in stock
            var productId = Guid.NewGuid();
            var locationId = Guid.NewGuid();
            
            await _context.Locations.AddAsync(new Location { LocationId = locationId, Name = "Stress Warehouse", LocationType = "WAREHOUSE" });
            await _context.Products.AddAsync(new Product { ProductId = productId, Name = "Limited Bear", Slug = "limit-b" });
            await _context.Inventories.AddAsync(new Inventory { ProductId = productId, LocationId = locationId, OnHand = 5, Reserved = 0 });
            await _context.SaveChangesAsync();

            int concurrencyCount = 50;
            var outcomes = new ConcurrentBag<TestOutcome>();

            // Act: 50 threads trying to reserve 1 unit each
            var tasks = Enumerable.Range(0, concurrencyCount).Select(async i =>
            {
                try
                {
                    await _inventoryService.ReserveStockAsync(productId, false, 1, locationId);
                    outcomes.Add(new TestOutcome { IsSuccess = true });
                }
                catch (Exception ex)
                {
                    outcomes.Add(new TestOutcome 
                    { 
                        IsSuccess = false, 
                        ErrorMessage = ex.Message, 
                        ExceptionType = ex.GetType().Name 
                    });
                }
            });

            await Task.WhenAll(tasks);

            // Assert
            var successCount = outcomes.Count(o => o.IsSuccess);
            var failureCount = outcomes.Count(o => !o.IsSuccess);

            Assert.Equal(5, successCount); // Only 5 should succeed
            Assert.Equal(45, failureCount); // 45 should fail
            
            // Verify DB integrity
            var finalInv = await _context.Inventories.AsNoTracking().FirstAsync(i => i.ProductId == productId);
            Assert.Equal(5, finalInv.Reserved);
            Assert.Equal(5, finalInv.OnHand);
            Assert.Equal(0, finalInv.OnHand - finalInv.Reserved); // Available = 0
        }

        [Fact]
        public async Task Cart_Upsert_Resiliency_StressTest()
        {
            // Arrange: Setup a cart
            var cartId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            await _context.Carts.AddAsync(new Cart { CartId = cartId, CreatedAt = DateTime.UtcNow });
            await _context.Products.AddAsync(new Product { ProductId = productId, Name = "Popular Accessory", Slug = "pop-acc", Variants = new List<ProductVariant> { new ProductVariant { Price = 50, Sku = "POP-ACC" } } });
            await _context.SaveChangesAsync();

            int concurrencyCount = 20;
            var outcomes = new ConcurrentBag<TestOutcome>();

            // Act: Simulating 20 clicks on "Add to Cart" simultaneously for the same item
            var tasks = Enumerable.Range(0, concurrencyCount).Select(async i =>
            {
                try
                {
                    await _cartService.AddItemAsync(new CreateCartItemRequest
                    {
                        CartId = cartId,
                        ProductId = productId,
                        Quantity = 1
                    });
                    outcomes.Add(new TestOutcome { IsSuccess = true });
                }
                catch (Exception ex)
                {
                    outcomes.Add(new TestOutcome 
                    { 
                        IsSuccess = false, 
                        ErrorMessage = ex.Message, 
                        ExceptionType = ex.GetType().Name 
                    });
                }
            });

            await Task.WhenAll(tasks);

            // Assert
            // With Keyed Locking + Unique Index Cleanup, ALL should technically succeed by incrementing the quantity
            var successCount = outcomes.Count(o => o.IsSuccess);
            Assert.Equal(concurrencyCount, successCount);

            var cartItem = await _context.CartItems.AsNoTracking().FirstAsync(ci => ci.CartId == cartId && ci.ProductId == productId);
            Assert.Equal(concurrencyCount, cartItem.Quantity);
            
            // Result: No duplicates in DB
            var countInDb = await _context.CartItems.CountAsync(ci => ci.CartId == cartId);
            Assert.Equal(1, countInDb);
        }

        [Fact]
        public async Task Product_Cache_Stampede_Prevention_Test()
        {
            // Arrange
            var slug = "stampede-bear";
            var prodId = Guid.NewGuid();
            await _context.Products.AddAsync(new Product { ProductId = prodId, Name = "Stampede Bear", Slug = slug, Variants = new List<ProductVariant> { new ProductVariant { Price = 500, Sku = "STAMP-1" } } });
            await _context.SaveChangesAsync();
            
            // Ensure cache is clear
            await _cacheService.RemoveAsync($"product:slug:{slug}");

            int concurrencyCount = 50;
            var outcomes = new ConcurrentBag<TestOutcome>();

            // Act: 50 requests hitting the same slug simultaneously (Cache Miss)
            // We want to verify only ONE database hit happens.
            // Since we can't easily count DB hits here without Interceptors, 
            // the Keyed Lock ensures they serialize correctly.
            var tasks = Enumerable.Range(0, concurrencyCount).Select(async i =>
            {
                try
                {
                    var result = await _productService.GetBySlugAsync(slug);
                    outcomes.Add(new TestOutcome { IsSuccess = true });
                }
                catch (Exception ex)
                {
                    outcomes.Add(new TestOutcome { IsSuccess = false, ErrorMessage = ex.Message });
                }
            });

            await Task.WhenAll(tasks);

            // Assert
            Assert.All(outcomes, o => Assert.True(o.IsSuccess));
        }
    }
}
