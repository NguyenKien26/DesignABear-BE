using DesignABear.Services.DTOs.Requests;
using DesignABear.Services.DTOs.Responses;

namespace DesignABear.Services.Services.Interfaces
{
    public interface IPersonalizationGroupService
    {
        Task<IEnumerable<PersonalizationGroupResponse>> GetAllAsync();
        Task<PersonalizationGroupResponse?> GetByIdAsync(Guid groupId);
        Task<PersonalizationGroupResponse> CreateAsync(CreatePersonalizationGroupRequest request);
        Task UpdateAsync(Guid groupId, UpdatePersonalizationGroupRequest request);
        Task DeleteAsync(Guid groupId);
    }
}
