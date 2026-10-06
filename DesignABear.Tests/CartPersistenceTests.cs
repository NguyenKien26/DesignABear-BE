using AutoMapper;
using DesignABear.Repositories.Models;
using DesignABear.Repositories.UnitOfWork;
using DesignABear.Services.DTOs.Requests;
using DesignABear.Services.DTOs.Responses;
using DesignABear.Services.Services.Implementations;
using DesignABear.Services.Mappings;
using Moq;
using Xunit;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DesignABear.Tests
{
    public class CartPersistenceTests
    {
        private readonly Mock<IUnitOfWork> _mockUow;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private readonly CartService _cartService;

        public CartPersistenceTests()
        {
            _mockUow = new Mock<IUnitOfWork>();
            _cache = new MemoryCache(new MemoryCacheOptions());
            
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = config.CreateMapper();

            _cartService = new CartService(_mockUow.Object, _mapper, _cache);
        }

        [Fact]
        public async Task AddItemAsync_WithBuildAndSize_PersistsSizeToBuild()
        {
            // Arrange
            var cartId = Guid.NewGuid();
            var buildId = Guid.NewGuid();
            var request = new CreateCartItemRequest
            {
                CartId = cartId,
                BuildId = buildId,
                SizeTag = "M",
                SizeDescription = "Medium Size",
                Quantity = 1
            };

            var build = new Build { BuildId = buildId, BaseProductId = Guid.NewGuid() };
            var cart = new Cart { CartId = cartId, CartItems = new List<CartItem>() };

            _mockUow.Setup(u => u.Carts.GetActiveCartByUserAsync(It.IsAny<Guid>())).ReturnsAsync(cart);
            _mockUow.Setup(u => u.Builds.GetByIdAsync(buildId)).ReturnsAsync(build);
            _mockUow.Setup(u => u.CartItems.FindAsync(It.IsAny<Expression<Func<CartItem, bool>>>()))
                .ReturnsAsync(new List<CartItem>());

            // Act
            await _cartService.AddItemToActiveCartAsync(Guid.NewGuid(), request);

            // Assert
            Assert.Equal("M", build.SizeTag);
            Assert.Equal("Medium Size", build.SizeDescription);
            _mockUow.Verify(u => u.Builds.Update(build), Times.Once);
        }

        [Fact]
        public void Mapping_CartItemWithBuildSize_PrioritizesBuildSize()
        {
            // Arrange
            var build = new Build 
            { 
                SizeTag = "L", 
                SizeDescription = "Large Build",
                BaseVariant = new ProductVariant { SizeTag = "S", SizeDescription = "Small Variant" }
            };
            var cartItem = new CartItem { Build = build, BuildId = Guid.NewGuid() };

            // Act
            var response = _mapper.Map<CartItemResponse>(cartItem);

            // Assert
            Assert.Equal("L", response.SizeTag);
            Assert.Equal("Large Build", response.Size);
        }
    }
}
