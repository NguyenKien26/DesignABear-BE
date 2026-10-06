using Microsoft.EntityFrameworkCore;
using DesignABear.Repositories.Data;
using DesignABear.Repositories.Models;
using DesignABear.Repositories.Repositories.Interfaces;

namespace DesignABear.Repositories.Repositories.Implementations
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context) { }

        public async Task<User?> GetByEmailAsync(string email)
            => await _dbSet.FirstOrDefaultAsync(u => u.Email == email);

        public async Task<User?> GetByProviderAsync(string provider, string providerId)
            => await _dbSet.FirstOrDefaultAsync(u => u.Provider == provider && u.ProviderId == providerId);

        public async Task<User?> GetWithRolesAsync(Guid userId)
            => await _dbSet
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.UserId == userId);

        public async Task<IEnumerable<User>> GetAllWithRolesAsync()
            => await _dbSet.Include(u => u.Role).ToListAsync();
    }
}
