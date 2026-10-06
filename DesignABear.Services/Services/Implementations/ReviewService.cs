using AutoMapper;
using DesignABear.Repositories.Models;
using DesignABear.Repositories.UnitOfWork;
using DesignABear.Services.DTOs.Requests;
using DesignABear.Services.DTOs.Responses;
using DesignABear.Services.Exceptions;
using DesignABear.Services.Services.Interfaces;
using DesignABear.Repositories.Paging;

namespace DesignABear.Services.Services.Implementations
{
    public class ReviewService : IReviewService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public ReviewService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<PagedResult<ReviewResponse>> GetPagedByProductIdAsync(Guid productId, int pageIndex, int pageSize)
        {
            var pagedReviews = await _uow.Reviews.GetPagedByProductIdAsync(productId, pageIndex, pageSize);
            return new PagedResult<ReviewResponse>
            {
                PageIndex = pagedReviews.PageIndex,
                PageSize = pagedReviews.PageSize,
                TotalCount = pagedReviews.TotalCount,
                Items = _mapper.Map<List<ReviewResponse>>(pagedReviews.Items)
            };
        }

        public async Task<PagedResult<ReviewResponse>> GetPagedAsync(int pageIndex, int pageSize, string? status = null)
        {
            var pagedReviews = await _uow.Reviews.GetPagedAsync(pageIndex, pageSize, status);
            return new PagedResult<ReviewResponse>
            {
                PageIndex = pagedReviews.PageIndex,
                PageSize = pagedReviews.PageSize,
                TotalCount = pagedReviews.TotalCount,
                Items = _mapper.Map<List<ReviewResponse>>(pagedReviews.Items)
            };
        }

        public async Task<IEnumerable<ReviewResponse>> GetByProductIdAsync(Guid productId)
        {
            var reviews = await _uow.Reviews.GetByProductIdAsync(productId);
            // Assuming we want published reviews on the product page
            var published = reviews.Where(r => r.Status == "PUBLISHED");
            return _mapper.Map<IEnumerable<ReviewResponse>>(published);
        }

        public async Task<IEnumerable<ReviewResponse>> GetByUserIdAsync(Guid userId)
        {
            var reviews = await _uow.Reviews.GetByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<ReviewResponse>>(reviews);
        }

        public async Task<IEnumerable<ReviewResponse>> GetPendingAsync()
        {
            var reviews = await _uow.Reviews.GetByStatusAsync("PENDING");
            return _mapper.Map<IEnumerable<ReviewResponse>>(reviews);
        }

        public async Task<ReviewResponse?> GetByIdAsync(Guid reviewId)
        {
            var review = await _uow.Reviews.GetByIdAsync(reviewId);
            if (review == null) throw new NotFoundException("Review", reviewId);
            return _mapper.Map<ReviewResponse>(review);
        }

        public async Task<double> GetAverageRatingAsync(Guid productId)
            => await _uow.Reviews.GetAverageRatingAsync(productId);

        public async Task<ReviewResponse> CreateAsync(CreateReviewRequest request)
        {
            if (request.UserId == null) throw new BadRequestException("UserId is required.");

            if (request.OrderItemId.HasValue)
            {
                var orderItem = await _uow.OrderItems.GetByIdAsync(request.OrderItemId.Value);
                if (orderItem == null) throw new NotFoundException("OrderItem", request.OrderItemId.Value);
            }

            // Check if user has purchased the product
            var canReview = await CanUserReviewAsync(request.ProductId, request.UserId.Value);
            if (!canReview) throw new BadRequestException("You must purchase this product before writing a review.");

            var review = _mapper.Map<Review>(request);
            review.ReviewId = Guid.NewGuid();
            review.Status = "PENDING";
            review.CreatedAt = DateTime.UtcNow;
            
            await _uow.Reviews.AddAsync(review);
            await _uow.SaveChangesAsync();
            
            return _mapper.Map<ReviewResponse>(review);
        }

        public async Task UpdateAsync(Guid reviewId, Guid userId, UpdateReviewRequest request)
        {
            var review = await _uow.Reviews.GetByIdAsync(reviewId);
            if (review == null) throw new NotFoundException("Review", reviewId);

            if (review.UserId != userId) throw new UnauthorizedAccessException("You can only edit your own reviews.");

            // Update allowed fields
            review.Rating = request.Rating;
            review.Title = request.Title;
            review.Body = request.Body;
            //review.Status = "PENDING"; // Re-evaluate after update

            _uow.Reviews.Update(review);
            await _uow.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid reviewId)
        {
            var review = await _uow.Reviews.GetByIdAsync(reviewId);
            if (review != null)
            {
                review.Status = "DELETED";
                _uow.Reviews.Update(review);
                await _uow.SaveChangesAsync();
            }
        }

        public async Task HideAsync(Guid reviewId, Guid userId)
        {
            var review = await _uow.Reviews.GetByIdAsync(reviewId);
            if (review == null) throw new NotFoundException("Review", reviewId);

            if (review.UserId != userId) throw new UnauthorizedAccessException("You can only hide your own reviews.");

            review.Status = "HIDDEN";
            _uow.Reviews.Update(review);
            await _uow.SaveChangesAsync();
        }

        public async Task<bool> CanUserReviewAsync(Guid productId, Guid userId)
        {
            return await _uow.Orders.HasUserPurchasedProductAsync(userId, productId);
        }

        public async Task ApproveAsync(Guid reviewId)
        {
            var review = await _uow.Reviews.GetByIdAsync(reviewId);
            if (review is null) throw new NotFoundException("Review", reviewId);
            
            review.Status = "PUBLISHED";
            _uow.Reviews.Update(review);
            await _uow.SaveChangesAsync();
        }

        public async Task RejectAsync(Guid reviewId)
        {
            var review = await _uow.Reviews.GetByIdAsync(reviewId);
            if (review is null) throw new NotFoundException("Review", reviewId);
            
            review.Status = "REJECTED";
            _uow.Reviews.Update(review);
            await _uow.SaveChangesAsync();
        }

        public async Task<ReviewReplyResponse> AddReplyAsync(Guid reviewId, CreateReviewReplyRequest request)
        {
            var review = await _uow.Reviews.GetByIdAsync(reviewId);
            if (review is null) throw new NotFoundException("Review", reviewId);

            var reply = _mapper.Map<ReviewReply>(request);
            reply.ReplyId = Guid.NewGuid();
            reply.ReviewId = reviewId; // ensure ID matches
            reply.CreatedAt = DateTime.UtcNow;
            
            await _uow.ReviewReplies.AddAsync(reply);
            await _uow.SaveChangesAsync();
            
            return _mapper.Map<ReviewReplyResponse>(reply);
        }

        public async Task<ReviewReplyResponse> UpdateReplyAsync(Guid replyId, UpdateReviewReplyRequest request)
        {
            var reply = await _uow.ReviewReplies.GetByIdAsync(replyId);
            if (reply is null) throw new NotFoundException("ReviewReply", replyId);

            reply.Content = request.Content;
            reply.StaffUserId = request.StaffUserId; // track who updated it
            
            _uow.ReviewReplies.Update(reply);
            await _uow.SaveChangesAsync();
            
            return _mapper.Map<ReviewReplyResponse>(reply);
        }

        public async Task<IEnumerable<ReviewReplyResponse>> GetRepliesByReviewIdAsync(Guid reviewId)
        {
            var replies = await _uow.ReviewReplies.FindAsync(r => r.ReviewId == reviewId);
            return _mapper.Map<IEnumerable<ReviewReplyResponse>>(replies);
        }
    }
}
