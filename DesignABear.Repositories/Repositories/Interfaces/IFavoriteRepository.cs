using DesignABear.Repositories.Models;
using DesignABear.Repositories.Paging;
using System;
using System.Threading.Tasks;

namespace DesignABear.Repositories.Repositories.Interfaces
{
    public interface IFavoriteRepository : IGenericRepository<Favorite>
    {
        Task<PagedResult<Favorite>> GetPagedByUserAsync(Guid userId, int pageIndex, int pageSize);
        Task<Favorite?> GetAsync(Guid userId, Guid productId);
    }
}
