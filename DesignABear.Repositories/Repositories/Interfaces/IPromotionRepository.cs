using DesignABear.Repositories.Models;

namespace DesignABear.Repositories.Repositories.Interfaces
{
    public interface IPromotionRepository : IGenericRepository<Promotion>
    {
        Task<Promotion?> GetByCodeAsync(string code);
        Task<IEnumerable<Promotion>> GetActivePromotionsAsync();
    }
}
