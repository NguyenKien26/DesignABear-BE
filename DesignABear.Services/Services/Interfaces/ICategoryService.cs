using DesignABear.Services.DTOs.Requests;
using DesignABear.Services.DTOs.Responses;

namespace DesignABear.Services.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<CategoryResponse?> GetByIdAsync(Guid categoryId, bool includeInactive = false);
        Task<IEnumerable<CategoryResponse>> GetAllAsync(bool includeInactive = false);
        Task<CategoryResponse> CreateAsync(CreateCategoryRequest request);
        Task UpdateAsync(Guid categoryId, UpdateCategoryRequest request);
        Task DeleteAsync(Guid categoryId);
        Task RestoreAsync(Guid categoryId);
    }
}
