using DesignABear.Services.DTOs.Requests;
using DesignABear.Services.DTOs.Responses;

namespace DesignABear.Services.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserResponse?> GetByIdAsync(Guid userId);
        Task<UserResponse?> GetByEmailAsync(string email);
        Task<IEnumerable<UserResponse>> GetAllAsync();
        Task<UserResponse> CreateAsync(CreateUserRequest request);
        Task UpdateAsync(Guid userId, UpdateUserRequest request);
        Task DeleteAsync(Guid userId);
        Task AssignRoleAsync(Guid userId, int roleId);
        Task RemoveRoleAsync(Guid userId, int roleId);
        Task BlockUserAsync(Guid userId);
        Task UnblockUserAsync(Guid userId);
    }
}
