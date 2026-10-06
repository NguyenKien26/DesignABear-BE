using DesignABear.Services.DTOs.Requests;

namespace DesignABear.Services.Services.Interfaces
{
    public interface ILoginService
    {
        Task<(bool IsSuccess, string ResultOrError)> ValidateUserAsync(LoginRequest request);
    }
}
