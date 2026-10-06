using DesignABear.Services.DTOs.Requests;
using DesignABear.Services.DTOs.Responses;

namespace DesignABear.Services.Services.Interfaces
{
    public interface IFulfillmentService
    {
        Task<IEnumerable<FulfillmentResponse>> GetByOrderIdAsync(Guid orderId);
        Task<FulfillmentResponse?> GetByIdAsync(Guid fulfillmentId);
        Task<FulfillmentResponse> CreateAsync(CreateFulfillmentRequest request);
        Task UpdateAsync(Guid fulfillmentId, UpdateFulfillmentRequest request);
    }
}
