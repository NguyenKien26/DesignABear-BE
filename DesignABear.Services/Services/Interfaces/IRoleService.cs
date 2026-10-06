using DesignABear.Services.DTOs.Requests;
using DesignABear.Services.DTOs.Responses;

namespace DesignABear.Services.Services.Interfaces
{
    public interface IRoleService
    {
        Task<RoleResponse?> GetByIdAsync(int roleId);
        Task<IEnumerable<RoleResponse>> GetAllAsync();
        Task<RoleResponse> CreateAsync(CreateRoleRequest request);
        Task UpdateAsync(int roleId, UpdateRoleRequest request);
        Task DeleteAsync(int roleId);
    }
}
