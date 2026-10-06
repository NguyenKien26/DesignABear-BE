using DesignABear.Repositories.Data;
using DesignABear.Repositories.Models;
using DesignABear.Repositories.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DesignABear.Repositories.Repositories.Implementations
{
    public class ProductComboImageRepository : GenericRepository<ProductComboImage>, IProductComboImageRepository
    {
        public ProductComboImageRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<ProductComboImage?> GetByCombinationKeyAsync(Guid baseProductId, string combinationKey)
        {
            return await _dbSet.FirstOrDefaultAsync(c => c.BaseProductId == baseProductId && c.CombinationKey == combinationKey);
        }

        public async Task<IEnumerable<ProductComboImage>> GetByBaseProductAsync(Guid baseProductId)
        {
            return await _dbSet.Where(c => c.BaseProductId == baseProductId).ToListAsync();
        }
    }
}
