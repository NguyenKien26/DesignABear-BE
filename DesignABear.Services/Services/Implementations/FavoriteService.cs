using AutoMapper;
using DesignABear.Repositories.Models;
using DesignABear.Repositories.UnitOfWork;
using DesignABear.Services.DTOs.Responses;
using DesignABear.Services.Services.Interfaces;
using DesignABear.Repositories.Paging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DesignABear.Services.Services.Implementations
{
    public class FavoriteService : IFavoriteService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public FavoriteService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<bool> ToggleFavoriteAsync(Guid userId, Guid productId)
        {
            var existing = await _uow.Favorites.GetAsync(userId, productId);

            if (existing != null)
            {
                _uow.Favorites.Remove(existing);
                await _uow.SaveChangesAsync();
                return false; // Removed
            }

            var product = await _uow.Products.GetByIdAsync(productId);
            if (product == null) throw new ArgumentException("Product not found.");

            var favorite = new Favorite
            {
                FavoriteId = Guid.NewGuid(),
                UserId = userId,
                ProductId = productId,
                CreatedAt = DateTime.UtcNow
            };

            await _uow.Favorites.AddAsync(favorite);
            await _uow.SaveChangesAsync();
            return true; // Added
        }

        public async Task<PagedResult<FavoriteResponse>> GetMyFavoritesAsync(Guid userId, int pageIndex, int pageSize)
        {
            var pagedFavorites = await _uow.Favorites.GetPagedByUserAsync(userId, pageIndex, pageSize);
            
            var responses = _mapper.Map<List<FavoriteResponse>>(pagedFavorites.Items);

            return new PagedResult<FavoriteResponse>
            {
                PageIndex = pagedFavorites.PageIndex,
                PageSize = pagedFavorites.PageSize,
                TotalCount = pagedFavorites.TotalCount,
                Items = responses
            };
        }
    }
}
