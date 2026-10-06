using DesignABear.Services.DTOs.Requests;
using DesignABear.Services.DTOs.Responses;

namespace DesignABear.Services.Services.Interfaces
{
    public interface ICollectionService
    {
        Task<IEnumerable<CollectionResponse>> GetAllAsync();
        Task<CollectionResponse?> GetByIdAsync(Guid collectionId);
        Task<CollectionResponse?> GetBySlugAsync(string slug);
        Task<CollectionResponse> CreateAsync(CreateCollectionRequest request);
        Task UpdateAsync(Guid collectionId, UpdateCollectionRequest request);
        Task DeleteAsync(Guid collectionId);

        Task AddProductAsync(Guid collectionId, Guid productId);
        Task RemoveProductAsync(Guid collectionId, Guid productId);
    }
}
