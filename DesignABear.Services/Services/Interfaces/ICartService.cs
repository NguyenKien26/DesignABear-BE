using DesignABear.Services.DTOs.Requests;
using DesignABear.Services.DTOs.Responses;

namespace DesignABear.Services.Services.Interfaces
{
    public interface ICartService
    {
        Task<CartResponse?> GetWithItemsAsync(Guid cartId);
        Task<CartResponse?> GetActiveCartByUserAsync(Guid userId);
        Task<CartResponse> CreateCartAsync(CreateCartRequest request);
        Task<CartItemResponse> AddItemAsync(CreateCartItemRequest request);
        Task<CartItemResponse> AddItemToActiveCartAsync(Guid userId, CreateCartItemRequest request);
        Task UpdateItemQuantityAsync(Guid cartItemId, UpdateCartItemRequest request);
        Task RemoveItemAsync(Guid cartItemId);
        Task ClearCartAsync(Guid cartId);
    }
}
