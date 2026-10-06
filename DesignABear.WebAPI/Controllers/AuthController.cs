using DesignABear.Services.Common;
using DesignABear.Services.DTOs.Requests;
using DesignABear.Services.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DesignABear.WebAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly ILoginService _loginService;
        private readonly IRegisterService _registerService;
        private readonly IAuthService _authService;

        public AuthController(
            ILoginService loginService,
            IRegisterService registerService,
            IAuthService authService)
        {
            _loginService   = loginService   ?? throw new ArgumentNullException(nameof(loginService));
            _registerService = registerService ?? throw new ArgumentNullException(nameof(registerService));
            _authService    = authService    ?? throw new ArgumentNullException(nameof(authService));
        }

        /// <summary>
        /// Registers a new user account.
        /// </summary>
        /// <param name="request">The registration details.</param>
        /// <returns>A result containing a success message or error.</returns>
        [HttpPost("signup")]
        [SwaggerOperation(Summary = "Sign up with email and password", Description = "Registers a new user. Use default test hash if applicable.")]
        public async Task<IActionResult> SignUp([FromBody] RegisterRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                return HandleResult(Result<string>.Failure(new Error("BadRequest", "Email and password are required.")));
            }

            var (isSuccess, resultOrError) = await _registerService.SignUpAsync(request);
            if (!isSuccess)
            {
                return HandleResult(Result<string>.Failure(new Error("RegistrationFailed", resultOrError)));
            }

            return HandleResult(Result<string>.Success(resultOrError));
        }

        /// <summary>
        /// Authenticates a user and returns a JWT token.
        /// </summary>
        /// <param name="request">The login credentials.</param>
        /// <returns>A result containing the JWT token and user info.</returns>
        [HttpPost("login")]
        [SwaggerOperation(Summary = "Login with email and password", Description = "Authenticates user and returns JWT.")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                return HandleResult(Result<object>.Failure(new Error("BadRequest", "Email and password are required.")));
            }

            var (isSuccess, resultOrError) = await _loginService.ValidateUserAsync(request);
            if (!isSuccess)
            {
                return HandleResult(Result<object>.Failure(new Error("Unauthorized", resultOrError)));
            }

            return HandleResult(Result<object>.Success(new { token = resultOrError, message = "Login successful." }));
        }

        /// <summary>
        /// Authenticates a user via Google OAuth credential.
        /// </summary>
        /// <param name="request">The Google credential token.</param>
        /// <returns>A result containing the JWT token if successful.</returns>
        [HttpPost("google-login")]
        [SwaggerOperation(Summary = "Login with Google OAuth")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleTokenRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Credential))
            {
                return HandleResult(Result<object>.Failure(new Error("BadRequest", "Google credential is required.")));
            }

            try
            {
                var response = await _authService.GoogleLoginAsync(request.Credential);
                return HandleResult(Result<object>.Success(new 
                { 
                    isNewUser = response.IsNewUser, 
                    token = response.Token, 
                    registrationToken = response.RegistrationToken,
                    email = response.Email,
                    fullName = response.FullName,
                    avatarUrl = response.AvatarUrl,
                    message = response.IsNewUser ? "Please complete your profile." : "Google login successful." 
                }));
            }
            catch (UnauthorizedAccessException ex)
            {
                return HandleResult(Result<object>.Failure(new Error("Unauthorized", ex.Message)));
            }
        }

        /// <summary>
        /// Completes the Google registration profile.
        /// </summary>
        /// <param name="request">The profile details and temporary token.</param>
        /// <returns>A result containing the final JWT token if successful.</returns>
        [HttpPost("google-complete-profile")]
        [SwaggerOperation(Summary = "Complete profile for new Google users")]
        public async Task<IActionResult> CompleteGoogleProfile([FromBody] GoogleCompleteProfileRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.RegistrationToken))
            {
                return HandleResult(Result<object>.Failure(new Error("BadRequest", "Registration token is required.")));
            }

            try
            {
                var token = await _authService.CompleteGoogleProfileAsync(request);
                return HandleResult(Result<object>.Success(new { token, message = "Profile completed and login successful." }));
            }
            catch (UnauthorizedAccessException ex)
            {
                return HandleResult(Result<object>.Failure(new Error("Unauthorized", ex.Message)));
            }
            catch (InvalidOperationException ex)
            {
                return HandleResult(Result<object>.Failure(new Error("Conflict", ex.Message)));
            }
        }

        /// <summary>
        /// Verifies a user's email using an OTP code sent during local registration.
        /// </summary>
        /// <param name="request">The email and OTP code.</param>
        /// <returns>A result containing the JWT token if successful.</returns>
        [HttpPost("verify-email")]
        [SwaggerOperation(Summary = "Verify email with OTP")]
        public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Otp))
            {
                return HandleResult(Result<object>.Failure(new Error("BadRequest", "Email and OTP are required.")));
            }

            var (isSuccess, resultOrError) = await _registerService.VerifyEmailAsync(request.Email, request.Otp);
            if (!isSuccess)
            {
                return HandleResult(Result<object>.Failure(new Error("VerificationFailed", resultOrError)));
            }

            return HandleResult(Result<object>.Success(new { token = resultOrError, message = "Email verified successfully. You are now logged in." }));
        }
    }
}
