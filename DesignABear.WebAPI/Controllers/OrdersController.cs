using DesignABear.Services.Common;
using DesignABear.Services.DTOs.Requests;
using DesignABear.Services.DTOs.Responses;
using DesignABear.Services.Services.Interfaces;
using DesignABear.Repositories.Paging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DesignABear.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : BaseController
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        [Authorize(Roles = "1, 2")]
        public async Task<IActionResult> GetAll([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            var roleId = GetCurrentRoleId();

            var orders = await _orderService.GetOrdersAsync(pageIndex, pageSize, roleId);
            return HandleResult(Result<PagedResult<OrderResponse>>.Success(orders));
        }

        /// <summary>
        /// Retrieves an order with full details by ID.
        /// </summary>
        /// <param name="id">The order ID.</param>
        /// <returns>The order detail response.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var order = await _orderService.GetWithDetailsAsync(id);
            if (order == null)
            {
                return HandleResult(Result<OrderResponse>.Failure(new Error("NotFound", $"Order with ID {id} not found.")));
            }
            return HandleResult(Result<OrderResponse>.Success(order));
        }

        [HttpGet("number/{orderNumber}")]
        public async Task<IActionResult> GetByOrderNumber(string orderNumber)
        {
            var order = await _orderService.GetByOrderNumberAsync(orderNumber);
            if (order == null)
            {
                return HandleResult(Result<OrderResponse>.Failure(new Error("NotFound", $"Order with number {orderNumber} not found.")));
            }
            return HandleResult(Result<OrderResponse>.Success(order));
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUser(Guid userId)
        {
            var orders = await _orderService.GetByUserIdAsync(userId);
            return HandleResult(Result<IEnumerable<OrderResponse>>.Success(orders));
        }

        [HttpGet("status/{status}")]
        [Authorize(Roles = "1, 2")]
        public async Task<IActionResult> GetByStatus(string status)
        {
            var orders = await _orderService.GetByStatusAsync(status);
            return HandleResult(Result<IEnumerable<OrderResponse>>.Success(orders));
        }

        /// <summary>
        /// Creates a new order from an existing cart.
        /// </summary>
        /// <param name="cartId">The source cart ID.</param>
        /// <param name="request">The order creation details (addresses, notes).</param>
        /// <returns>The newly created order.</returns>
        [HttpPost("from-cart/{cartId}")]
        [SwaggerOperation(Summary = "Place an order from cart")]
        public async Task<IActionResult> CreateFromCart(Guid cartId, [FromBody] CreateOrderRequest request)
        {
            var newOrder = await _orderService.CreateFromCartAsync(cartId, request);
            return CreatedAtAction(nameof(GetById), new { id = newOrder.OrderId }, Result<OrderResponse>.Success(newOrder));
        }

        [HttpPut("{id}/status")]
        [Authorize(Roles = "1, 2, 3")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateOrderRequest request)
        {
            await _orderService.UpdateStatusAsync(id, request);
            return HandleResult(Result.Success());
        }

        /// <summary>
        /// Applies a promotional discount code to an existing order.
        /// </summary>
        /// <param name="id">The unique identifier (GUID) of the order to which the promotion will be applied.</param>
        /// <param name="promoCode">The promotional code string provided in the request body.</param>
        /// <returns>The updated order details including calculated discounts and the new grand total.</returns>
        /// <response code="200">Returns the updated order if the promotion was successfully applied.</response>
        /// <response code="400">If the promo code is invalid, expired, or doesn't meet order requirements.</response>
        /// <response code="404">If the order with the specified ID is not found.</response>
        [HttpPost("{id}/promotion")]
        public async Task<IActionResult> ApplyPromotion(Guid id, [FromBody] string promoCode)
        {
            await _orderService.ApplyPromotionAsync(id, promoCode);
            var updatedOrder = await _orderService.GetWithDetailsAsync(id);
            return HandleResult(Result<OrderResponse>.Success(updatedOrder));
        }

        [HttpPost("{id}/cancel")]
        public async Task<IActionResult> Cancel(Guid id)
        {
            var userId = GetCurrentUserId();
            await _orderService.CancelAsync(id, userId);
            return HandleResult(Result.Success());
        }

        [HttpPost("{id}/reorder")]
        public async Task<IActionResult> Reorder(Guid id)
        {
            var userId = GetCurrentUserId();
            await _orderService.ReorderAsync(id, userId);
            return HandleResult(Result.Success());
        }
    }
}
