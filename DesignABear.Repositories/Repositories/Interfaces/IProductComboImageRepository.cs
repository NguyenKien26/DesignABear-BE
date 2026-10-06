using DesignABear.Repositories.Models;

namespace DesignABear.Repositories.Repositories.Interfaces
{
    public interface IProductComboImageRepository : IGenericRepository<ProductComboImage>
    {
        Task<ProductComboImage?> GetByCombinationKeyAsync(Guid baseProductId, string combinationKey);
        Task<IEnumerable<ProductComboImage>> GetByBaseProductAsync(Guid baseProductId);
    }
}
