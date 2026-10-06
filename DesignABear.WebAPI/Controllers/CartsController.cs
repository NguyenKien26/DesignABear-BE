using DesignABear.Services.Common;
using DesignABear.Services.DTOs.Requests;
using DesignABear.Services.DTOs.Responses;
using DesignABear.Services.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DesignABear.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartsController : BaseController
    {
        private readonly ICartService _cartService;

        public CartsController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var cart = await _cartService.GetWithItemsAsync(id);
            if (cart == null)
            {
                return HandleResult(Result<CartResponse>.Failure(new Error("NotFound", $"Cart with ID {id} not found.")));
            }
            return HandleResult(Result<CartResponse>.Success(cart));
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetActiveCartByUser(Guid userId)
        {
            var cart = await _cartService.GetActiveCartByUserAsync(userId);
            return HandleResult(Result<CartResponse?>.Success(cart));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCartRequest request)
        {
            var newCart = await _cartService.CreateCartAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = newCart.CartId }, Result<CartResponse>.Success(newCart));
        }

        [HttpPost("items")]
        public async Task<IActionResult> AddItem([FromBody] CreateCartItemRequest request)
        {
            var userIdClaim = User.FindFirst("id")?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                return HandleResult(Result<CartItemResponse>.Failure(new Error("Unauthorized", "User ID not found in token.")));
            }

            var item = await _cartService.AddItemToActiveCartAsync(userId, request);
            return HandleResult(Result<CartItemResponse>.Success(item));
        }

        [HttpPut("items/{itemId}")]
        public async Task<IActionResult> UpdateItemQuantity(Guid itemId, [FromBody] UpdateCartItemRequest request)
        {
            await _cartService.UpdateItemQuantityAsync(itemId, request);
            return HandleResult(Result.Success());
        }

        [HttpDelete("items/{itemId}")]
        public async Task<IActionResult> RemoveItem(Guid itemId)
        {
            await _cartService.RemoveItemAsync(itemId);
            return HandleResult(Result.Success());
        }

        [HttpDelete("{id}/clear")]
        public async Task<IActionResult> ClearCart(Guid id)
        {
            await _cartService.ClearCartAsync(id);
            return HandleResult(Result.Success());
        }
    }
}
