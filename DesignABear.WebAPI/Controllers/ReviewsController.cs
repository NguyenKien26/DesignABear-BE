using DesignABear.Services.Common;
using DesignABear.Services.DTOs.Requests;
using DesignABear.Services.DTOs.Responses;
using DesignABear.Services.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DesignABear.Repositories.Paging;
using System.Security.Claims;

namespace DesignABear.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : BaseController
    {
        private readonly IReviewService _reviewService;

        public ReviewsController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet("product/{productId}")]
        public async Task<IActionResult> GetByProduct(Guid productId, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            var reviews = await _reviewService.GetPagedByProductIdAsync(productId, pageIndex, pageSize);
            return HandleResult(Result<PagedResult<ReviewResponse>>.Success(reviews));
        }

        [HttpGet("all")]
        [Authorize(Roles = "1,2")]
        public async Task<IActionResult> GetAllPaged([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10, [FromQuery] string? status = null)
        {
            var reviews = await _reviewService.GetPagedAsync(pageIndex, pageSize, status);
            return HandleResult(Result<PagedResult<ReviewResponse>>.Success(reviews));
        }

        [HttpGet("product/{productId}/can-review")]
        [Authorize]
        public async Task<IActionResult> CanUserReview(Guid productId)
        {
            var userIdClaim = User.FindFirst("id")?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId)) return Unauthorized();

            var canReview = await _reviewService.CanUserReviewAsync(productId, userId);
            return HandleResult(Result<bool>.Success(canReview));
        }

        [HttpGet("user/{userId}")]
        [Authorize]
        public async Task<IActionResult> GetByUser(Guid userId)
        {
            var reviews = await _reviewService.GetByUserIdAsync(userId);
            return HandleResult(Result<IEnumerable<ReviewResponse>>.Success(reviews));
        }

        [HttpGet("pending")]
        [Authorize(Roles = "1,2")]
        public async Task<IActionResult> GetPending()
        {
            var reviews = await _reviewService.GetPendingAsync();
            return HandleResult(Result<IEnumerable<ReviewResponse>>.Success(reviews));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var review = await _reviewService.GetByIdAsync(id);
            if (review == null)
            {
                return HandleResult(Result<ReviewResponse>.Failure(new Error("NotFound", $"Review with ID {id} not found.")));
            }
            return HandleResult(Result<ReviewResponse>.Success(review));
        }

        [HttpGet("{id}/replies")]
        public async Task<IActionResult> GetReplies(Guid id)
        {
            var replies = await _reviewService.GetRepliesByReviewIdAsync(id);
            return HandleResult(Result<IEnumerable<ReviewReplyResponse>>.Success(replies));
        }

        [HttpGet("product/{productId}/average")]
        public async Task<IActionResult> GetAverageRating(Guid productId)
        {
            var avg = await _reviewService.GetAverageRatingAsync(productId);
            return HandleResult(Result<double>.Success(avg));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateReviewRequest request)
        {
            var userIdClaim = User.FindFirst("id")?.Value;
            if (!string.IsNullOrEmpty(userIdClaim) && Guid.TryParse(userIdClaim, out var userId))
            {
                request.UserId = userId;
            }

            var review = await _reviewService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = review.ReviewId }, Result<ReviewResponse>.Success(review));
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateReviewRequest request)
        {
            var userIdClaim = User.FindFirst("id")?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId)) return Unauthorized();

            await _reviewService.UpdateAsync(id, userId, request);
            return HandleResult(Result.Success());
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "1,2")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _reviewService.DeleteAsync(id);
            return HandleResult(Result.Success());
        }

        [HttpPut("{id}/hide")]
        [Authorize]
        public async Task<IActionResult> Hide(Guid id)
        {
            var userIdClaim = User.FindFirst("id")?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId)) return Unauthorized();

            await _reviewService.HideAsync(id, userId);
            return HandleResult(Result.Success());
        }

        [HttpPut("{id}/approve")]
        [Authorize(Roles = "1,2")]
        public async Task<IActionResult> Approve(Guid id)
        {
            await _reviewService.ApproveAsync(id);
            return HandleResult(Result.Success());
        }

        [HttpPut("{id}/reject")]
        [Authorize(Roles = "1,2")]
        public async Task<IActionResult> Reject(Guid id)
        {
            await _reviewService.RejectAsync(id);
            return HandleResult(Result.Success());
        }

        [HttpPost("{id}/reply")]
        [Authorize(Roles = "1,2")]
        public async Task<IActionResult> AddReply(Guid id, [FromBody] CreateReviewReplyRequest request)
        {
            var reply = await _reviewService.AddReplyAsync(id, request);
            return HandleResult(Result<ReviewReplyResponse>.Success(reply));
        }

        [HttpPut("replies/{replyId}")]
        [Authorize(Roles = "1,2")]
        public async Task<IActionResult> UpdateReply(Guid replyId, [FromBody] UpdateReviewReplyRequest request)
        {
            var userIdClaim = User.FindFirst("id")?.Value;
            if (!string.IsNullOrEmpty(userIdClaim) && Guid.TryParse(userIdClaim, out var userId))
            {
                request.StaffUserId = userId;
            }

            var reply = await _reviewService.UpdateReplyAsync(replyId, request);
            return HandleResult(Result<ReviewReplyResponse>.Success(reply));
        }
    }
}
