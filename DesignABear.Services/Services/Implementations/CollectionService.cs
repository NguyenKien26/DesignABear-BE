using AutoMapper;
using DesignABear.Repositories.Models;
using DesignABear.Repositories.UnitOfWork;
using DesignABear.Services.DTOs.Requests;
using DesignABear.Services.DTOs.Responses;
using DesignABear.Services.Exceptions;
using DesignABear.Services.Services.Interfaces;

namespace DesignABear.Services.Services.Implementations
{
    public class CollectionService : ICollectionService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public CollectionService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CollectionResponse>> GetAllAsync()
        {
            var collections = await _uow.Collections.GetAllAsync();
            return _mapper.Map<IEnumerable<CollectionResponse>>(collections);
        }

        public async Task<CollectionResponse?> GetByIdAsync(Guid collectionId)
        {
            var collection = await _uow.Collections.GetByIdAsync(collectionId);
            if (collection == null) throw new NotFoundException("Collection", collectionId);
            return _mapper.Map<CollectionResponse>(collection);
        }

        public async Task<CollectionResponse?> GetBySlugAsync(string slug)
        {
            var collection = await _uow.Collections.GetBySlugAsync(slug);
            if (collection == null) throw new NotFoundException("Collection", slug);
            return _mapper.Map<CollectionResponse>(collection);
        }

        public async Task<CollectionResponse> CreateAsync(CreateCollectionRequest request)
        {
            var collection = _mapper.Map<Collection>(request);
            collection.CollectionId = Guid.NewGuid();

            await _uow.Collections.AddAsync(collection);
            await _uow.SaveChangesAsync();

            return _mapper.Map<CollectionResponse>(collection);
        }

        public async Task UpdateAsync(Guid collectionId, UpdateCollectionRequest request)
        {
            var collection = await _uow.Collections.GetByIdAsync(collectionId);
            if (collection == null) throw new NotFoundException("Collection", collectionId);

            _mapper.Map(request, collection);

            _uow.Collections.Update(collection);
            await _uow.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid collectionId)
        {
            var collection = await _uow.Collections.GetByIdAsync(collectionId);
            if (collection is not null)
            {
                _uow.Collections.Remove(collection);
                await _uow.SaveChangesAsync();
            }
        }

        public async Task AddProductAsync(Guid collectionId, Guid productId)
        {
            var collection = await _uow.Collections.GetByIdAsync(collectionId);
            if (collection == null) throw new NotFoundException("Collection", collectionId);

            var product = await _uow.Products.GetByIdAsync(productId);
            if (product == null) throw new NotFoundException("Product", productId);

            var exists = await _uow.ProductCollections.AnyAsync(pc => pc.CollectionId == collectionId && pc.ProductId == productId);
            if (exists) return;

            await _uow.ProductCollections.AddAsync(new ProductCollection
            {
                CollectionId = collectionId,
                ProductId = productId
            });

            await _uow.SaveChangesAsync();
        }

        public async Task RemoveProductAsync(Guid collectionId, Guid productId)
        {
            var links = await _uow.ProductCollections.FindAsync(pc => pc.CollectionId == collectionId && pc.ProductId == productId);
            var link = links.FirstOrDefault();
            if (link == null) return;

            _uow.ProductCollections.Remove(link);
            await _uow.SaveChangesAsync();
        }
    }
}
