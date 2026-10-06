using Microsoft.EntityFrameworkCore;
using DesignABear.Repositories.Data;
using DesignABear.Repositories.Models;
using DesignABear.Repositories.Repositories.Interfaces;
using DesignABear.Repositories.Paging;

namespace DesignABear.Repositories.Repositories.Implementations
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext context) : base(context) { }

        public async Task<PagedResult<Order>> GetPagedOrdersAsync(int pageIndex, int pageSize, string[]? statuses = null)
        {
            var query = _dbSet.AsQueryable();

            if (statuses != null && statuses.Length > 0)
            {
                query = query.Where(o => statuses.Contains(o.Status));
            }

            var totalCount = await query.CountAsync();
            var items = await query
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Variant)
                    .ThenInclude(v => v!.Product)
                        .ThenInclude(p => p!.Media)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Build)
                    .ThenInclude(b => b!.BaseProduct)
                        .ThenInclude(p => p!.Media)
                .OrderByDescending(o => o.CreatedAt)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Order>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = totalCount,
                Items = items
            };
        }

        public async Task<Order?> GetByOrderNumberAsync(string orderNumber)
            => await _dbSet.FirstOrDefaultAsync(o => o.OrderNumber == orderNumber);

        public async Task<Order?> GetWithDetailsAsync(Guid orderId)
            => await _dbSet
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.ProductionJobs)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Variant)
                        .ThenInclude(p => p.Product)
                        .ThenInclude(p => p.Media)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Build)
                        .ThenInclude(b => b!.BaseProduct)
                        .ThenInclude(p => p!.Media)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Build)
                        .ThenInclude(b => b!.BaseVariant)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Build)
                        .ThenInclude(b => b!.BuildComponents)
                            .ThenInclude(bc => bc.OptionProduct)
                            .ThenInclude(op => op!.Media)
                .Include(o => o.Payments)
                .Include(o => o.Fulfillments)
                .Include(o => o.OrderPromotions)
                    .ThenInclude(op => op.Promotion)
                .Include(o => o.ShippingAddress)
                .Include(o => o.BillingAddress)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

        public async Task<IEnumerable<Order>> GetByUserIdAsync(Guid userId)
            => await _dbSet
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.ProductionJobs)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Variant)
                        .ThenInclude(p => p.Product)
                        .ThenInclude(p => p.Media)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Build)
                        .ThenInclude(b => b!.BaseProduct)
                        .ThenInclude(p => p!.Media)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Build)
                        .ThenInclude(b => b!.BuildComponents)
                            .ThenInclude(bc => bc.OptionProduct)
                                .ThenInclude(op => op!.Media)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Build)
                        .ThenInclude(b => b!.BuildComponents)
                            .ThenInclude(bc => bc.OptionProduct)
                                .ThenInclude(op => op!.Variants)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();

        public async Task<IEnumerable<Order>> GetByStatusAsync(string status)
            => await _dbSet.Where(o => o.Status == status).ToListAsync();

        public async Task<bool> HasUserPurchasedProductAsync(Guid userId, Guid VariantId)
        {
            var validStatuses = new[] { "COMPLETED", "DELIVERED" };
            return await _dbSet
                .Where(o => o.UserId == userId && validStatuses.Contains(o.Status.ToUpper()))
                .SelectMany(o => o.OrderItems)
                .AnyAsync(oi => 
                    (oi.VariantId == VariantId) || 
                    (oi.Build != null && oi.Build.BaseProductId == VariantId));
        }
    }
}
