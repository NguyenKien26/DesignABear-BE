using DesignABear.Repositories.Data;
using DesignABear.Repositories.Models;
using DesignABear.Repositories.Repositories.Interfaces;
using DesignABear.Repositories.Paging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DesignABear.Repositories.Repositories.Implementations
{
    public class FavoriteRepository : GenericRepository<Favorite>, IFavoriteRepository
    {
        public FavoriteRepository(AppDbContext context) : base(context) { }

        public async Task<PagedResult<Favorite>> GetPagedByUserAsync(Guid userId, int pageIndex, int pageSize)
        {
            var query = _dbSet
                .Include(f => f.Product)
                    .ThenInclude(p => p.Media)
                .Include(f => f.Product)
                    .ThenInclude(p => p.Variants)
                .Where(f => f.UserId == userId)
                .OrderByDescending(f => f.CreatedAt);

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Favorite>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = totalCount,
                Items = items
            };
        }

        public async Task<Favorite?> GetAsync(Guid userId, Guid productId)
        {
            return await _dbSet
                .Include(f => f.Product)
                    .ThenInclude(p => p.Media)
                .Include(f => f.Product)
                    .ThenInclude(p => p.Variants)
                .FirstOrDefaultAsync(f => 
                    f.UserId == userId && 
                    f.ProductId == productId);
        }
    }
}
