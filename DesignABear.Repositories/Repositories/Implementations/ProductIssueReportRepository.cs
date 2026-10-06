using Microsoft.EntityFrameworkCore;
using DesignABear.Repositories.Data;
using DesignABear.Repositories.Models;
using DesignABear.Repositories.Repositories.Interfaces;
using DesignABear.Repositories.Paging;

namespace DesignABear.Repositories.Repositories.Implementations
{
    public class ProductIssueReportRepository : GenericRepository<ProductIssueReport>, IProductIssueReportRepository
    {
        public ProductIssueReportRepository(AppDbContext context) : base(context) { }

        public async Task<PagedResult<ProductIssueReport>> GetPagedReportsAsync(int pageIndex, int pageSize, string? status = null, Guid? userId = null, Guid? productId = null)
        {
            IQueryable<ProductIssueReport> query = _dbSet
                .Include(r => r.User)
                .Include(r => r.Product)
                .Include(r => r.AssignedStaff)
                .Include(r => r.OrderItem);

            if (!string.IsNullOrEmpty(status))
                query = query.Where(r => r.Status == status);

            if (userId.HasValue)
                query = query.Where(r => r.UserId == userId.Value);

            if (productId.HasValue)
                query = query.Where(r => r.ProductId == productId.Value);

            var totalCount = await query.CountAsync();
            var items = await query
                .OrderByDescending(r => r.CreatedAt)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<ProductIssueReport>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = totalCount,
                Items = items
            };
        }
    }
}
