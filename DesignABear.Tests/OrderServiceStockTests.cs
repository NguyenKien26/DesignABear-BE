using Xunit;
using Moq;
using DesignABear.Services.Services.Implementations;
using DesignABear.Repositories.UnitOfWork;
using DesignABear.Repositories.Models;
using DesignABear.Services.Services.Interfaces;
using DesignABear.Services.DTOs.Requests;
using DesignABear.Services.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using DesignABear.Services.Configurations;
using AutoMapper;
using DesignABear.Repositories.Repositories.Interfaces;

namespace DesignABear.Tests
{
    public class OrderServiceStockTests
    {
        private readonly Mock<IUnitOfWork> _mockUow;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IServiceProvider> _mockServiceProvider;
        private readonly Mock<IGhtkShippingService> _mockGhtk;
        private readonly Mock<IInventoryService> _mockInventory;
        private readonly Mock<IOptions<BusinessSettings>> _mockOptions;
        private readonly BusinessSettings _settings;
        private readonly OrderService _service;

        public OrderServiceStockTests()
        {
            _mockUow = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _mockServiceProvider = new Mock<IServiceProvider>();
            _mockGhtk = new Mock<IGhtkShippingService>();
            _mockInventory = new Mock<IInventoryService>();
            _mockOptions = new Mock<IOptions<BusinessSettings>>();

            _settings = new BusinessSettings
            {
                DefaultWarehouseId = "55555555-5555-5555-5555-555555555555",
                AiProcessorProductId = "33333333-3333-3333-3333-333333333334"
            };
            _mockOptions.Setup(o => o.Value).Returns(_settings);

            _mockUow.Setup(u => u.Orders).Returns(new Mock<IOrderRepository>().Object);
            _mockUow.Setup(u => u.CartItems).Returns(new Mock<ICartItemRepository>().Object);
            _mockUow.Setup(u => u.ExecuteInTransactionAsync(It.IsAny<Func<Task>>()))
                .Returns<Func<Task>>(async (action) => await action());
            _mockServiceProvider.Setup(x => x.GetService(typeof(IInventoryService))).Returns(_mockInventory.Object);

            _service = new OrderService(
                _mockUow.Object, 
                _mockMapper.Object, 
                _mockInventory.Object,
                new Mock<IPromotionService>().Object,
                new Mock<ICartService>().Object,
                _mockGhtk.Object,
                new Mock<Microsoft.Extensions.Logging.ILogger<OrderService>>().Object,
                _mockOptions.Object
            );
        }

