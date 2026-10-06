using DesignABear.Services.Common;
using DesignABear.Services.DTOs.Requests;
using DesignABear.Services.DTOs.Responses;
using DesignABear.Services.Services.Interfaces;
using DesignABear.Repositories.Paging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DesignABear.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FavoritesController : BaseController
    {
        private readonly IFavoriteService _favoriteService;

        public FavoritesController(IFavoriteService favoriteService)
        {
            _favoriteService = favoriteService;
        }

        [HttpPost("toggle")]
        public async Task<IActionResult> Toggle([FromBody] ToggleFavoriteRequest request)
        {
            var userId = GetCurrentUserId();
            var isAdded = await _favoriteService.ToggleFavoriteAsync(userId, request.ProductId);
            
            return HandleResult(Result<object>.Success(new { IsAdded = isAdded, Message = isAdded ? "Added to favorites" : "Removed from favorites" }));
        }

        [HttpGet]
        public async Task<IActionResult> GetMyFavorites([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            var userId = GetCurrentUserId();
            var results = await _favoriteService.GetMyFavoritesAsync(userId, pageIndex, pageSize);
            return HandleResult(Result<PagedResult<FavoriteResponse>>.Success(results));
        }
    }
}
