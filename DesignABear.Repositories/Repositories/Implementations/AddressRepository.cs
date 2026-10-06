using Microsoft.EntityFrameworkCore;
using DesignABear.Repositories.Data;
using DesignABear.Repositories.Models;
using DesignABear.Repositories.Repositories.Interfaces;

namespace DesignABear.Repositories.Repositories.Implementations
{
    public class AddressRepository : GenericRepository<Address>, IAddressRepository
    {
        public AddressRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Address>> GetByUserIdAsync(Guid userId)
            => await _dbSet.Where(a => a.UserId == userId).ToListAsync();

        public async Task<Address?> GetDefaultShippingAsync(Guid userId)
            => await _dbSet.FirstOrDefaultAsync(a => a.UserId == userId && a.IsDefaultShipping);

        public async Task<Address?> GetDefaultBillingAsync(Guid userId)
            => await _dbSet.FirstOrDefaultAsync(a => a.UserId == userId && a.IsDefaultBilling);
    }
}
