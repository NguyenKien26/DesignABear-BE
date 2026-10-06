using DesignABear.Services.DTOs.Requests;
using DesignABear.Services.DTOs.Responses;
using DesignABear.Services.Services.Interfaces;
using DesignABear.Services.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DesignABear.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RefundsController : BaseController
    {
        private readonly IRefundService _refundService;

        public RefundsController(IRefundService refundService)
        {
            _refundService = refundService;
        }

        [HttpPost]
        [Authorize(Roles = "2, 3")] // Staff and Customers
        public async Task<IActionResult> CreateRefundRequest([FromBody] RefundRequest request)
        {
            var userId = GetCurrentUserId();
            var roleId = GetCurrentRoleId();
            
            var result = await _refundService.CreateRequestAsync(request, userId, roleId);
            return HandleResult(Result<RefundResponse>.Success(result));
        }

        [HttpPost("{id}/approve")]
        [Authorize(Roles = "1")] // Admin only
        public async Task<IActionResult> ApproveRefund(Guid id, [FromBody] RefundDecisionRequest request)
        {
            var userId = GetCurrentUserId();
            var result = await _refundService.ApproveAsync(id, userId, request.AdminNotes);
            return HandleResult(Result<RefundResponse>.Success(result));
        }

        [HttpPost("{id}/reject")]
        [Authorize(Roles = "1")] // Admin only
        public async Task<IActionResult> RejectRefund(Guid id, [FromBody] RefundDecisionRequest request)
        {
            var userId = GetCurrentUserId();
            var result = await _refundService.RejectAsync(id, userId, request.AdminNotes);
            return HandleResult(Result<RefundResponse>.Success(result));
        }

        [HttpGet]
        [Authorize(Roles = "1")] // Admin only
        public async Task<IActionResult> GetAllPending()
        {
            var results = await _refundService.GetAllPendingAsync();
            return HandleResult(Result<IEnumerable<RefundResponse>>.Success(results));
        }

        [HttpGet("order/{orderId}")]
        [Authorize] 
        public async Task<IActionResult> GetByOrder(Guid orderId)
        {
            var results = await _refundService.GetRefundsByOrderAsync(orderId);
            return HandleResult(Result<IEnumerable<RefundResponse>>.Success(results));
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "1, 2")] // Admin and Staff
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _refundService.GetByIdAsync(id);
            if (result == null) return NotFound();
            return HandleResult(Result<RefundResponse>.Success(result));
        }
    }
}
