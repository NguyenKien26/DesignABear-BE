using DesignABear.Repositories.Models;

namespace DesignABear.Repositories.Repositories.Interfaces
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<Category?> GetBySlugAsync(string slug);
        Task<IEnumerable<Category>> GetRootCategoriesAsync();
        Task<IEnumerable<Category>> GetChildrenAsync(Guid parentId);
    }
}
