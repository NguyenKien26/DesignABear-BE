using DesignABear.Services.DTOs.Requests;
using DesignABear.Services.DTOs.Responses;
using DesignABear.Repositories.Paging;

namespace DesignABear.Services.Services.Interfaces
{
    public interface IOrderService
    {
        Task<PagedResult<OrderResponse>> GetOrdersAsync(int pageIndex, int pageSize, int? roleId = null);
        Task<OrderResponse?> GetByIdAsync(Guid orderId);
        Task<OrderResponse?> GetByOrderNumberAsync(string orderNumber);
        Task<OrderResponse?> GetWithDetailsAsync(Guid orderId);
        Task<IEnumerable<OrderResponse>> GetByUserIdAsync(Guid userId);
        Task<IEnumerable<OrderResponse>> GetByStatusAsync(string status);
        Task<OrderResponse> CreateFromCartAsync(Guid cartId, CreateOrderRequest request);
        Task UpdateStatusAsync(Guid orderId, UpdateOrderRequest request, Guid? updatedBy = null);
        Task ApplyPromotionAsync(Guid orderId, string promoCode);
        Task CancelAsync(Guid orderId, Guid? updatedBy = null);
        Task ReorderAsync(Guid orderId, Guid userId);
        Task ValidateOrderStockAsync(Guid orderId);
    }
}
