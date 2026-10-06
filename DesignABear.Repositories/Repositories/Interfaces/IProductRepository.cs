using DesignABear.Repositories.Models;
using DesignABear.Repositories.Paging;

namespace DesignABear.Repositories.Repositories.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<PagedResult<Product>> GetPagedProductsAsync(int pageIndex, int pageSize, string? sortBy = null, bool onlyActive = false);
        Task IncrementViewCountAsync(Guid productId);
        Task BulkIncrementViewCountsAsync(Dictionary<Guid, long> viewCounts);
        Task<Product?> GetBySlugAsync(string slug);
        Task<Product?> GetWithDetailsAsync(Guid productId);
        Task<Product?> GetFullDetailsAsync(Guid productId);
        Task<IEnumerable<Product>> GetByCategoryAsync(Guid categoryId);
        Task<IEnumerable<Product>> GetByCollectionAsync(Guid collectionId);
        Task<IEnumerable<Product>> GetByCharacterAsync(Guid characterId);
        Task<IEnumerable<Product>> GetActiveProductsAsync();
        Task<int> ExecuteRawSqlAsync(string sql, params object[] parameters);
    }
}
