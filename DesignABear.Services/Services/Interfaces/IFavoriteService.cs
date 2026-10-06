using DesignABear.Services.DTOs.Responses;
using DesignABear.Repositories.Paging;
using System;
using System.Threading.Tasks;

namespace DesignABear.Services.Services.Interfaces
{
    public interface IFavoriteService
    {
        Task<bool> ToggleFavoriteAsync(Guid userId, Guid productId);
        Task<PagedResult<FavoriteResponse>> GetMyFavoritesAsync(Guid userId, int pageIndex, int pageSize);
    }
}
