using DesignABear.Repositories.Data;
using DesignABear.Repositories.Models;
using DesignABear.Repositories.Repositories.Interfaces;

namespace DesignABear.Repositories.Repositories.Implementations
{
    public class ProductAccessoryRepository : GenericRepository<ProductAccessory>, IProductAccessoryRepository
    {
        public ProductAccessoryRepository(AppDbContext context) : base(context)
        {
        }
    }
}
