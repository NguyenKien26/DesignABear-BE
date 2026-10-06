using DesignABear.Services.DTOs.Requests;
using DesignABear.Services.DTOs.Responses;
using DesignABear.Repositories.Paging;

namespace DesignABear.Services.Services.Interfaces
{
    public interface IReviewService
    {
        Task<PagedResult<ReviewResponse>> GetPagedByProductIdAsync(Guid productId, int pageIndex, int pageSize);
        Task<PagedResult<ReviewResponse>> GetPagedAsync(int pageIndex, int pageSize, string? status = null);
        Task<IEnumerable<ReviewResponse>> GetByProductIdAsync(Guid productId);
        Task<IEnumerable<ReviewResponse>> GetByUserIdAsync(Guid userId);
        Task<IEnumerable<ReviewResponse>> GetPendingAsync();
        Task<ReviewResponse?> GetByIdAsync(Guid reviewId);
        Task<double> GetAverageRatingAsync(Guid productId);
        Task<ReviewResponse> CreateAsync(CreateReviewRequest request);
        Task UpdateAsync(Guid reviewId, Guid userId, UpdateReviewRequest request);
        Task DeleteAsync(Guid reviewId);
        Task HideAsync(Guid reviewId, Guid userId);
        Task<bool> CanUserReviewAsync(Guid productId, Guid userId);
        Task ApproveAsync(Guid reviewId);
        Task RejectAsync(Guid reviewId);
        Task<ReviewReplyResponse> AddReplyAsync(Guid reviewId, CreateReviewReplyRequest request);
        Task<ReviewReplyResponse> UpdateReplyAsync(Guid replyId, UpdateReviewReplyRequest request);
        Task<IEnumerable<ReviewReplyResponse>> GetRepliesByReviewIdAsync(Guid reviewId);
    }
}
