using DesignABear.Services.DTOs.Requests;
using DesignABear.Services.DTOs.Responses;

namespace DesignABear.Services.Services.Interfaces
{
    public interface IBuildService
    {
        Task<BuildResponse?> GetByIdAsync(Guid buildId);
        Task<BuildResponse?> GetWithComponentsAsync(Guid buildId);
        Task<IEnumerable<BuildResponse>> GetByUserIdAsync(Guid userId);
        Task<BuildResponse> CreateAsync(CreateBuildRequest request);
        Task<BuildComponentResponse> AddComponentAsync(CreateBuildComponentRequest request);
        Task RemoveComponentAsync(Guid buildComponentId);
        Task UpdateAsync(Guid buildId, UpdateBuildRequest request);
        Task DeleteAsync(Guid buildId);
        Task<decimal> CalculatePriceAsync(Guid buildId);
    }
}
