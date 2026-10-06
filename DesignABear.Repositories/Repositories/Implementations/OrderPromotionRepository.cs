using DesignABear.Repositories.Data;
using DesignABear.Repositories.Models;
using DesignABear.Repositories.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DesignABear.Repositories.Repositories.Implementations
{
    public class OrderPromotionRepository : GenericRepository<OrderPromotion>, IOrderPromotionRepository
    {
        public OrderPromotionRepository(AppDbContext context) : base(context) { }

        public async Task<int> CountUserUsageAsync(Guid userId, Guid promotionId)
        {
            return await _dbSet
                .CountAsync(op => op.PromotionId == promotionId && op.Order.UserId == userId);
        }
    }
}
