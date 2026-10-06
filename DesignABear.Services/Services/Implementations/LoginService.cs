using DesignABear.Repositories.UnitOfWork;
using DesignABear.Services.DTOs.Requests;
using DesignABear.Services.Services.Interfaces;

namespace DesignABear.Services.Services.Implementations
{
    public class LoginService : ILoginService
    {
        private readonly IUnitOfWork _uow;
        private readonly IAuthService _authService;

        public LoginService(IUnitOfWork uow, IAuthService authService)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        }

        public async Task<(bool IsSuccess, string ResultOrError)> ValidateUserAsync(LoginRequest request)
        {
            var user = await _uow.Users.GetByEmailAsync(request.Email);
            if (user == null)
            {
                return (false, "Wrong email or password.");
            }

            // Google-only accounts have no local password
            if (user.Provider != "Local" || string.IsNullOrEmpty(user.PasswordHash))
            {
                return (false, "This account uses Google login. Please sign in with Google.");
            }

            if (!_authService.VerifyPassword(request.Password, user.PasswordHash))
            {
                return (false, "Wrong email or password.");
            }

            if (user.Status != DesignABear.Repositories.Models.UserStatus.Active)
            {
                return (false, "This account has been disabled.");
            }

            var token = _authService.GenerateJwtToken(user);
            return (true, token);
        }
    }
}
