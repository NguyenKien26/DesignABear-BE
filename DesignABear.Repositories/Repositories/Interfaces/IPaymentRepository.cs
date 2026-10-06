using DesignABear.Repositories.Models;

namespace DesignABear.Repositories.Repositories.Interfaces
{
    public interface IPaymentRepository : IGenericRepository<Payment>
    {
        Task<IEnumerable<Payment>> GetByOrderIdAsync(Guid orderId);
        Task<Payment?> GetByExternalTxnIdAsync(string externalTxnId);
    }
}
