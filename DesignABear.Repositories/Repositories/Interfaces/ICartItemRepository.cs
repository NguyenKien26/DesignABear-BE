using DesignABear.Repositories.Models;

namespace DesignABear.Repositories.Repositories.Interfaces
{
    public interface ICartItemRepository : IGenericRepository<CartItem>
    {
        Task<CartItem?> GetWithEnrichmentAsync(Guid cartItemId);
    }
}
