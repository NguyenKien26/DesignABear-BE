using DesignABear.Services.DTOs.Requests;
using DesignABear.Services.DTOs.Responses;

namespace DesignABear.Services.Services.Interfaces
{
    public interface IRefundService
    {
        Task<RefundResponse> CreateRequestAsync(RefundRequest request, Guid userId, int roleId);
        Task<RefundResponse> ApproveAsync(Guid refundId, Guid userId, string? adminNotes);
        Task<RefundResponse> RejectAsync(Guid refundId, Guid userId, string? adminNotes);
        Task<IEnumerable<RefundResponse>> GetAllPendingAsync();
        Task<IEnumerable<RefundResponse>> GetRefundsByOrderAsync(Guid orderId);
        Task<RefundResponse?> GetByIdAsync(Guid refundId);
    }
}
