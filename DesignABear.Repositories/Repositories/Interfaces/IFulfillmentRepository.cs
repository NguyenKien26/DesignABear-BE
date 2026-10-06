using DesignABear.Repositories.Models;

namespace DesignABear.Repositories.Repositories.Interfaces
{
    public interface IFulfillmentRepository : IGenericRepository<Fulfillment>
    {
        Task<IEnumerable<Fulfillment>> GetByOrderIdAsync(Guid orderId);
        Task<IEnumerable<Fulfillment>> GetByStatusAsync(string status);
    }
}
