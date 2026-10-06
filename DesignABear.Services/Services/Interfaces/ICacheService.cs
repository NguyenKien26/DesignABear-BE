using System;
using System.Threading.Tasks;

namespace DesignABear.Services.Services.Interfaces
{
    public interface ICacheService
    {
        Task<T?> GetAsync<T>(string key);
        Task SetAsync<T>(string key, T value, TimeSpan? expiration = null);
        Task RemoveAsync(string key);
        Task RemoveByPrefixAsync(string prefix);
        
        // Startup operations
        Task FlushAllCachedDataAsync();

        // View Count operations
        Task IncrementViewCountAsync(Guid productId);
        Task<long> GetViewCountAsync(Guid productId);
        Task<Dictionary<Guid, long>> GetAllViewCountsAsync();
        Task ClearViewCountAsync(Guid productId);

        // Trending operations
        Task TrackTrendingCategoryAsync(string categorySlug);
    }
}
