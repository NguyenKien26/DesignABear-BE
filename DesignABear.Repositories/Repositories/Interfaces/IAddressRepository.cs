using DesignABear.Repositories.Models;

namespace DesignABear.Repositories.Repositories.Interfaces
{
    public interface IAddressRepository : IGenericRepository<Address>
    {
        Task<IEnumerable<Address>> GetByUserIdAsync(Guid userId);
        Task<Address?> GetDefaultShippingAsync(Guid userId);
        Task<Address?> GetDefaultBillingAsync(Guid userId);
    }
}
