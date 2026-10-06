using AutoMapper;
using DesignABear.Repositories.Models;
using DesignABear.Services.DTOs.Requests;
using DesignABear.Services.DTOs.Responses;
using System.Linq;
using System.Text.Json;

namespace DesignABear.Services.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // User mappings
            CreateMap<User, UserResponse>()
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role != null ? src.Role.RoleName : string.Empty))
                .ForMember(dest => dest.WorkplaceName, opt => opt.MapFrom(src => src.Workplace != null ? src.Workplace.Name : null));
            CreateMap<CreateUserRequest, User>();
            CreateMap<UpdateUserRequest, User>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Role, RoleResponse>();
            CreateMap<CreateRoleRequest, Role>();
            CreateMap<UpdateRoleRequest, Role>();

            // Accessory mappings
            CreateMap<Accessory, AccessoryResponse>();
            CreateMap<CreateAccessoryRequest, Accessory>();
            CreateMap<UpdateAccessoryRequest, Accessory>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));



            // Address mappings
            CreateMap<Address, AddressResponse>();
            CreateMap<CreateAddressRequest, Address>();
            CreateMap<UpdateAddressRequest, Address>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Category mappings
            CreateMap<Category, CategoryResponse>();
            CreateMap<CreateCategoryRequest, Category>();
            CreateMap<UpdateCategoryRequest, Category>();

            // Product mappings
            CreateMap<Product, ProductListResponse>()
                .ForMember(dest => dest.ShortDescription, opt => opt.MapFrom(src => 
                    string.IsNullOrEmpty(src.Description) ? "" : 
                    (src.Description.Length > 100 ? src.Description.Substring(0, 100) + "..." : src.Description)))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => 
                    src.Media != null && src.Media.Any() ? src.Media.First().Url : null))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Variants.Any() ? src.Variants.Min(v => v.Price) : 0))
                .ForMember(dest => dest.MinPrice, opt => opt.MapFrom(src => src.Variants.Any() ? src.Variants.Min(v => v.Price) : 0))
                .ForMember(dest => dest.BaseCost, opt => opt.MapFrom(src => src.Variants.Any() ? src.Variants.First().BaseCost : 0))
                .ForMember(dest => dest.AssemblyCost, opt => opt.MapFrom(src => src.Variants.Any() ? src.Variants.First().AssemblyCost : 0))
                .ForMember(dest => dest.TotalSales, opt => opt.MapFrom(src => src.TotalSales))
                .ForMember(dest => dest.ViewCountIn10Min, opt => opt.MapFrom(src => src.DynamicStats != null ? src.DynamicStats.ViewCountIn10Min : 0))
                .ForMember(dest => dest.DiscountRate, opt => opt.MapFrom(src => src.DynamicStats != null ? src.DynamicStats.DiscountRate : 0))
                .ForMember(dest => dest.AverageRating, opt => opt.MapFrom(src => 
                    src.Reviews != null && src.Reviews.Any() ? src.Reviews.Average(r => r.Rating) : 0))
                .ForMember(dest => dest.ReviewCount, opt => opt.MapFrom(src => 
                    src.Reviews != null ? src.Reviews.Count() : 0))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.Media, opt => opt.MapFrom(src => src.Media))
                .ForMember(dest => dest.Variants, opt => opt.MapFrom(src => src.Variants))
                .ForMember(dest => dest.OnHand, opt => opt.MapFrom(src => src.Inventories.Sum(i => i.OnHand)))
                .ForMember(dest => dest.Reserved, opt => opt.MapFrom(src => src.Inventories.Sum(i => i.Reserved)))
                .ForMember(dest => dest.ProductType, opt => opt.MapFrom(_ => "Standard"));
 
            CreateMap<Product, ProductResponse>()
                .ForMember(dest => dest.Media, opt => opt.MapFrom(src => src.Media))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Variants.Any() ? src.Variants.Min(v => v.Price) : 0))
                .ForMember(dest => dest.BaseCost, opt => opt.MapFrom(src => src.Variants.Any() ? src.Variants.First().BaseCost : 0))
                .ForMember(dest => dest.AssemblyCost, opt => opt.MapFrom(src => src.Variants.Any() ? src.Variants.First().AssemblyCost : 0))
                .ForMember(dest => dest.Sku, opt => opt.MapFrom(src => src.Sku))
                .ForMember(dest => dest.WeightGram, opt => opt.MapFrom(src => src.Variants.Any() ? src.Variants.First().WeightGram : 0f))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => 
                    src.Media != null && src.Media.Any() ? src.Media.First().Url : null))
                .ForMember(dest => dest.OnHand, opt => opt.MapFrom(src => src.Inventories.Sum(i => i.OnHand)))
                .ForMember(dest => dest.Reserved, opt => opt.MapFrom(src => src.Inventories.Sum(i => i.Reserved)))
                .ForMember(dest => dest.ProductType, opt => opt.MapFrom(_ => "Standard"));

            CreateMap<Product, ProductDetailResponse>()
                .IncludeBase<Product, ProductResponse>()
                .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.ProductCategories.Select(pc => pc.Category)))
                .ForMember(dest => dest.Characters, opt => opt.MapFrom(src => src.ProductCharacters.Select(pc => pc.Character)))
                .ForMember(dest => dest.Accessories, opt => opt.MapFrom(src => src.ProductAccessories.Select(pa => pa.Accessory)))
                .ForMember(dest => dest.ComboImages, opt => opt.MapFrom(src => src.ComboImages))
                .ForMember(dest => dest.Variants, opt => opt.MapFrom(src => src.Variants));

            CreateMap<PersonalizationRule, PersonalizationRuleResponse>()
                .ForMember(dest => dest.AllowedComponentProductId, opt => opt.MapFrom(src => src.AddonProductId))
                .ForMember(dest => dest.AddonProduct, opt => opt.MapFrom(src => src.AddonProduct))
                .ForMember(dest => dest.AddonProductName, opt => opt.MapFrom(src => src.AddonProduct != null ? src.AddonProduct.Name : null))
                .ForMember(dest => dest.GroupName, opt => opt.MapFrom(src => src.Group != null ? src.Group.Name : null));

            CreateMap<Product, ProductFullDetailResponse>()
                .IncludeBase<Product, ProductDetailResponse>()
                .ForMember(dest => dest.Collections, opt => opt.MapFrom(src => src.ProductCollections.Select(pc => pc.Collection)))
                .ForMember(dest => dest.ViewCount, opt => opt.MapFrom(src => src.DynamicStats != null ? src.DynamicStats.ViewCountIn10Min : 0))
                .ForMember(dest => dest.DiscountRate, opt => opt.MapFrom(src => src.DynamicStats != null ? (float)src.DynamicStats.DiscountRate : 0f));

            CreateMap<ProductVariant, ProductVariantResponse>()
                .ForMember(dest => dest.OnHand, opt => opt.MapFrom(src => 
                    src.Product != null ? src.Product.Inventories.Where(i => i.IdentityId == src.VariantId).Sum(i => i.OnHand) : 0))
                .ForMember(dest => dest.Reserved, opt => opt.MapFrom(src => 
                    src.Product != null ? src.Product.Inventories.Where(i => i.IdentityId == src.VariantId).Sum(i => i.Reserved) : 0));

            CreateMap<ProductVariantInlineRequest, ProductVariant>();
            CreateMap<CreateProductRequest, Product>()
                .ForMember(dest => dest.Variants, opt => opt.Ignore())
                .ForMember(dest => dest.Media, opt => opt.Ignore())
                .ForMember(dest => dest.ProductCategories, opt => opt.Ignore())
                .ForMember(dest => dest.ProductCharacters, opt => opt.Ignore())
                .ForMember(dest => dest.ProductAccessories, opt => opt.Ignore())
                .ForMember(dest => dest.ComboImages, opt => opt.Ignore());
            CreateMap<UpdateProductRequest, Product>()
                .ForMember(dest => dest.Variants, opt => opt.Ignore())
                .ForMember(dest => dest.Media, opt => opt.Ignore())
                .ForMember(dest => dest.ProductCategories, opt => opt.Ignore())
                .ForMember(dest => dest.ProductCharacters, opt => opt.Ignore())
                .ForMember(dest => dest.ProductAccessories, opt => opt.Ignore())
                .ForMember(dest => dest.ComboImages, opt => opt.Ignore());



            // ProductMedia mappings
            CreateMap<ProductMedia, ProductMediaResponse>();
            CreateMap<CreateProductMediaRequest, ProductMedia>();
            CreateMap<UpdateProductMediaRequest, ProductMedia>();

            // ProductComboImage mappings
            CreateMap<ProductComboImage, ProductComboImageResponse>();

            CreateMap<Accessory, AccessoryResponse>();

            // Cart mappings
            CreateMap<Cart, CartResponse>();
            CreateMap<CreateCartRequest, Cart>();
            CreateMap<UpdateCartRequest, Cart>();

            // CartItem mappings
            CreateMap<CartItem, CartItemResponse>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src =>
                    src.ProductId ?? (src.Variant != null ? src.Variant.ProductId : (src.Build != null ? src.Build.BaseProductId : (Guid?)null))))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src =>
                    src.Variant != null && src.Variant.Product != null ? src.Variant.Product.Name :
                    src.Build != null && src.Build.BaseProduct != null ? src.Build.BaseProduct.Name :
                    src.Product != null ? src.Product.Name : null))
                .ForMember(dest => dest.ProductSlug, opt => opt.MapFrom(src =>
                    src.Variant != null && src.Variant.Product != null ? src.Variant.Product.Slug :
                    src.Build != null && src.Build.BaseProduct != null ? src.Build.BaseProduct.Slug :
                    src.Product != null ? src.Product.Slug : null))
                .ForMember(dest => dest.Sku, opt => opt.MapFrom(src =>
                    src.Variant != null ? src.Variant.Sku :
                    src.Build != null && src.Build.BaseVariant != null ? src.Build.BaseVariant.Sku :
                    src.Product != null ? src.Product.Sku : null))
                .ForMember(dest => dest.Size, opt => opt.MapFrom(src =>
                    src.Build != null && !string.IsNullOrEmpty(src.Build.SizeDescription) ? src.Build.SizeDescription :
                    src.Variant != null ? src.Variant.SizeDescription : 
                    src.Build != null && src.Build.BaseVariant != null ? src.Build.BaseVariant.SizeDescription : null))
                .ForMember(dest => dest.SizeTag, opt => opt.MapFrom(src => 
                    src.Build != null && !string.IsNullOrEmpty(src.Build.SizeTag) ? src.Build.SizeTag :
                    src.Variant != null ? src.Variant.SizeTag : 
                    src.Build != null && src.Build.BaseVariant != null ? src.Build.BaseVariant.SizeTag : null))
                .ForMember(dest => dest.ProductImageUrl, opt => opt.MapFrom(src => 
                    (src.Variant != null && src.Variant.Product != null && src.Variant.Product.Media != null && src.Variant.Product.Media.Any()) ? src.Variant.Product.Media.First().Url :
                    (src.Build != null && src.Build.BaseProduct != null && src.Build.BaseProduct.Media != null && src.Build.BaseProduct.Media.Any()) ? src.Build.BaseProduct.Media.First().Url :
                    (src.Product != null && src.Product.Media != null && src.Product.Media.Any()) ? src.Product.Media.First().Url : null))
                .ForMember(dest => dest.AvailableStock, opt => opt.Ignore()) // Polished in Service
                .ForMember(dest => dest.BuildDetails, opt => opt.MapFrom(src => src.Build));
            CreateMap<CreateCartItemRequest, CartItem>();
            CreateMap<UpdateCartItemRequest, CartItem>();

            // Order mappings
            CreateMap<Order, OrderResponse>()
                .ForMember(dest => dest.IsPaid, opt => opt.MapFrom(src => 
                    src.Payments.Any(p => p.Status == "CAPTURED" || p.Status == "PAID" || p.Status == "SUCCESS")));
            CreateMap<CreateOrderRequest, Order>();
            CreateMap<UpdateOrderRequest, Order>();
            CreateMap<OrderRefund, RefundResponse>()
                .ForMember(dest => dest.EvidenceUrls, opt => opt.MapFrom(src => 
                    !string.IsNullOrEmpty(src.EvidenceUrls) 
                    ? JsonSerializer.Deserialize<string[]>(src.EvidenceUrls, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) 
                    : null));

            // ProductIssueReport mappings
            CreateMap<ProductIssueReport, ProductIssueReportResponse>()
                .ForMember(dest => dest.EvidenceUrls, opt => opt.MapFrom(src => 
                    !string.IsNullOrEmpty(src.EvidenceUrls) 
                    ? JsonSerializer.Deserialize<string[]>(src.EvidenceUrls, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) 
                    : null));
            CreateMap<CreateProductIssueReportRequest, ProductIssueReport>()
                .ForMember(dest => dest.EvidenceUrls, opt => opt.MapFrom(src => 
                    src.EvidenceUrls != null 
                    ? JsonSerializer.Serialize(src.EvidenceUrls, (JsonSerializerOptions?)null) 
                    : null));

            // Favorite mappings
            CreateMap<Favorite, FavoriteResponse>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.ProductImageUrl, opt => opt.MapFrom(src => 
                    src.Product.Media != null && src.Product.Media.Any() ? src.Product.Media.First().Url : null));

            // OrderItem mappings
            CreateMap<OrderItem, OrderItemResponse>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => 
                    src.ProductId ?? 
                    (src.Variant != null ? src.Variant.ProductId : 
                    (src.Build != null ? src.Build.BaseProductId : (Guid?)null))))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src =>
                    !string.IsNullOrEmpty(src.ProductNameSnapshot)
                        ? src.ProductNameSnapshot
                        : (src.Variant != null && src.Variant.Product != null
                            ? src.Variant.Product.Name
                            : (src.Build != null && src.Build.BaseProduct != null 
                                ? src.Build.BaseProduct.Name : "Sản phẩm"))))
                .ForMember(dest => dest.ProductImageUrl, opt => opt.MapFrom(src => 
                    (src.Variant != null && src.Variant.Product != null && src.Variant.Product.Media.Any()) ? src.Variant.Product.Media.First().Url :
                    (src.Build != null && src.Build.BaseProduct != null && src.Build.BaseProduct.Media.Any()) ? src.Build.BaseProduct.Media.First().Url : null))
                .ForMember(dest => dest.Sku, opt => opt.MapFrom(src => src.Variant != null ? src.Variant.Sku : null))
                .ForMember(dest => dest.Size, opt => opt.MapFrom(src => src.Variant != null ? src.Variant.SizeDescription : null))
                .ForMember(dest => dest.SizeTag, opt => opt.MapFrom(src => src.Variant != null ? src.Variant.SizeTag : null))
                .ForMember(dest => dest.BuildDetails, opt => opt.MapFrom(src => src.Build));
            CreateMap<CreateOrderItemRequest, OrderItem>();
            CreateMap<UpdateOrderItemRequest, OrderItem>();

            // Collection mappings
            CreateMap<Collection, CollectionResponse>();
            CreateMap<CreateCollectionRequest, Collection>();
            CreateMap<UpdateCollectionRequest, Collection>();

            // Character mappings
            CreateMap<Character, CharacterResponse>();
            CreateMap<CreateCharacterRequest, Character>();
            CreateMap<UpdateCharacterRequest, Character>();

            // Promotion mappings
            CreateMap<Promotion, PromotionResponse>();
            CreateMap<CreatePromotionRequest, Promotion>();
            CreateMap<UpdatePromotionRequest, Promotion>();

            // Review mappings
            CreateMap<Review, ReviewResponse>()
                .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src =>
                    src.User != null ? src.User.FullName : "User"))
                .ForMember(dest => dest.AuthorAvatar, opt => opt.MapFrom(src =>
                    src.User != null ? src.User.AvatarUrl : null)); ;
            CreateMap<CreateReviewRequest, Review>();
            CreateMap<UpdateReviewRequest, Review>();

            // ReviewReply mappings
            CreateMap<ReviewReply, ReviewReplyResponse>();
            CreateMap<CreateReviewReplyRequest, ReviewReply>();
            CreateMap<UpdateReviewReplyRequest, ReviewReply>();

            // Build mappings
            CreateMap<Build, BuildResponse>()
                .ForMember(dest => dest.BaseProductName, opt => opt.MapFrom(src => src.BaseProduct != null ? src.BaseProduct.Name : null))
                .ForMember(dest => dest.BaseProductImageUrl, opt => opt.MapFrom(src => (src.BaseProduct != null && src.BaseProduct.Media.Any()) ? src.BaseProduct.Media.First().Url : null))
                .ForMember(dest => dest.SizeTag, opt => opt.MapFrom(src => src.SizeTag))
                .ForMember(dest => dest.SizeDescription, opt => opt.MapFrom(src => src.SizeDescription));
            CreateMap<CreateBuildRequest, Build>();
            CreateMap<UpdateBuildRequest, Build>();

            // BuildComponent mappings
            CreateMap<BuildComponent, BuildComponentResponse>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.OptionProduct != null ? src.OptionProduct.Name : null))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => (src.OptionProduct != null && src.OptionProduct.Media.Any()) ? src.OptionProduct.Media.First().Url : null))
                .ForMember(dest => dest.Size, opt => opt.MapFrom(src => 
                    src.OptionVariant != null ? src.OptionVariant.SizeDescription :
                    (src.OptionProduct != null && src.OptionProduct.Variants.Any() ? src.OptionProduct.Variants.First().SizeDescription : null)))
                .ForMember(dest => dest.WeightGram, opt => opt.MapFrom(src => 
                    src.OptionVariant != null ? src.OptionVariant.WeightGram :
                    (src.OptionProduct != null && src.OptionProduct.Variants.Any() ? src.OptionProduct.Variants.First().WeightGram : 0f)));
            CreateMap<CreateBuildComponentRequest, BuildComponent>();
            CreateMap<UpdateBuildComponentRequest, BuildComponent>();

            // Fulfillment mappings
            CreateMap<Fulfillment, FulfillmentResponse>();
            CreateMap<CreateFulfillmentRequest, Fulfillment>();
            CreateMap<UpdateFulfillmentRequest, Fulfillment>();

            CreateMap<Inventory, InventoryResponse>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => 
                    src.Product != null ? src.Product.Name : "Unknown"))
                .ForMember(dest => dest.ProductType, opt => opt.MapFrom(src => 
                    src.IsAccessory ? "Accessory" : "Standard"))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => 
                    src.Product != null && src.Product.Media.Any() ? src.Product.Media.First().Url : null))
                .ForMember(dest => dest.OnHand, opt => opt.MapFrom(src => src.OnHand))
                .ForMember(dest => dest.Reserved, opt => opt.MapFrom(src => src.Reserved));
            CreateMap<CreateInventoryRequest, Inventory>();
            CreateMap<UpdateInventoryRequest, Inventory>();

            // Location mappings
            CreateMap<Location, LocationResponse>();
            CreateMap<CreateLocationRequest, Location>();
            CreateMap<UpdateLocationRequest, Location>();

            // Payment mappings
            CreateMap<Payment, PaymentResponse>();
            CreateMap<CreatePaymentRequest, Payment>()
                .ForMember(dest => dest.ExternalTxnId, opt => opt.MapFrom(src => src.TransactionId))
                .ForMember(dest => dest.Provider, opt => opt.MapFrom(src => src.PaymentMethod));
            CreateMap<UpdatePaymentRequest, Payment>();

            // PersonalizationGroup mappings
            CreateMap<PersonalizationGroup, PersonalizationGroupResponse>();
            CreateMap<CreatePersonalizationGroupRequest, PersonalizationGroup>();
            CreateMap<UpdatePersonalizationGroupRequest, PersonalizationGroup>();

            CreateMap<CreatePersonalizationRuleRequest, PersonalizationRule>()
                .ForMember(dest => dest.AddonProductId, opt => opt.MapFrom(src => src.AllowedComponentProductId))
                .ForMember(dest => dest.MaxQty, opt => opt.MapFrom(src => src.MaxQuantity));
            CreateMap<UpdatePersonalizationRuleRequest, PersonalizationRule>();

            // ProductionJob mappings
            CreateMap<ProductionJob, ProductionJobResponse>()
                .ForMember(dest => dest.AssignedUser, opt => opt.MapFrom(src => src.TechnicianId))
                .ForMember(dest => dest.CompletedAt, opt => opt.MapFrom(src => src.FinishedAt))
                .ForMember(dest => dest.QCInspectorId, opt => opt.MapFrom(src => src.QCInspectorId))
                .ForMember(dest => dest.QCFinishedAt, opt => opt.MapFrom(src => src.QCFinishedAt))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => 
                    (src.Build != null && src.Build.BaseProduct != null) ? src.Build.BaseProduct.Name :
                    (src.OrderItem != null && src.OrderItem.Build != null && src.OrderItem.Build.BaseProduct != null) ? src.OrderItem.Build.BaseProduct.Name :
                    (src.OrderItem != null ? (src.OrderItem.ProductNameSnapshot ?? (src.OrderItem.Variant != null && src.OrderItem.Variant.Product != null ? src.OrderItem.Variant.Product.Name : "Unknown")) : "Unknown")))
                .ForMember(dest => dest.BaseProductName, opt => opt.MapFrom(src => 
                    (src.Build != null && src.Build.BaseProduct != null) ? src.Build.BaseProduct.Name :
                    (src.OrderItem != null && src.OrderItem.Build != null && src.OrderItem.Build.BaseProduct != null) ? src.OrderItem.Build.BaseProduct.Name :
                    (src.OrderItem != null && src.OrderItem.Variant != null && src.OrderItem.Variant.Product != null ? src.OrderItem.Variant.Product.Name : src.OrderItem != null ? src.OrderItem.ProductNameSnapshot : null)))
                .ForMember(dest => dest.BuildName, opt => opt.MapFrom(src => 
                    src.Build != null ? src.Build.BuildName :
                    (src.OrderItem != null && src.OrderItem.Build != null ? src.OrderItem.Build.BuildName : null)))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => 
                    (src.Build != null && src.Build.BaseProduct != null && src.Build.BaseProduct.Media.Any()) ? src.Build.BaseProduct.Media.First().Url :
                    (src.OrderItem != null && src.OrderItem.Build != null && src.OrderItem.Build.BaseProduct != null && src.OrderItem.Build.BaseProduct.Media.Any()) ? src.OrderItem.Build.BaseProduct.Media.First().Url :
                    (src.OrderItem != null && src.OrderItem.Variant != null && src.OrderItem.Variant.Product != null && src.OrderItem.Variant.Product.Media.Any()) ? src.OrderItem.Variant.Product.Media.First().Url : null))
                .ForMember(dest => dest.Size, opt => opt.MapFrom(src => 
                    (src.Build != null && src.Build.BaseProduct != null) ? src.Build.BaseProduct.Name : // Building base product uses master name/size
                    (src.OrderItem != null && src.OrderItem.Variant != null) ? src.OrderItem.Variant.SizeDescription : null))
                .ForMember(dest => dest.SizeTag, opt => opt.MapFrom(src => 
                    (src.Build != null && src.Build.BaseVariant != null) ? src.Build.BaseVariant.SizeTag :
                    (src.OrderItem != null && src.OrderItem.Variant != null) ? src.OrderItem.Variant.SizeTag : null))
                .ForMember(dest => dest.WeightGram, opt => opt.MapFrom(src => 
                    (src.Build != null && src.Build.BaseVariant != null) ? src.Build.BaseVariant.WeightGram :
                    (src.OrderItem != null && src.OrderItem.Variant != null) ? src.OrderItem.Variant.WeightGram : 1000f))
                .ForMember(dest => dest.TechnicianName, opt => opt.MapFrom(src => (src.Technician != null) ? src.Technician.FullName : "N/A"))
                .ForMember(dest => dest.Components, opt => opt.MapFrom(src => 
                    (src.Build != null) ? src.Build.BuildComponents :
                    (src.OrderItem != null && src.OrderItem.Build != null) ? src.OrderItem.Build.BuildComponents : new List<BuildComponent>()))
                .ForMember(dest => dest.TotalAssemblyCost, opt => opt.MapFrom(src => src.TotalAssemblyCost))
                .ForMember(dest => dest.CraftsmanCommission, opt => opt.MapFrom(src => src.CraftsmanCommission))
                .ForMember(dest => dest.QCCommission, opt => opt.MapFrom(src => src.QCCommission))
                .ForMember(dest => dest.EvidenceImages, opt => opt.MapFrom(src => src.EvidenceImages));

            CreateMap<CreateProductionJobRequest, ProductionJob>()
                .ForMember(dest => dest.TechnicianId, opt => opt.MapFrom(src => src.AssignedUser));

            CreateMap<UpdateProductionJobRequest, ProductionJob>()
                .ForMember(dest => dest.TechnicianId, opt => opt.MapFrom(src => src.AssignedUser))
                .ForMember(dest => dest.FinishedAt, opt => opt.MapFrom(src => src.CompletedAt));
        }
    }
}
