using AutoMapper;
using DesignABear.Repositories.Models;
using DesignABear.Repositories.Repositories.Interfaces;
using DesignABear.Repositories.UnitOfWork;
using DesignABear.Services.DTOs.Requests;
using DesignABear.Services.Services.Implementations;
using DesignABear.Services.Services.Interfaces;
using Moq;
using System.Linq.Expressions;
using Xunit;

namespace DesignABear.Tests
{
    public class ProductServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUow;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ICacheService> _mockCache;
        private readonly Mock<IProductRepository> _mockProductRepo;
        private readonly Mock<IAccessoryRepository> _mockAccessoryRepo;
        private readonly Mock<IProductComboImageRepository> _mockComboRepo;
        private readonly Mock<IGenericRepository<ProductVariant>> _mockVariantRepo;
        private readonly Mock<ICategoryRepository> _mockCategoryRepo;
        private readonly Mock<ICharacterRepository> _mockCharacterRepo;
        private readonly ProductService _productService;

        public ProductServiceTests()
        {
            _mockUow = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _mockCache = new Mock<ICacheService>();
            _mockProductRepo = new Mock<IProductRepository>();
            _mockAccessoryRepo = new Mock<IAccessoryRepository>();
            _mockComboRepo = new Mock<IProductComboImageRepository>();
            _mockVariantRepo = new Mock<IGenericRepository<ProductVariant>>();
            _mockCategoryRepo = new Mock<ICategoryRepository>();
            _mockCharacterRepo = new Mock<ICharacterRepository>();

            // Setup repository mocks on UoW
            _mockUow.Setup(u => u.Products).Returns(_mockProductRepo.Object);
            _mockUow.Setup(u => u.Accessories).Returns(_mockAccessoryRepo.Object);
            _mockUow.Setup(u => u.ProductComboImages).Returns(_mockComboRepo.Object);
            _mockUow.Setup(u => u.ProductVariants).Returns(_mockVariantRepo.Object);
            _mockUow.Setup(u => u.Categories).Returns(_mockCategoryRepo.Object);
            _mockUow.Setup(u => u.Characters).Returns(_mockCharacterRepo.Object);

            _mockProductRepo.Setup(u => u.AnyAsync(It.IsAny<Expression<Func<Product, bool>>>()))
                .ReturnsAsync(false);
            _mockAccessoryRepo.Setup(u => u.AnyAsync(It.IsAny<Expression<Func<Accessory, bool>>>()))
                .ReturnsAsync(false);
            _mockVariantRepo.Setup(u => u.AnyAsync(It.IsAny<Expression<Func<ProductVariant, bool>>>()))
                .ReturnsAsync(false);

            // Default FindAsync for categories/characters
            _mockCategoryRepo.Setup(u => u.FindAsync(It.IsAny<Expression<Func<Category, bool>>>()))
                .ReturnsAsync(new List<Category>());
            _mockCharacterRepo.Setup(u => u.FindAsync(It.IsAny<Expression<Func<Character, bool>>>()))
                .ReturnsAsync(new List<Character>());
            
            // Default FindAsync for ComboImages
            _mockComboRepo.Setup(u => u.FindAsync(It.IsAny<Expression<Func<ProductComboImage, bool>>>()))
                .ReturnsAsync(new List<ProductComboImage>());

            _productService = new ProductService(_mockUow.Object, _mockMapper.Object, _mockCache.Object);
        }

        [Fact]
        public async Task CreateAsync_WithoutSlug_GeneratesSlugFromNameAndSku()
        {
            // Arrange
            var request = new CreateProductRequest
            {
                Name = "Gấu Bông Cute",
                Variants = new List<ProductVariantInlineRequest> { new ProductVariantInlineRequest { SizeTag = "OS" } }
            };

            _mockProductRepo.Setup(u => u.AnyAsync(It.IsAny<Expression<Func<Product, bool>>>()))
                .ReturnsAsync(false);

            // Mock mapper to return a product with the slug from the request
            _mockMapper.Setup(m => m.Map<Product>(It.IsAny<CreateProductRequest>()))
                .Returns((CreateProductRequest r) => new Product 
                { 
                    Name = r.Name, 
                    Slug = r.Slug 
                });

            // Act
            await _productService.CreateAsync(request);

            // Assert
            Assert.Equal("gau-bong-cute", request.Slug);
            _mockProductRepo.Verify(u => u.AddAsync(It.Is<Product>(p => p.Slug == "gau-bong-cute")), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_WithDuplicateSlug_AppendsCounter()
        {
            // Arrange
            var request = new CreateProductRequest
            {
                Name = "Gấu Bông Cute",
                Variants = new List<ProductVariantInlineRequest> { new ProductVariantInlineRequest { SizeTag = "OS" } }
            };

            _mockProductRepo.SetupSequence(u => u.AnyAsync(It.IsAny<Expression<Func<Product, bool>>>()))
                .ReturnsAsync(true)  // First slug check
                .ReturnsAsync(false) // Second slug check (unique)
                .ReturnsAsync(false); // SKU check

            _mockMapper.Setup(m => m.Map<Product>(It.IsAny<CreateProductRequest>()))
                .Returns((CreateProductRequest r) => new Product { Slug = r.Slug });

            // Act
            await _productService.CreateAsync(request);

            // Assert
            Assert.Equal("gau-bong-cute-1", request.Slug);
        }

        [Fact]
        public async Task CreateAsync_DuplicateSku_ThrowsBadRequestException()
        {
            // Arrange
            var request = new CreateProductRequest
            {
                Sku = "BEAR",
                Variants = new List<ProductVariantInlineRequest>
                {
                    new ProductVariantInlineRequest { SizeTag = "S" }
                }
            };

            // Mock SKU exists in Accessories
            _mockAccessoryRepo.Setup(u => u.AnyAsync(It.IsAny<Expression<Func<Accessory, bool>>>()))
                .ReturnsAsync(true);
            
            _mockMapper.Setup(m => m.Map<Product>(request))
                .Returns(new Product { ProductId = Guid.NewGuid(), Variants = new List<ProductVariant>() });

            // Act & Assert
            await Assert.ThrowsAsync<DesignABear.Services.Exceptions.BadRequestException>(() => _productService.CreateAsync(request));
        }


        [Fact]
        public async Task UpdateAsync_ValidRequest_ReturnsSuccess()
        {
            var productId = Guid.NewGuid();
            var product = new Product { ProductId = productId, Name = "Old Name", Slug = "old-name" };
            var request = new UpdateProductRequest { Name = "New Name", Slug = "new-name" };

            _mockProductRepo.Setup(u => u.GetWithDetailsAsync(productId)).ReturnsAsync(product);
            _mockMapper.Setup(m => m.Map(request, product));
            
            // Setup for Slug unique check during update
            _mockProductRepo.Setup(u => u.AnyAsync(It.IsAny<Expression<Func<Product, bool>>>()))
                .ReturnsAsync(false);

            // Act
            await _productService.UpdateAsync(productId, request);

            // Assert
            _mockUow.Verify(u => u.SaveChangesWithTransactionAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ValidId_ReturnsSuccess()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var product = new Product { ProductId = productId };

            _mockProductRepo.Setup(u => u.GetByIdAsync(productId)).ReturnsAsync(product);

            // Act
            await _productService.DeleteAsync(productId);

            // Assert
            _mockProductRepo.Verify(u => u.Remove(product), Times.Once);
            _mockUow.Verify(u => u.SaveChangesAsync(), Times.Once);
        }
    }
}
