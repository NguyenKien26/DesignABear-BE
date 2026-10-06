using DesignABear.Repositories.Data;
using DesignABear.Repositories.Models;
using DesignABear.Repositories.Repositories.Interfaces;

namespace DesignABear.Repositories.Repositories.Implementations
{
    public class AccessoryRepository : GenericRepository<Accessory>, IAccessoryRepository
    {
        public AccessoryRepository(AppDbContext context) : base(context)
        {
        }
    }
}
