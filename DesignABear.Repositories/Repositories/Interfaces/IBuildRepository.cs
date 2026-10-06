using DesignABear.Repositories.Models;

namespace DesignABear.Repositories.Repositories.Interfaces
{
    public interface IBuildRepository : IGenericRepository<Build>
    {
        Task<Build?> GetWithComponentsAsync(Guid buildId);
        Task<IEnumerable<Build>> GetByUserIdAsync(Guid userId);
    }
}
