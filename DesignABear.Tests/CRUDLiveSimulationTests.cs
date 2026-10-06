using AutoMapper;
using DesignABear.Repositories.Models;
using DesignABear.Services.DTOs.Responses;
using DesignABear.Services.DTOs.Requests;
using DesignABear.Services.Mappings;
using Xunit;
using System.Collections.Generic;
using System.Linq;

namespace DesignABear.Tests
{
    public class CRUDLiveSimulationTests
    {
        private readonly IMapper _mapper;

        public CRUDLiveSimulationTests()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = config.CreateMapper();
        }

        [Fact]
        public void Test_CartItem_Mapping_WithBuild_ReturnsSizeAndAccessories()
        {
            // Arrange: Tạo dữ liệu giả mô phỏng kết quả từ DB (sau Include)
            var productId = Guid.NewGuid();
            var variantId = Guid.NewGuid();
            var buildId = Guid.NewGuid();

            var baseProduct = new Product 
            { 
                ProductId = productId, 
                Name = "Gấu Teddy Gốc", 
                Slug = "gau-teddy-goc",
                Media = new List<ProductMedia> { new ProductMedia { Url = "teddy.jpg" } }
            };

            var baseVariant = new ProductVariant 
            { 
                VariantId = variantId, 
                ProductId = productId, 
                SizeDescription = "60cm - Khổng lồ",
                SizeTag = "XL",
                Sku = "TEDDY-60CM"
            };

            var build = new Build
            {
                BuildId = buildId,
                BaseProductId = productId,
                BaseProduct = baseProduct,
                BaseVariant = baseVariant,
                BuildComponents = new List<BuildComponent>
                {
                    new BuildComponent
                    {
                        OptionProductId = Guid.NewGuid(),
                        OptionProduct = new Product { Name = "Áo Len Đỏ" },
                        Quantity = 1
                    },
                    new BuildComponent
                    {
                        OptionProductId = Guid.NewGuid(),
                        OptionProduct = new Product { Name = "Mũ Noel" },
                        Quantity = 1
                    }
                }
            };

            var cartItem = new CartItem
            {
                CartItemId = Guid.NewGuid(),
                BuildId = buildId,
                Build = build,
                Quantity = 1
            };

            // Act: Map sang DTO
            var response = _mapper.Map<CartItemResponse>(cartItem);

            // Assert: Kiểm tra cấu trúc trả về theo đúng yêu cầu
            Assert.Equal("Gấu Teddy Gốc", response.ProductName);
            Assert.Equal("teddy.jpg", response.ProductImageUrl);
            Assert.Equal("60cm - Khổng lồ", response.Size);
            Assert.Equal("XL", response.SizeTag);
            Assert.Equal("TEDDY-60CM", response.Sku);
            
            Assert.NotNull(response.BuildDetails);
            Assert.Equal(2, response.BuildDetails.BuildComponents.Count());
            
            var accessories = response.BuildDetails.BuildComponents.ToList();
            Assert.Equal("Áo Len Đỏ", accessories[0].ProductName);
            Assert.Equal("Mũ Noel", accessories[1].ProductName);
            Assert.NotEqual(Guid.Empty, accessories[0].OptionProductId);
        }

        [Fact]
        public void Test_ProductDetail_Mapping_IncludesStockForVariantsAndAccessories()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var variantId = Guid.NewGuid();
            var accessoryId = Guid.NewGuid();

            var product = new Product
            {
                ProductId = productId,
                Name = "Gấu Testing",
                Variants = new List<ProductVariant>
                {
                    new ProductVariant
                    {
                        VariantId = variantId,
                        ProductId = productId,
                        SizeTag = "M"
                    }
                },
                ProductAccessories = new List<ProductAccessory>
                {
                    new ProductAccessory
                    {
                        Accessory = new Accessory
                        {
                            AccessoryId = accessoryId,
                            Name = "Nơ Hồng"
                        }
                    }
                },
                Inventories = new List<Inventory>
                {
                    new Inventory { IdentityId = variantId, OnHand = 10, Reserved = 2, IsAccessory = false, ProductId = productId },
                    new Inventory { IdentityId = accessoryId, OnHand = 50, Reserved = 5, IsAccessory = true, ProductId = productId }
                }
            };

            // Act
            var response = _mapper.Map<ProductDetailResponse>(product);

            // Assert
            var variant = Assert.Single(response.Variants);
            Assert.Equal(10, variant.OnHand);
            Assert.Equal(2, variant.Reserved);
            Assert.Equal(8, variant.Available);

            var accessory = Assert.Single(response.Accessories);
            Assert.Equal("Nơ Hồng", accessory.Name);
            Assert.Equal(50, accessory.OnHand);
            Assert.Equal(5, accessory.Reserved);
            Assert.Equal(45, accessory.Available);
        }

        [Fact]
        public void Test_Accessory_Mapping_IsSuccessful()
        {
            // Arrange
            var accessoryId = Guid.NewGuid();
            var accessory = new Accessory
            {
                AccessoryId = accessoryId,
                Name = "Nơ Đỏ",
                Sku = "ACC-RED-1",
                BaseCost = 10,
                TargetPrice = 25
            };

            // Act
            var response = _mapper.Map<AccessoryResponse>(accessory);

            // Assert
            Assert.Equal(accessoryId, response.AccessoryId);
            Assert.Equal("Nơ Đỏ", response.Name);
            Assert.Equal("ACC-RED-1", response.Sku);
            Assert.Equal(10, response.BaseCost);
            Assert.Equal(25, response.TargetPrice);
        }

        [Fact]
        public void Test_Favorite_Mapping_IsMinimal()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var favorite = new Favorite
            {
                ProductId = productId,
                Product = new Product 
                { 
                    Name = "Gấu Bông Cute", 
                    Media = new List<ProductMedia> { new ProductMedia { Url = "cute.jpg" } } 
                }
            };

            // Act
            var response = _mapper.Map<FavoriteResponse>(favorite);

            // Assert: Chỉ có 3 trường yêu cầu
            Assert.Equal(productId, response.ProductId);
            Assert.Equal("Gấu Bông Cute", response.ProductName);
            Assert.Equal("cute.jpg", response.ProductImageUrl);
            
            // Kiểm tra xem các trường thừa có bị xóa khỏi DTO (compilation check)
            // Note: C# DTO đã được sửa nên nếu truy cập field cũ sẽ bị lỗi build
        }
    }
}
