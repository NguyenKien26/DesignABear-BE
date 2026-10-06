using DesignABear.Repositories.Models;
using DesignABear.Repositories.Paging;

namespace DesignABear.Repositories.Repositories.Interfaces
{
    public interface IReviewRepository : IGenericRepository<Review>
    {
        Task<PagedResult<Review>> GetPagedByProductIdAsync(Guid productId, int pageIndex, int pageSize);
        Task<PagedResult<Review>> GetPagedAsync(int pageIndex, int pageSize, string? status = null);
        Task<IEnumerable<Review>> GetByProductIdAsync(Guid productId);
        Task<IEnumerable<Review>> GetByUserIdAsync(Guid userId);
        Task<IEnumerable<Review>> GetByStatusAsync(string status);
        Task<double> GetAverageRatingAsync(Guid productId);
    }
}
