using DesignABear.Repositories.Models;

namespace DesignABear.Repositories.Repositories.Interfaces
{
    public interface IProductionJobRepository : IGenericRepository<ProductionJob>
    {
        Task<IEnumerable<ProductionJob>> GetByStatusAsync(string status);
        Task<IEnumerable<ProductionJob>> GetByTechnicianAsync(Guid technicianId);
        Task<ProductionJob?> GetByOrderItemAsync(Guid orderItemId);
    }
}
