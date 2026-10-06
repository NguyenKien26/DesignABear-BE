using DesignABear.Repositories.Models;

namespace DesignABear.Repositories.Repositories.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByProviderAsync(string provider, string providerId);
        Task<User?> GetWithRolesAsync(Guid userId);
        Task<IEnumerable<User>> GetAllWithRolesAsync();
    }
}
