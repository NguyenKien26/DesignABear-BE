using DesignABear.Services.DTOs.Requests;
using DesignABear.Services.DTOs.Responses;

namespace DesignABear.Services.Services.Interfaces
{
    public interface IPromotionService
    {
        Task<IEnumerable<PromotionResponse>> GetAllAsync();
        Task<IEnumerable<PromotionResponse>> GetActivePromotionsAsync();
        Task<PromotionResponse?> GetByIdAsync(Guid promotionId);
        Task<PromotionResponse?> GetByCodeAsync(string code);
        Task<PromotionResponse> CreateAsync(CreatePromotionRequest request);
        Task<PromotionResponse> UpdateAsync(Guid promotionId, UpdatePromotionRequest request);
        Task DeleteAsync(Guid promotionId);
        Task<bool> ValidateAsync(string code, Guid? userId = null, decimal? orderAmount = null, decimal? shippingAmount = null);
        Task<PromotionApplyResponse> ApplyPromotionAsync(string code, Guid userId, decimal orderAmount, decimal shippingAmount);
        Task UsePromotionAsync(string code, Guid userId, Guid orderId, decimal discountAmount);
    }
}
