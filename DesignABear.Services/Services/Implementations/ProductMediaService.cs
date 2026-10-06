using AutoMapper;
using DesignABear.Repositories.Models;
using DesignABear.Repositories.UnitOfWork;
using DesignABear.Services.DTOs.Requests;
using DesignABear.Services.DTOs.Responses;
using DesignABear.Services.Exceptions;
using DesignABear.Services.Services.Interfaces;

namespace DesignABear.Services.Services.Implementations
{
    public class ProductMediaService : IProductMediaService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public ProductMediaService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<ProductMediaResponse?> GetByIdAsync(Guid mediaId)
        {
            var media = await _uow.ProductMedias.GetByIdAsync(mediaId);
            if (media == null) throw new NotFoundException("ProductMedia", mediaId);
            return _mapper.Map<ProductMediaResponse>(media);
        }

        public async Task<IEnumerable<ProductMediaResponse>> GetByProductIdAsync(Guid productId)
        {
            var media = await _uow.ProductMedias.FindAsync(m => m.ProductId == productId);
            // Sort by order so they come back properly aligned
            media = media.OrderBy(m => m.SortOrder).ToList();
            return _mapper.Map<IEnumerable<ProductMediaResponse>>(media);
        }

        public async Task<ProductMediaResponse> CreateAsync(CreateProductMediaRequest request)
        {
            var media = _mapper.Map<ProductMedia>(request);
            media.MediaId = Guid.NewGuid();

            await _uow.ProductMedias.AddAsync(media);
            await _uow.SaveChangesAsync();

            return _mapper.Map<ProductMediaResponse>(media);
        }

        public async Task UpdateAsync(Guid mediaId, UpdateProductMediaRequest request)
        {
            var media = await _uow.ProductMedias.GetByIdAsync(mediaId);
            if (media == null) throw new NotFoundException("ProductMedia", mediaId);

            _mapper.Map(request, media);
            _uow.ProductMedias.Update(media);
            await _uow.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid mediaId)
        {
            var media = await _uow.ProductMedias.GetByIdAsync(mediaId);
            if (media != null)
            {
                _uow.ProductMedias.Remove(media);
                await _uow.SaveChangesAsync();
            }
        }
    }
}
