using DesignABear.Repositories.Models;
using DesignABear.Repositories.Repositories.Interfaces;

namespace DesignABear.Repositories.UnitOfWork
{
    public interface IUnitOfWork : IDisposable, IAsyncDisposable
    {
        IUserRepository Users { get; }
        IRoleRepository Roles { get; }
        IGenericRepository<UserToken> UserTokens { get; }
        IAddressRepository Addresses { get; }
        ICategoryRepository Categories { get; }
        ICollectionRepository Collections { get; }
        ICharacterRepository Characters { get; }
        IProductRepository Products { get; }
        IGenericRepository<ProductMedia> ProductMedias { get; }
        IGenericRepository<ProductCategory> ProductCategories { get; }
        IGenericRepository<ProductCollection> ProductCollections { get; }
        IGenericRepository<ProductCharacter> ProductCharacters { get; }
        IGenericRepository<PersonalizationGroup> PersonalizationGroups { get; }
        IPersonalizationRuleRepository PersonalizationRules { get; }
        IProductComboImageRepository ProductComboImages { get; }
        IBuildRepository Builds { get; }
        IGenericRepository<BuildComponent> BuildComponents { get; }
        IGenericRepository<Location> Locations { get; }
        IInventoryRepository Inventories { get; }
        ICartRepository Carts { get; }
        ICartItemRepository CartItems { get; }
        IOrderRepository Orders { get; }
        IGenericRepository<OrderItem> OrderItems { get; }
        IPromotionRepository Promotions { get; }
        IOrderPromotionRepository OrderPromotions { get; }
        IPaymentRepository Payments { get; }
        IFulfillmentRepository Fulfillments { get; }
        IProductionJobRepository ProductionJobs { get; }
        IReviewRepository Reviews { get; }
        IGenericRepository<ReviewReply> ReviewReplies { get; }
        IGenericRepository<OrderRefund> OrderRefunds { get; }
        IProductIssueReportRepository ProductIssueReports { get; }
        IFavoriteRepository Favorites { get; }
        IAccessoryRepository Accessories { get; }
        IProductAccessoryRepository ProductAccessories { get; }
        IGenericRepository<ProductVariant> ProductVariants { get; }

        Task CreateTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
        Task ExecuteInTransactionAsync(Func<Task> action);
        Task<int> SaveChangesAsync();
        int SaveChangesWithTransaction();
        Task<int> SaveChangesWithTransactionAsync();
    }
}
