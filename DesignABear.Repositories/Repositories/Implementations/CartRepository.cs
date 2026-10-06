using Microsoft.EntityFrameworkCore;
using DesignABear.Repositories.Data;
using DesignABear.Repositories.Models;
using DesignABear.Repositories.Repositories.Interfaces;

namespace DesignABear.Repositories.Repositories.Implementations
{
    public class CartRepository : GenericRepository<Cart>, ICartRepository
    {
        public CartRepository(AppDbContext context) : base(context) { }

        public async Task<Cart?> GetWithItemsAsync(Guid cartId)
            => await _dbSet
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                        .ThenInclude(p => p.Media)
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Variant)
                        .ThenInclude(v => v.Product)
                            .ThenInclude(p => p.Media)
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Build)
                        .ThenInclude(b => b.BaseProduct)
                            .ThenInclude(p => p.Media)
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Build)
                        .ThenInclude(b => b.BaseVariant)
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Build)
                        .ThenInclude(b => b.BuildComponents)
                            .ThenInclude(bc => bc.OptionVariant)
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Build)
                        .ThenInclude(b => b.BuildComponents)
                            .ThenInclude(bc => bc.OptionProduct)
                                .ThenInclude(op => op.Media)
                .FirstOrDefaultAsync(c => c.CartId == cartId);

        public async Task<Cart?> GetActiveCartByUserAsync(Guid userId)
            => await _dbSet
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                        .ThenInclude(p => p.Media)
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Variant)
                        .ThenInclude(v => v.Product)
                            .ThenInclude(p => p.Media)
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Build)
                        .ThenInclude(b => b.BaseProduct)
                            .ThenInclude(p => p.Media)
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Build)
                        .ThenInclude(b => b.BaseVariant)
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Build)
                        .ThenInclude(b => b.BuildComponents)
                            .ThenInclude(bc => bc.OptionVariant)
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Build)
                        .ThenInclude(b => b.BuildComponents)
                            .ThenInclude(bc => bc.OptionProduct)
                                .ThenInclude(op => op.Media)
                .Where(c => c.UserId == userId)
                .OrderByDescending(c => c.UpdatedAt)
                .ThenByDescending(c => c.CreatedAt)
                .FirstOrDefaultAsync();

        public async Task UpdateTimestampAsync(Guid cartId)
        {
            await _dbSet.Where(c => c.CartId == cartId)
                .ExecuteUpdateAsync(s => s.SetProperty(c => c.UpdatedAt, c => DateTime.UtcNow));
        }
    }
}
