using Microsoft.EntityFrameworkCore;
using DesignABear.Repositories.Data;
using DesignABear.Repositories.Models;
using DesignABear.Repositories.Repositories.Interfaces;

namespace DesignABear.Repositories.Repositories.Implementations
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context) : base(context) { }

        public async Task<Category?> GetBySlugAsync(string slug)
            => await _dbSet.FirstOrDefaultAsync(c => c.Slug == slug);

        public async Task<IEnumerable<Category>> GetRootCategoriesAsync()
            => await _dbSet.Where(c => c.ParentId == null).ToListAsync();

        public async Task<IEnumerable<Category>> GetChildrenAsync(Guid parentId)
            => await _dbSet.Where(c => c.ParentId == parentId).ToListAsync();
    }
}
