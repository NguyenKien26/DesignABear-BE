using Microsoft.EntityFrameworkCore;
using DesignABear.Repositories.Data;
using DesignABear.Repositories.Models;
using DesignABear.Repositories.Repositories.Interfaces;

namespace DesignABear.Repositories.Repositories.Implementations
{
    public class BuildRepository : GenericRepository<Build>, IBuildRepository
    {
        public BuildRepository(AppDbContext context) : base(context) { }

        public async Task<Build?> GetWithComponentsAsync(Guid buildId)
            => await _dbSet
                .Include(b => b.BaseProduct)
                .Include(b => b.BuildComponents)
                    .ThenInclude(bc => bc.OptionProduct)
                .Include(b => b.BuildComponents)
                    .ThenInclude(bc => bc.Group)
                .FirstOrDefaultAsync(b => b.BuildId == buildId);

        public async Task<IEnumerable<Build>> GetByUserIdAsync(Guid userId)
            => await _dbSet.Where(b => b.UserId == userId).ToListAsync();
    }
}
