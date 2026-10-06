using DesignABear.Repositories.Paging;
using DesignABear.Services.DTOs.Requests;
using DesignABear.Services.DTOs.Responses;

namespace DesignABear.Services.Services.Interfaces
{
    public interface IProductService
    {
        Task<ProductDetailResponse?> GetBySlugAsync(string slug);
        Task<ProductDetailResponse?> GetByIdAsync(Guid productId);
        Task<ProductFullDetailResponse?> GetFullByIdAsync(Guid productId);
        Task<PagedResult<ProductListResponse>> GetProductsAsync(int pageIndex, int pageSize, string? sortBy = null, int? roleId = null);
        Task<List<ProductListResponse>> GetTopProductsAsync(int count = 20);
        Task<ProductDetailResponse> CreateAsync(CreateProductRequest request);
        Task UpdateAsync(Guid productId, UpdateProductRequest request);
        Task UpdateTotalSalesAsync(Guid productId, int totalSales);
        Task IncrementViewCountAsync(Guid productId);
        Task DeleteAsync(Guid productId);
        Task<bool> IsSlugUniqueAsync(string slug, Guid? excludeProductId = null);
    }
}
