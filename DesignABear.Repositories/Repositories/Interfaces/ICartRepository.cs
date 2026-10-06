using DesignABear.Repositories.Models;

namespace DesignABear.Repositories.Repositories.Interfaces
{
    public interface ICartRepository : IGenericRepository<Cart>
    {
        Task<Cart?> GetWithItemsAsync(Guid cartId);
        Task<Cart?> GetActiveCartByUserAsync(Guid userId);
        Task UpdateTimestampAsync(Guid cartId);
    }
}
