using DesignABear.Repositories.Data;
using DesignABear.Repositories.Models;
using DesignABear.Repositories.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DesignABear.Repositories.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly IServiceProvider _serviceProvider;
        private bool _disposed = false;

        private IUserRepository? _users;
        private IRoleRepository? _roles;
        private IGenericRepository<UserToken>? _userTokens;
        private IAddressRepository? _addresses;
        private ICategoryRepository? _categories;
        private ICollectionRepository? _collections;
        private ICharacterRepository? _characters;
        private IProductRepository? _products;
        private IGenericRepository<ProductMedia>? _productMedias;
        private IGenericRepository<ProductCategory>? _productCategories;
        private IGenericRepository<ProductCollection>? _productCollections;
        private IGenericRepository<ProductCharacter>? _productCharacters;
        private IGenericRepository<PersonalizationGroup>? _personalizationGroups;
        private IPersonalizationRuleRepository? _personalizationRules;
        private IProductComboImageRepository? _productComboImages;
        private IBuildRepository? _builds;
        private IGenericRepository<BuildComponent>? _buildComponents;
        private IGenericRepository<Location>? _locations;
        private IInventoryRepository? _inventories;
        private ICartRepository? _carts;
        private ICartItemRepository? _cartItems;
        private IOrderRepository? _orders;
        private IGenericRepository<OrderItem>? _orderItems;
        private IPromotionRepository? _promotions;
        private IOrderPromotionRepository? _orderPromotions;
        private IPaymentRepository? _payments;
        private IFulfillmentRepository? _fulfillments;
        private IProductionJobRepository? _productionJobs;
        private IReviewRepository? _reviews;
        private IGenericRepository<ReviewReply>? _reviewReplies;
        private IGenericRepository<OrderRefund>? _orderRefunds;
        private IProductIssueReportRepository? _productIssueReports;
        private IFavoriteRepository? _favorites;
        private IAccessoryRepository? _accessories;
        private IProductAccessoryRepository? _productAccessories;
        private IGenericRepository<ProductVariant>? _productVariants;

        public UnitOfWork(AppDbContext context, IServiceProvider serviceProvider)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public IUserRepository Users => _users ??= _serviceProvider.GetRequiredService<IUserRepository>();
        public IRoleRepository Roles => _roles ??= _serviceProvider.GetRequiredService<IRoleRepository>();
        public IGenericRepository<UserToken> UserTokens => _userTokens ??= _serviceProvider.GetRequiredService<IGenericRepository<UserToken>>();
        public IAddressRepository Addresses => _addresses ??= _serviceProvider.GetRequiredService<IAddressRepository>();
        public ICategoryRepository Categories => _categories ??= _serviceProvider.GetRequiredService<ICategoryRepository>();
        public ICollectionRepository Collections => _collections ??= _serviceProvider.GetRequiredService<ICollectionRepository>();
        public ICharacterRepository Characters => _characters ??= _serviceProvider.GetRequiredService<ICharacterRepository>();
        public IProductRepository Products => _products ??= _serviceProvider.GetRequiredService<IProductRepository>();
        public IGenericRepository<ProductMedia> ProductMedias => _productMedias ??= _serviceProvider.GetRequiredService<IGenericRepository<ProductMedia>>();
        public IGenericRepository<ProductCategory> ProductCategories => _productCategories ??= _serviceProvider.GetRequiredService<IGenericRepository<ProductCategory>>();
        public IGenericRepository<ProductCollection> ProductCollections => _productCollections ??= _serviceProvider.GetRequiredService<IGenericRepository<ProductCollection>>();
        public IGenericRepository<ProductCharacter> ProductCharacters => _productCharacters ??= _serviceProvider.GetRequiredService<IGenericRepository<ProductCharacter>>();
        public IGenericRepository<PersonalizationGroup> PersonalizationGroups => _personalizationGroups ??= _serviceProvider.GetRequiredService<IGenericRepository<PersonalizationGroup>>();
        public IPersonalizationRuleRepository PersonalizationRules => _personalizationRules ??= _serviceProvider.GetRequiredService<IPersonalizationRuleRepository>();
        public IProductComboImageRepository ProductComboImages => _productComboImages ??= _serviceProvider.GetRequiredService<IProductComboImageRepository>();
        public IBuildRepository Builds => _builds ??= _serviceProvider.GetRequiredService<IBuildRepository>();
        public IGenericRepository<BuildComponent> BuildComponents => _buildComponents ??= _serviceProvider.GetRequiredService<IGenericRepository<BuildComponent>>();
        public IGenericRepository<Location> Locations => _locations ??= _serviceProvider.GetRequiredService<IGenericRepository<Location>>();
        public IInventoryRepository Inventories => _inventories ??= _serviceProvider.GetRequiredService<IInventoryRepository>();
        public ICartRepository Carts => _carts ??= _serviceProvider.GetRequiredService<ICartRepository>();
        public ICartItemRepository CartItems => _cartItems ??= _serviceProvider.GetRequiredService<ICartItemRepository>();
        public IOrderRepository Orders => _orders ??= _serviceProvider.GetRequiredService<IOrderRepository>();
        public IGenericRepository<OrderItem> OrderItems => _orderItems ??= _serviceProvider.GetRequiredService<IGenericRepository<OrderItem>>();
        public IPromotionRepository Promotions => _promotions ??= _serviceProvider.GetRequiredService<IPromotionRepository>();
        public IOrderPromotionRepository OrderPromotions => _orderPromotions ??= _serviceProvider.GetRequiredService<IOrderPromotionRepository>();
        public IPaymentRepository Payments => _payments ??= _serviceProvider.GetRequiredService<IPaymentRepository>();
        public IFulfillmentRepository Fulfillments => _fulfillments ??= _serviceProvider.GetRequiredService<IFulfillmentRepository>();
        public IProductionJobRepository ProductionJobs => _productionJobs ??= _serviceProvider.GetRequiredService<IProductionJobRepository>();
        public IReviewRepository Reviews => _reviews ??= _serviceProvider.GetRequiredService<IReviewRepository>();
        public IGenericRepository<ReviewReply> ReviewReplies => _reviewReplies ??= _serviceProvider.GetRequiredService<IGenericRepository<ReviewReply>>();
        public IGenericRepository<OrderRefund> OrderRefunds => _orderRefunds ??= _serviceProvider.GetRequiredService<IGenericRepository<OrderRefund>>();
        public IProductIssueReportRepository ProductIssueReports => _productIssueReports ??= _serviceProvider.GetRequiredService<IProductIssueReportRepository>();
        public IFavoriteRepository Favorites => _favorites ??= _serviceProvider.GetRequiredService<IFavoriteRepository>();
        public IAccessoryRepository Accessories => _accessories ??= _serviceProvider.GetRequiredService<IAccessoryRepository>();
        public IProductAccessoryRepository ProductAccessories => _productAccessories ??= _serviceProvider.GetRequiredService<IProductAccessoryRepository>();
        public IGenericRepository<ProductVariant> ProductVariants => _productVariants ??= _serviceProvider.GetRequiredService<IGenericRepository<ProductVariant>>();

        public async Task CreateTransactionAsync() 
            => await _context.Database.BeginTransactionAsync();

        public async Task CommitAsync() 
            => await _context.Database.CommitTransactionAsync();

        public async Task RollbackAsync() 
            => await _context.Database.RollbackTransactionAsync();

        public async Task ExecuteInTransactionAsync(Func<Task> action)
        {
            if (_context.Database.ProviderName == "Microsoft.EntityFrameworkCore.InMemory")
            {
                await action();
                return;
            }

            // Check if there's already an active transaction
            if (_context.Database.CurrentTransaction != null)
            {
                // Already in a transaction, just join it by executing the action
                await action();
                return;
            }

            var strategy = _context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    await action();
                    await transaction.CommitAsync();
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            });
        }

        public async Task<int> SaveChangesAsync() 
        {
            try 
            {
                return await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex) when (ex.InnerException is Npgsql.PostgresException pex && pex.SqlState == "23505")
            {
                throw new Exception("Mã SKU đã tồn tại trong hệ thống. Vui lòng kiểm tra lại bảng Sản phẩm hoặc Phụ kiện.");
            }
        }

        public int SaveChangesWithTransaction()
        {
            if (_context.Database.ProviderName == "Microsoft.EntityFrameworkCore.InMemory")
            {
                return _context.SaveChanges();
            }

            var strategy = _context.Database.CreateExecutionStrategy();
            int result = 0;
            strategy.Execute(() =>
            {
                using var transaction = _context.Database.BeginTransaction();
                try
                {
                    result = _context.SaveChanges();
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            });
            return result;
        }

        public async Task<int> SaveChangesWithTransactionAsync()
        {
            if (_context.Database.ProviderName == "Microsoft.EntityFrameworkCore.InMemory")
            {
                return await _context.SaveChangesAsync();
            }

            var strategy = _context.Database.CreateExecutionStrategy();
            return await strategy.ExecuteAsync(async () =>
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    var result = await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return result;
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    // Handle stale data: for each conflicting entry, reload from DB
                    // Entries marked as Deleted that no longer exist are simply detached
                    foreach (var entry in ex.Entries)
                    {
                        if (entry.State == EntityState.Deleted || entry.State == EntityState.Modified)
                        {
                            // Reload from DB; if row is gone, this detaches the entity
                            await entry.ReloadAsync();
                        }
                    }
                    
                    // Retry save after resolving conflicts
                    try
                    {
                        var result = await _context.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return result;
                    }
                    catch (Exception retryEx)
                    {
                        await transaction.RollbackAsync();
                        var entities = string.Join(", ", ex.Entries.Select(e => e.Entity.GetType().Name));
                        throw new Exception($"Concurrency failure after retry. Affected entities: {entities}. Details: {retryEx.Message}", retryEx);
                    }
                }
                catch (DbUpdateException ex) when (ex.InnerException is Npgsql.PostgresException pex && pex.SqlState == "23505")
                {
                    await transaction.RollbackAsync();
                    throw new Exception("Mã SKU đã tồn tại trong hệ thống. Giao dịch đã bị hủy bỏ để bảo vệ tính toàn vẹn dữ liệu.");
                }
                catch (Exception)
                {
                    if (transaction != null) await transaction.RollbackAsync();
                    throw;
                }
            });
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async ValueTask DisposeAsync()
        {
            if (!_disposed)
            {
                await _context.DisposeAsync();
                _disposed = true;
                GC.SuppressFinalize(this);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                _context?.Dispose();
            }

            _disposed = true;
        }

        ~UnitOfWork()
        {
            Dispose(false);
        }
    }
}