        [Fact]
        public async Task CreateFromCartAsync_ShouldThrowBadRequest_WhenStockIsInsufficient()
        {
            // Arrange
            var cartId = Guid.NewGuid();
            var variantId = Guid.NewGuid();
            var cart = new Cart
            {
                CartId = cartId,
                CartItems = new List<CartItem>
                {
                    new CartItem { VariantId = variantId, Quantity = 10, Variant = new ProductVariant { Product = new Product { Name = "Test Bear" } } }
                }
            };

            _mockUow.Setup(u => u.Carts.GetWithItemsAsync(cartId)).ReturnsAsync(cart);
            _mockInventory.Setup(i => i.ReserveStockBatchAsync(It.IsAny<Guid>(), It.IsAny<IEnumerable<StockReservationItem>>()))
                .ThrowsAsync(new BadRequestException("Insufficient stock")); 

            var request = new CreateOrderRequest { UserId = Guid.NewGuid() };
            _mockMapper.Setup(m => m.Map<Order>(It.IsAny<CreateOrderRequest>())).Returns(new Order());

            // Act & Assert
            var ex = await Assert.ThrowsAsync<BadRequestException>(() => _service.CreateFromCartAsync(cartId, request));
            Assert.Contains("Insufficient stock", ex.Message);
            
            // Verify no transaction committed (SaveChangesAsync not called)
            _mockUow.Verify(u => u.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task CreateFromCartAsync_ShouldReserveStock_WhenStockIsAvailable()
        {
            // Arrange
            var cartId = Guid.NewGuid();
            var variantId = Guid.NewGuid();
            var cart = new Cart
            {
                CartId = cartId,
                CartItems = new List<CartItem>
                {
                    new CartItem { VariantId = variantId, Quantity = 2, Variant = new ProductVariant { Price = 100, Product = new Product { Name = "Test Bear" } } }
                }
            };

            _mockUow.Setup(u => u.Carts.GetWithItemsAsync(cartId)).ReturnsAsync(cart);
            _mockInventory.Setup(i => i.GetTotalAvailableAsync(variantId, false)).ReturnsAsync(10);
            
            _mockUow.Setup(u => u.ExecuteInTransactionAsync(It.IsAny<Func<Task>>()))
                .Returns<Func<Task>>(async (action) => await action());
                
            _mockMapper.Setup(m => m.Map<Order>(It.IsAny<CreateOrderRequest>())).Returns(new Order { OrderItems = new List<OrderItem>() });

            var request = new CreateOrderRequest { UserId = Guid.NewGuid() };

            // Act
            await _service.CreateFromCartAsync(cartId, request);

            // Assert
            _mockInventory.Verify(i => i.ReserveStockBatchAsync(It.IsAny<Guid>(), It.Is<IEnumerable<StockReservationItem>>(items => items.Any(x => x.IdentityId == variantId && x.Quantity == 2))), Times.Once);
            _mockUow.Verify(u => u.SaveChangesAsync(), Times.AtLeastOnce);
        }

        [Fact]
        public async Task CreateFromCartAsync_ShouldReserveAccessoryStock_WhenBuildItemIsPresent()
        {
            // Arrange
            var cartId = Guid.NewGuid();
            var baseProductId = Guid.NewGuid();
            var accessoryId1 = Guid.NewGuid();
            var buildId = Guid.NewGuid();

            var cart = new Cart
            {
                CartId = cartId,
                CartItems = new List<CartItem>
                {
                    new CartItem 
                    { 
                        VariantId = baseProductId, // Reuse IDs for variant in tests
                        BuildId = buildId, 
                        Quantity = 1,
                        Variant = new ProductVariant { Product = new Product { Name = "Base Bear" } },
                        Build = new Build
                        {
                            BuildId = buildId,
                            BaseProductId = baseProductId,
                            BuildComponents = new List<BuildComponent>
                            {
                                new BuildComponent { OptionProductId = accessoryId1, Quantity = 2 }
                            }
                        }
                    }
                }
            };

            _mockUow.Setup(u => u.Carts.GetWithItemsAsync(cartId)).ReturnsAsync(cart);
            _mockInventory.Setup(i => i.GetTotalAvailableAsync(baseProductId, false)).ReturnsAsync(10);
            _mockInventory.Setup(i => i.GetTotalAvailableAsync(accessoryId1, true)).ReturnsAsync(10);
            
            _mockUow.Setup(u => u.ExecuteInTransactionAsync(It.IsAny<Func<Task>>()))
                .Returns<Func<Task>>(async (action) => await action());
                
            _mockMapper.Setup(m => m.Map<Order>(It.IsAny<CreateOrderRequest>())).Returns(new Order { OrderItems = new List<OrderItem>() });

            var request = new CreateOrderRequest { UserId = Guid.NewGuid() };

            // Act
            await _service.CreateFromCartAsync(cartId, request);

            // Assert
            _mockInventory.Verify(i => i.ReserveStockBatchAsync(It.IsAny<Guid>(), It.Is<IEnumerable<StockReservationItem>>(items => items.Any(x => x.IdentityId == baseProductId && x.Quantity == 1) && items.Any(x => x.IdentityId == accessoryId1 && x.Quantity == 2))), Times.Once);
        }

        [Fact]
        public async Task UpdateStatusAsync_ShouldReleaseStock_WhenCancelled()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var variantId = Guid.NewGuid();
            var order = new Order
            {
                OrderId = orderId,
                Status = "PENDING",
                OrderItems = new List<OrderItem>
                {
                    new OrderItem { VariantId = variantId, Quantity = 3 }
                }
            };

            _mockUow.Setup(u => u.Orders.GetWithDetailsAsync(orderId)).ReturnsAsync(order);
            _mockUow.Setup(u => u.Orders.Update(It.IsAny<Order>())).Verifiable();
            
            var request = new UpdateOrderRequest { Status = "CANCELLED" };

            // Act
            await _service.UpdateStatusAsync(orderId, request);

            // Assert
            _mockInventory.Verify(i => i.ReleaseReservationAsync(variantId, false, 3, It.IsAny<Guid>()), Times.Once);
            Assert.Equal("CANCELLED", order.Status);
        }
    }

    // Stub for transaction since we are mocking at UoW level
    public interface IDbTransactionWrapper : IAsyncDisposable
    {
        Task CommitAsync();
        Task RollbackAsync();
    }
}
