using DesignABear.Services.DTOs.Requests;
using DesignABear.Services.DTOs.Responses;

namespace DesignABear.Services.Services.Interfaces
{
    public interface IProductMediaService
    {
        Task<ProductMediaResponse?> GetByIdAsync(Guid mediaId);
        Task<IEnumerable<ProductMediaResponse>> GetByProductIdAsync(Guid productId);
        Task<ProductMediaResponse> CreateAsync(CreateProductMediaRequest request);
        Task UpdateAsync(Guid mediaId, UpdateProductMediaRequest request);
        Task DeleteAsync(Guid mediaId);
    }
}
