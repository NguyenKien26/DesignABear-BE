using Microsoft.EntityFrameworkCore;
using DesignABear.Repositories.Data;
using DesignABear.Repositories.Models;
using DesignABear.Repositories.Repositories.Interfaces;
using DesignABear.Repositories.Paging;

namespace DesignABear.Repositories.Repositories.Implementations
{
    public class ReviewRepository : GenericRepository<Review>, IReviewRepository
    {
        public ReviewRepository(AppDbContext context) : base(context) { }

        public async Task<PagedResult<Review>> GetPagedByProductIdAsync(Guid productId, int pageIndex, int pageSize)
        {
            var query = _dbSet
                .Include(r => r.User)
                .Include(r => r.ReviewReplies)
                .Where(r => r.ProductId == productId && r.Status == "PUBLISHED");

            var totalCount = await query.CountAsync();
            var items = await query
                .OrderByDescending(r => r.CreatedAt)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Review>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = totalCount,
                Items = items
            };
        }

        public async Task<PagedResult<Review>> GetPagedAsync(int pageIndex, int pageSize, string? status = null)
        {
            var query = _dbSet.Include(r => r.User).AsQueryable();

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(r => r.Status == status);
            }

            var totalCount = await query.CountAsync();
            var items = await query
                .OrderByDescending(r => r.CreatedAt)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Review>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = totalCount,
                Items = items
            };
        }

        public async Task<IEnumerable<Review>> GetByProductIdAsync(Guid productId)
            => await _dbSet
                .Include(r => r.ReviewReplies)
                .Where(r => r.ProductId == productId && r.Status == "PUBLISHED")
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

        public async Task<IEnumerable<Review>> GetByUserIdAsync(Guid userId)
            => await _dbSet.Where(r => r.UserId == userId).ToListAsync();

        public async Task<IEnumerable<Review>> GetByStatusAsync(string status)
            => await _dbSet.Where(r => r.Status == status).ToListAsync();

        public async Task<double> GetAverageRatingAsync(Guid productId)
        {
            var ratings = await _dbSet
                .Where(r => r.ProductId == productId && r.Status == "PUBLISHED")
                .Select(r => r.Rating)
                .ToListAsync();
            return ratings.Count == 0 ? 0 : ratings.Average();
        }
    }
}
