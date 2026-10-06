using DesignABear.Repositories.Models;

namespace DesignABear.Repositories.Repositories.Interfaces
{
    public interface ICollectionRepository : IGenericRepository<Collection>
    {
        Task<Collection?> GetBySlugAsync(string slug);
    }
}
