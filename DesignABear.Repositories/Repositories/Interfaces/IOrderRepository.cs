using DesignABear.Repositories.Models;
using DesignABear.Repositories.Paging;

namespace DesignABear.Repositories.Repositories.Interfaces
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<PagedResult<Order>> GetPagedOrdersAsync(int pageIndex, int pageSize, string[]? statuses = null);
        Task<Order?> GetByOrderNumberAsync(string orderNumber);
        Task<Order?> GetWithDetailsAsync(Guid orderId);
        Task<IEnumerable<Order>> GetByUserIdAsync(Guid userId);
        Task<IEnumerable<Order>> GetByStatusAsync(string status);
        Task<bool> HasUserPurchasedProductAsync(Guid userId, Guid productId);
    }
}
