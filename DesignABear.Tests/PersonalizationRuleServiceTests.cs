using AutoMapper;
using DesignABear.Repositories.Models;
using DesignABear.Repositories.UnitOfWork;
using DesignABear.Services.Services.Implementations;
using Moq;
using Xunit;

namespace DesignABear.Tests
{
    public class PersonalizationRuleServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUow;
        private readonly Mock<IMapper> _mockMapper;
        private readonly PersonalizationRuleService _service;

        public PersonalizationRuleServiceTests()
        {
            _mockUow = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _service = new PersonalizationRuleService(_mockUow.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetCombinedImageUrlAsync_EmptyIds_ReturnsNull()
        {
            // Arrange
            var baseProductId = Guid.NewGuid();
            var ids = new List<Guid>();

            // Act
            var result = await _service.GetCombinedImageUrlAsync(baseProductId, ids);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetCombinedImageUrlAsync_NullIds_ReturnsNull()
        {
            // Act
            var result = await _service.GetCombinedImageUrlAsync(Guid.NewGuid(), null!);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetCombinedImageUrlAsync_ValidIds_ReturnsImageUrl()
        {
            // Arrange
            var baseProductId = Guid.NewGuid();
            var id1 = Guid.Parse("00000000-0000-0000-0000-000000000001");
            var id2 = Guid.Parse("00000000-0000-0000-0000-000000000002");
            var ids = new List<Guid> { id1, id2 };
            
            // Expected key is sorted: A|B
            var expectedKey = $"{id1}|{id2}";
            var expectedUrl = "http://example.com/combo.jpg";

            _mockUow.Setup(u => u.ProductComboImages.GetByCombinationKeyAsync(baseProductId, expectedKey))
                .ReturnsAsync(new ProductComboImage { ImageUrl = expectedUrl });

            // Act
            var result = await _service.GetCombinedImageUrlAsync(baseProductId, ids);

            // Assert
            Assert.Equal(expectedUrl, result);
        }

        [Fact]
        public async Task GetCombinedImageUrlAsync_UnsortedIds_HandlesSortingCorrectly()
        {
            // Arrange
            var baseProductId = Guid.NewGuid();
            var id1 = Guid.Parse("00000000-0000-0000-0000-000000000001");
            var id2 = Guid.Parse("00000000-0000-0000-0000-000000000002");
            
            // Input is unsorted: B, A
            var ids = new List<Guid> { id2, id1 };
            
            // Expected key should still be A|B because of OrderBy
            var expectedKey = $"{id1}|{id2}";
            var expectedUrl = "http://example.com/combo.jpg";

            _mockUow.Setup(u => u.ProductComboImages.GetByCombinationKeyAsync(baseProductId, expectedKey))
                .ReturnsAsync(new ProductComboImage { ImageUrl = expectedUrl });

            // Act
            var result = await _service.GetCombinedImageUrlAsync(baseProductId, ids);

            // Assert
            Assert.Equal(expectedUrl, result);
            _mockUow.Verify(u => u.ProductComboImages.GetByCombinationKeyAsync(baseProductId, expectedKey), Times.Once);
        }

        [Fact]
        public async Task GetCombinedImageUrlAsync_NonExistentCombo_ReturnsNull()
        {
            // Arrange
            var baseProductId = Guid.NewGuid();
            var ids = new List<Guid> { Guid.NewGuid() };

            _mockUow.Setup(u => u.ProductComboImages.GetByCombinationKeyAsync(It.IsAny<Guid>(), It.IsAny<string>()))
                .ReturnsAsync((ProductComboImage?)null);

            // Act
            var result = await _service.GetCombinedImageUrlAsync(baseProductId, ids);

            // Assert
            Assert.Null(result);
        }
    }
}
