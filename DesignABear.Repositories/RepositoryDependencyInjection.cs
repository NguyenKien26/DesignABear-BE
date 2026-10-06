using DesignABear.Repositories.Repositories.Implementations;
using DesignABear.Repositories.Repositories.Interfaces;
using DesignABear.Repositories.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class RepositoryDependencyInjection
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IAddressRepository, AddressRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICollectionRepository, CollectionRepository>();
            services.AddScoped<ICharacterRepository, CharacterRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IPersonalizationRuleRepository, PersonalizationRuleRepository>();
            services.AddScoped<IProductComboImageRepository, ProductComboImageRepository>();
            services.AddScoped<IBuildRepository, BuildRepository>();
            services.AddScoped<IInventoryRepository, InventoryRepository>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<ICartItemRepository, CartItemRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IPromotionRepository, PromotionRepository>();
            services.AddScoped<IOrderPromotionRepository, OrderPromotionRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IFulfillmentRepository, FulfillmentRepository>();
            services.AddScoped<IProductionJobRepository, ProductionJobRepository>();
            services.AddScoped<IReviewRepository, ReviewRepository>();
            services.AddScoped<IProductIssueReportRepository, ProductIssueReportRepository>();
            services.AddScoped<IFavoriteRepository, FavoriteRepository>();
            services.AddScoped<IAccessoryRepository, AccessoryRepository>();
            services.AddScoped<IProductAccessoryRepository, ProductAccessoryRepository>();

            // Register Generic Repositories
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            return services;
        }
    }
}
