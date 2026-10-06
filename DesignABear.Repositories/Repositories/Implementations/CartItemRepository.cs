using Microsoft.EntityFrameworkCore;
using DesignABear.Repositories.Data;
using DesignABear.Repositories.Models;
using DesignABear.Repositories.Repositories.Interfaces;

namespace DesignABear.Repositories.Repositories.Implementations
{
    public class CartItemRepository : GenericRepository<CartItem>, ICartItemRepository
    {
        public CartItemRepository(AppDbContext context) : base(context) { }

        public async Task<CartItem?> GetWithEnrichmentAsync(Guid cartItemId)
        {
            return await _dbSet
                .Include(ci => ci.Product)
                    .ThenInclude(p => p.Media)
                .Include(ci => ci.Variant)
                    .ThenInclude(v => v.Product)
                        .ThenInclude(p => p.Media)
                .Include(ci => ci.Build)
                    .ThenInclude(b => b.BaseProduct)
                        .ThenInclude(p => p.Media)
                .Include(ci => ci.Build)
                    .ThenInclude(b => b.BaseVariant)
                .Include(ci => ci.Build)
                    .ThenInclude(b => b.BuildComponents)
                        .ThenInclude(bc => bc.OptionVariant)
                .Include(ci => ci.Build)
                    .ThenInclude(b => b.BuildComponents)
                        .ThenInclude(bc => bc.OptionProduct)
                            .ThenInclude(op => op.Media)
                .FirstOrDefaultAsync(ci => ci.CartItemId == cartItemId);
        }
    }
}
