using Moq;
using Xunit;
using DesignABear.Services.Services.Implementations;
using DesignABear.Repositories.UnitOfWork;
using AutoMapper;
using Microsoft.Extensions.Logging;
using DesignABear.Repositories.Models;
using DesignABear.Services.Exceptions;

namespace DesignABear.Tests
{
    public class InventoryServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUow;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<InventoryService>> _mockLogger;
        private readonly InventoryService _service;

        public InventoryServiceTests()
        {
            _mockUow = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<InventoryService>>();
            _service = new InventoryService(_mockUow.Object, _mockMapper.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task AdjustStockAsync_ShouldCreateRecord_WhenNotFoundAndDeltaPositive()
        {
            // Arrange
            var locationId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var delta = 10;

            _mockUow.Setup(u => u.Inventories.AtomicAdjustOnHandAsync(productId, false, delta, locationId))
                .ReturnsAsync(0); // No record found

            // Act
            await _service.AdjustStockAsync(productId, false, delta, locationId);

            // Assert
            _mockUow.Verify(u => u.Inventories.AddAsync(It.IsAny<Inventory>()), Times.Once);
            _mockUow.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task AdjustStockAsync_ShouldThrowException_WhenNotFoundAndDeltaNegative()
        {
            // Arrange
            var locationId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var delta = -5;

            _mockUow.Setup(u => u.Inventories.AtomicAdjustOnHandAsync(productId, false, delta, locationId))
                .ReturnsAsync(0);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => 
                _service.AdjustStockAsync(productId, false, delta, locationId));
        }

        [Fact]
        public async Task ConfirmDeductionAsync_ShouldDecrementOnHandAndReserved()
        {
            // Arrange
            var locationId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var quantity = 3;

            _mockUow.Setup(u => u.Inventories.AtomicDeductAsync(productId, false, quantity, locationId))
                .ReturnsAsync(1); // 1 row affected

            // Act
            await _service.ConfirmDeductionAsync(productId, false, quantity, locationId);

            // Assert
            _mockUow.Verify(u => u.Inventories.AtomicDeductAsync(productId, false, quantity, locationId), Times.Once);
        }

        [Fact]
        public async Task ReleaseReservationAsync_ShouldDecrementReservedAndKeepPositive()
        {
            // Arrange
            var locationId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var quantity = 5;

            _mockUow.Setup(u => u.Inventories.AtomicReleaseAsync(productId, false, quantity, locationId))
                .ReturnsAsync(1);

            // Act
            await _service.ReleaseReservationAsync(productId, false, quantity, locationId);

            // Assert
            _mockUow.Verify(u => u.Inventories.AtomicReleaseAsync(productId, false, quantity, locationId), Times.Once);
        }
    }
}
