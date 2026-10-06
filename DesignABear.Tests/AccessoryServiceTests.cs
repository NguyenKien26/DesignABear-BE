using AutoMapper;
using DesignABear.Repositories.Models;
using DesignABear.Repositories.UnitOfWork;
using DesignABear.Services.DTOs.Requests;
using DesignABear.Services.Services.Implementations;
using DesignABear.Services.Mappings;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace DesignABear.Tests
{
    public class AccessoryServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUow;
        private readonly IMapper _mapper;
        private readonly AccessoryService _service;

        public AccessoryServiceTests()
        {
            _mockUow = new Mock<IUnitOfWork>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = config.CreateMapper();
            _service = new AccessoryService(_mockUow.Object, _mapper);
        }

        [Fact]
        public async Task GetByProductIdAsync_ReturnsMappedAccessories()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var accId = Guid.NewGuid();
            var acc = new Accessory { AccessoryId = accId, Name = "Bow", Sku = "BOW-001" };
            
            var relations = new List<ProductAccessory>
            {
                new ProductAccessory { ProductId = productId, AccessoryId = accId }
            };

            _mockUow.Setup(u => u.ProductAccessories.FindAsync(It.IsAny<Expression<Func<ProductAccessory, bool>>>()))
                .ReturnsAsync(relations);
            
            _mockUow.Setup(u => u.Accessories.GetByIdAsync(accId))
                .ReturnsAsync(acc);

            // Act
            var result = await _service.GetByProductIdAsync(productId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Single(result.Value);
            Assert.Equal("Bow", result.Value.First().Name);
        }

        [Fact]
        public async Task GetSmartChipAsync_ReturnsCorrectPrice()
        {
            // Arrange
            var chips = new List<Accessory>
            {
                new Accessory { Sku = "SMART-CHIP-001", TargetPrice = 999000, IsActive = true }
            };
            
            _mockUow.Setup(u => u.Accessories.FindAsync(It.IsAny<Expression<Func<Accessory, bool>>>()))
                .ReturnsAsync(chips);

            // Act
            var result = await _service.GetSmartChipAsync();

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(999000, result.Value.TargetPrice);
        }

        [Fact]
        public async Task AddToProductAsync_CreatesNewRelation()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var accId = Guid.NewGuid();
            
            _mockUow.Setup(u => u.ProductAccessories.AnyAsync(It.IsAny<Expression<Func<ProductAccessory, bool>>>()))
                .ReturnsAsync(false);

            // Act
            var result = await _service.AddToProductAsync(productId, accId);

            // Assert
            Assert.True(result.IsSuccess);
            _mockUow.Verify(u => u.ProductAccessories.AddAsync(It.Is<ProductAccessory>(pa => pa.ProductId == productId && pa.AccessoryId == accId)), Times.Once);
            _mockUow.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ValidRequest_ReturnsSuccess()
        {
            // Arrange
            var request = new CreateAccessoryRequest { Name = "Hat", Sku = "HAT-001" };
            
            _mockUow.Setup(u => u.Products.AnyAsync(It.IsAny<Expression<Func<Product, bool>>>())).ReturnsAsync(false);
            _mockUow.Setup(u => u.Accessories.AnyAsync(It.IsAny<Expression<Func<Accessory, bool>>>())).ReturnsAsync(false);

            // Act
            var result = await _service.CreateAsync(request);

            // Assert
            Assert.True(result.IsSuccess);
            _mockUow.Verify(u => u.Accessories.AddAsync(It.IsAny<Accessory>()), Times.Once);
            _mockUow.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_DuplicateSku_ThrowsBadRequestException()
        {
            // Arrange
            var request = new CreateAccessoryRequest { Sku = "EXISTING-SKU" };
            
            // SKU exists in Products
            _mockUow.Setup(u => u.Products.AnyAsync(It.IsAny<Expression<Func<Product, bool>>>())).ReturnsAsync(true);

            // Act & Assert
            await Assert.ThrowsAsync<DesignABear.Services.Exceptions.BadRequestException>(() => _service.CreateAsync(request));
        }

        [Fact]
        public async Task UpdateAsync_ValidRequest_ReturnsSuccess()
        {
            // Arrange
            var accId = Guid.NewGuid();
            var accessory = new Accessory { AccessoryId = accId, Name = "Old Name" };
            var request = new UpdateAccessoryRequest { Name = "New Name" };

            _mockUow.Setup(u => u.Accessories.GetByIdAsync(accId)).ReturnsAsync(accessory);

            // Act
            var result = await _service.UpdateAsync(accId, request);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("New Name", accessory.Name);
            _mockUow.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ValidId_ReturnsSuccess()
        {
            // Arrange
            var accId = Guid.NewGuid();
            var accessory = new Accessory { AccessoryId = accId };

            _mockUow.Setup(u => u.Accessories.GetByIdAsync(accId)).ReturnsAsync(accessory);

            // Act
            var result = await _service.DeleteAsync(accId);

            // Assert
            Assert.True(result.IsSuccess);
            _mockUow.Verify(u => u.Accessories.Remove(accessory), Times.Once);
            _mockUow.Verify(u => u.SaveChangesAsync(), Times.Once);
        }
    }
}
