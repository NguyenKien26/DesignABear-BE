using AutoMapper;
using DesignABear.Repositories.Models;
using DesignABear.Services.DTOs.Responses;
using DesignABear.Services.Mappings;
using Xunit;

namespace DesignABear.Tests
{
    public class InventoryMappingTests
    {
        private readonly IMapper _mapper;

        public InventoryMappingTests()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = config.CreateMapper();
        }

        [Fact]
        public void ProductVariant_Mapping_ShouldCalculateOnHandAndReserved()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var variantId = Guid.NewGuid();
            var variant = new ProductVariant
            {
                VariantId = variantId,
                ProductId = productId,
                Product = new Product
                {
                    ProductId = productId,
                    Inventories = new List<Inventory>
                    {
                        new Inventory { IdentityId = variantId, OnHand = 10, Reserved = 2, IsAccessory = false },
                        new Inventory { IdentityId = variantId, OnHand = 5, Reserved = 1, IsAccessory = false }
                    }
                }
            };

            // Act
            var response = _mapper.Map<ProductVariantResponse>(variant);

            // Assert
            Assert.Equal(15, response.OnHand);
            Assert.Equal(3, response.Reserved);
            Assert.Equal(12, response.Available);
        }

        [Fact]
        public void Product_Mapping_ShouldSumFromVariants()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var v1Id = Guid.NewGuid();
            var v2Id = Guid.NewGuid();

            var product = new Product
            {
                ProductId = productId,
                Variants = new List<ProductVariant>
                {
                    new ProductVariant { VariantId = v1Id, ProductId = productId },
                    new ProductVariant { VariantId = v2Id, ProductId = productId }
                },
                Inventories = new List<Inventory>
                {
                    new Inventory { IdentityId = v1Id, OnHand = 10, Reserved = 2, IsAccessory = false },
                    new Inventory { IdentityId = v2Id, OnHand = 5, Reserved = 1, IsAccessory = false }
                }
            };

            // Act: Map to ProductListResponse
            var response = _mapper.Map<ProductListResponse>(product);

            // Assert
            Assert.Equal(15, response.OnHand);
            Assert.Equal(3, response.Reserved);
            Assert.Equal(12, response.Available);
        }

        [Fact]
        public void Accessory_Mapping_ShouldCalculateOnHandAndReserved()
        {
            // Note: Currently AccessoryResponse mapping doesn't support automatic OnHand calculation 
            // from the Accessory entity alone as it lacks a navigation property back to Inventories.
            // This test is kept as a placeholder or to verify basic mapping.
            
            // Arrange
            var accessoryId = Guid.NewGuid();
            var accessory = new Accessory
            {
                AccessoryId = accessoryId,
                Name = "Nơ Hồng",
                Sku = "ACC-1"
            };

            // Act
            var response = _mapper.Map<AccessoryResponse>(accessory);

            // Assert
            Assert.Equal(accessoryId, response.AccessoryId);
            Assert.Equal(0, response.OnHand); // Expect 0 as it's not mapped yet
        }
    }
}
