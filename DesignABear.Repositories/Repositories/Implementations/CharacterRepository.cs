using Microsoft.EntityFrameworkCore;
using DesignABear.Repositories.Data;
using DesignABear.Repositories.Models;
using DesignABear.Repositories.Repositories.Interfaces;

namespace DesignABear.Repositories.Repositories.Implementations
{
    public class CharacterRepository : GenericRepository<Character>, ICharacterRepository
    {
        public CharacterRepository(AppDbContext context) : base(context) { }

        public async Task<Character?> GetBySlugAsync(string slug)
            => await _dbSet.FirstOrDefaultAsync(c => c.Slug == slug);

        public async Task<IEnumerable<Character>> GetAllWithAccessoriesAsync()
        {
            return await _dbSet
                .Include(c => c.ProductCharacters)
                    .ThenInclude(pc => pc.Product)
                        .ThenInclude(p => p.BasePersonalizationRules.Where(pr => pr.IsActive))
                            .ThenInclude(pr => pr.AddonProduct)
                .Include(c => c.ProductCharacters)
                    .ThenInclude(pc => pc.Product)
                        .ThenInclude(p => p.BasePersonalizationRules)
                            .ThenInclude(pr => pr.Group)
                .AsSplitQuery()
                .ToListAsync();
        }
    }
}
