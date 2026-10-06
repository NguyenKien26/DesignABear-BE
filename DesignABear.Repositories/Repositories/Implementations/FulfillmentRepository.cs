using Microsoft.EntityFrameworkCore;
using DesignABear.Repositories.Data;
using DesignABear.Repositories.Models;
using DesignABear.Repositories.Repositories.Interfaces;

namespace DesignABear.Repositories.Repositories.Implementations
{
    public class FulfillmentRepository : GenericRepository<Fulfillment>, IFulfillmentRepository
    {
        public FulfillmentRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Fulfillment>> GetByOrderIdAsync(Guid orderId)
            => await _dbSet.Where(f => f.OrderId == orderId).ToListAsync();

        public async Task<IEnumerable<Fulfillment>> GetByStatusAsync(string status)
            => await _dbSet.Where(f => f.Status == status).ToListAsync();
    }
}
