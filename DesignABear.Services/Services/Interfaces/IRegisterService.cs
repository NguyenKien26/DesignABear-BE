using DesignABear.Services.DTOs.Requests;

namespace DesignABear.Services.Services.Interfaces
{
    public interface IRegisterService
    {
        Task<(bool IsSuccess, string ResultOrError)> SignUpAsync(RegisterRequest request);
        Task<(bool IsSuccess, string ResultOrError)> VerifyEmailAsync(string email, string otp);
        Task<(bool IsSuccess, string Message, string? Token)> QuickSignUpAsync(QuickRegisterRequest request);
    }
}
