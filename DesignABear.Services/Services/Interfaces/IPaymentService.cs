using DesignABear.Services.DTOs.Requests;
using DesignABear.Services.DTOs.Responses;

namespace DesignABear.Services.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<IEnumerable<PaymentResponse>> GetByOrderIdAsync(Guid orderId);
        Task<PaymentResponse?> GetByIdAsync(Guid paymentId);
        Task<PaymentResponse?> GetByExternalTxnIdAsync(string externalTxnId);
        Task<PaymentResponse> CreateAsync(CreatePaymentRequest request);
        Task UpdateAsync(Guid paymentId, UpdatePaymentRequest request);
        Task HandleWebhookAsync(string externalTxnId, string rawPayload, string? statusOverride = null);
    }
}
