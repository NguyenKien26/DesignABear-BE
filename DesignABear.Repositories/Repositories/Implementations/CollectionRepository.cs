using Microsoft.EntityFrameworkCore;
using DesignABear.Repositories.Data;
using DesignABear.Repositories.Models;
using DesignABear.Repositories.Repositories.Interfaces;

namespace DesignABear.Repositories.Repositories.Implementations
{
    public class CollectionRepository : GenericRepository<Collection>, ICollectionRepository
    {
        public CollectionRepository(AppDbContext context) : base(context) { }

        public async Task<Collection?> GetBySlugAsync(string slug)
            => await _dbSet.FirstOrDefaultAsync(c => c.Slug == slug);
    }
}
