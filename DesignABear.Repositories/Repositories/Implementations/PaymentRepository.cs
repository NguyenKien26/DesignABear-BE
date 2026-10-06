using Microsoft.EntityFrameworkCore;
using DesignABear.Repositories.Data;
using DesignABear.Repositories.Models;
using DesignABear.Repositories.Repositories.Interfaces;

namespace DesignABear.Repositories.Repositories.Implementations
{
    public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Payment>> GetByOrderIdAsync(Guid orderId)
            => await _dbSet.Where(p => p.OrderId == orderId).ToListAsync();

        public async Task<Payment?> GetByExternalTxnIdAsync(string externalTxnId)
            => await _dbSet.FirstOrDefaultAsync(p => p.ExternalTxnId == externalTxnId);
    }
}
