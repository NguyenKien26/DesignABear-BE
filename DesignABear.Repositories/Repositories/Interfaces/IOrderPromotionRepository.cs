using DesignABear.Repositories.Models;

namespace DesignABear.Repositories.Repositories.Interfaces
{
    public interface IOrderPromotionRepository : IGenericRepository<OrderPromotion>
    {
        Task<int> CountUserUsageAsync(Guid userId, Guid promotionId);
    }
}
