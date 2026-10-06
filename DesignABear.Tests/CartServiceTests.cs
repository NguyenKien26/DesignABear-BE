using AutoMapper;
using DesignABear.Repositories.Models;
using DesignABear.Repositories.Repositories.Interfaces;
using DesignABear.Repositories.UnitOfWork;
using DesignABear.Services.DTOs.Requests;
using DesignABear.Services.DTOs.Responses;
using DesignABear.Services.Services.Implementations;
using Moq;
using System.Linq.Expressions;
using Xunit;

namespace DesignABear.Tests
{
    public class CartServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUow;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Microsoft.Extensions.Caching.Memory.IMemoryCache _cache;
        private readonly CartService _cartService;

        public CartServiceTests()
        {
            _mockUow = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _cache = new Microsoft.Extensions.Caching.Memory.MemoryCache(new Microsoft.Extensions.Caching.Memory.MemoryCacheOptions());
            _cartService = new CartService(_mockUow.Object, _mockMapper.Object, _cache);
        }

        [Fact]
        public async Task AddItemAsync_ExistingItem_IncrementsQuantity()
        {
            // Arrange
            var cartId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var buildId = Guid.NewGuid();
            var request = new CreateCartItemRequest
            {
                CartId = cartId,
                ProductId = productId,
                BuildId = buildId,
                Quantity = 2
            };

            var existingItem = new CartItem
            {
                CartItemId = Guid.NewGuid(),
                CartId = cartId,
                ProductId = productId,
                BuildId = buildId,
                Quantity = 3
            };

            var itemsList = new List<CartItem> { existingItem };

            _mockUow.Setup(u => u.CartItems.FindAsync(It.IsAny<Expression<Func<CartItem, bool>>>()))
                .ReturnsAsync(itemsList);
            
            _mockUow.Setup(u => u.Carts.GetByIdAsync(cartId))
                .ReturnsAsync(new Cart { CartId = cartId });

            _mockMapper.Setup(m => m.Map<CartItemResponse>(existingItem))
                .Returns(new CartItemResponse { CartItemId = existingItem.CartItemId, Quantity = 5 });

            // Act
            var result = await _cartService.AddItemAsync(request);

            // Assert
            Assert.Equal(5, existingItem.Quantity);
            _mockUow.Verify(u => u.CartItems.Update(existingItem), Times.Once);
            _mockUow.Verify(u => u.SaveChangesAsync(), Times.Once);
            Assert.Equal(5, result.Quantity);
        }

        [Fact]
        public async Task AddItemAsync_NewItem_CreatesNewRecord()
        {
            // Arrange
            var cartId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var request = new CreateCartItemRequest
            {
                CartId = cartId,
                ProductId = productId,
                Quantity = 1
            };

            _mockUow.Setup(u => u.CartItems.FindAsync(It.IsAny<Expression<Func<CartItem, bool>>>()))
                .ReturnsAsync(new List<CartItem>());

            _mockUow.Setup(u => u.Carts.GetByIdAsync(cartId))
                .ReturnsAsync(new Cart { CartId = cartId });

            var newItem = new CartItem { CartId = cartId, ProductId = productId, Quantity = 1 };
            _mockMapper.Setup(m => m.Map<CartItem>(request)).Returns(newItem);
            _mockMapper.Setup(m => m.Map<CartItemResponse>(newItem)).Returns(new CartItemResponse { Quantity = 1 });

            // Act
            var result = await _cartService.AddItemAsync(request);

            // Assert
            _mockUow.Verify(u => u.CartItems.AddAsync(It.IsAny<CartItem>()), Times.Once);
            _mockUow.Verify(u => u.SaveChangesAsync(), Times.Once);
            Assert.Equal(1, result.Quantity);
        }
    }
}
