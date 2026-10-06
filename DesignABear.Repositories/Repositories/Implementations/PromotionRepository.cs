using Microsoft.EntityFrameworkCore;
using DesignABear.Repositories.Data;
using DesignABear.Repositories.Models;
using DesignABear.Repositories.Repositories.Interfaces;

namespace DesignABear.Repositories.Repositories.Implementations
{
    public class PromotionRepository : GenericRepository<Promotion>, IPromotionRepository
    {
        public PromotionRepository(AppDbContext context) : base(context) { }

        public async Task<Promotion?> GetByCodeAsync(string code)
            => await _dbSet.FirstOrDefaultAsync(p => p.Code == code);

        public async Task<IEnumerable<Promotion>> GetActivePromotionsAsync()
        {
            var now = DateTime.UtcNow;
            return await _dbSet
                .Where(p => p.IsActive && (p.StartsAt == null || p.StartsAt <= now) && (p.EndsAt == null || p.EndsAt >= now))
                .ToListAsync();
        }
    }
}
