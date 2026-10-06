using DesignABear.Repositories.Models;

namespace DesignABear.Services.Services.Interfaces
{
    public interface IAuthService
    {
        string GenerateJwtToken(User user);
        string HashPassword(string password);
        bool VerifyPassword(string plainPassword, string hashedPassword);
        Task<DesignABear.Services.DTOs.Responses.GoogleAuthResponse> GoogleLoginAsync(string idToken);
        Task<string> CompleteGoogleProfileAsync(DesignABear.Services.DTOs.Requests.GoogleCompleteProfileRequest request);
    }
}
