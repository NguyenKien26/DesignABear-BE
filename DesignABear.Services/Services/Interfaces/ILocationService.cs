using DesignABear.Services.DTOs.Requests;
using DesignABear.Services.DTOs.Responses;

namespace DesignABear.Services.Services.Interfaces
{
    public interface ILocationService
    {
        Task<IEnumerable<LocationResponse>> GetAllAsync();
        Task<LocationResponse?> GetByIdAsync(Guid locationId);
        Task<LocationResponse> CreateAsync(CreateLocationRequest request);
        Task UpdateAsync(Guid locationId, UpdateLocationRequest request);
        Task DeleteAsync(Guid locationId);
    }
}
