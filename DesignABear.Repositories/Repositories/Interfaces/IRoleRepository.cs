using DesignABear.Repositories.Models;

namespace DesignABear.Repositories.Repositories.Interfaces
{
    public interface IRoleRepository : IGenericRepository<Role>
    {
        Task<Role?> GetByNameAsync(string roleName);
    }
}
