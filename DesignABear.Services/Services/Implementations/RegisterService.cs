using DesignABear.Repositories.Models;
using DesignABear.Repositories.UnitOfWork;
using DesignABear.Services.DTOs.Requests;
using DesignABear.Services.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DesignABear.Services.Services.Implementations
{
    public class RegisterService : IRegisterService
    {
        private readonly IUnitOfWork _uow;
        private readonly IAuthService _authService;
        private readonly IEmailService _emailService;

        public RegisterService(IUnitOfWork uow, IAuthService authService, IEmailService emailService)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        }

        public async Task<(bool IsSuccess, string ResultOrError)> SignUpAsync(RegisterRequest request)
        {
            var existingUser = await _uow.Users.GetByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return (false, "Email is already in use.");
            }

            var user = new User
            {
                UserId = Guid.NewGuid(),
                Email = request.Email,
                PasswordHash = _authService.HashPassword(request.Password),
                FullName = request.FullName,
                PhoneNumber = request.PhoneNumber,
                AvatarUrl = request.AvatarUrl,
                DateOfBirth = request.DateOfBirth.HasValue ? DateOnly.FromDateTime(request.DateOfBirth.Value) : null,
                Gender = request.Gender,
                RoleId = 3, // Default to Customer/Parent
                Provider = "Local",
                ProviderId = null,
                Status = UserStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            bool success = false;
            string message = "An error occurred during registration. Please try again.";

            await _uow.ExecuteInTransactionAsync(async () =>
            {
                await _uow.Users.AddAsync(user);
                await _uow.SaveChangesAsync();

                // Generate 6-digit OTP using strong PRNG
                var otp = System.Security.Cryptography.RandomNumberGenerator.GetInt32(100000, 1000000).ToString();
                var userToken = new UserToken
                {
                    UserId = user.UserId,
                    Token = otp, 
                    Type = "EmailVerification",
                    CreatedAt = DateTime.UtcNow,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(15),
                    IsUsed = false
                };

                await _uow.UserTokens.AddAsync(userToken);
                await _uow.SaveChangesAsync();

                // Send Email in Background
                var subject = "DesignABear - Email Confirmation";
                var body = $"<h1>Welcome to DesignABear!</h1><p>Your verification code is: <strong>{otp}</strong></p><p>This code will expire in 15 minutes.</p>";
                var emailAddress = user.Email;

                _ = Task.Run(async () =>
                {
                    try
                    {
                        await _emailService.SendEmailAsync(emailAddress, subject, body, true);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Background email error: {ex.Message}");
                    }
                });

                success = true;
                message = "Account registered successfully. Please check your email for the verification code.";
            });

            return (success, message);
        }

        public async Task<(bool IsSuccess, string ResultOrError)> VerifyEmailAsync(string email, string otp)
        {
            var user = await _uow.Users.GetByEmailAsync(email);
            if (user == null) return (false, "User not found.");

            if (user.Status != UserStatus.Pending)
            {
                return (false, "Account is already verified or blocked.");
            }

            var tokens = await _uow.UserTokens.FindAsync(t => t.UserId == user.UserId && t.Type == "EmailVerification" && t.Token == otp && !t.IsUsed && t.ExpiresAt > DateTime.UtcNow);
            var tokenEntity = tokens.OrderByDescending(t => t.CreatedAt).FirstOrDefault();

            if (tokenEntity == null)
            {
                return (false, "Invalid or expired verification code.");
            }

            bool success = false;
            string resultOrError = "An error occurred during verification.";

            await _uow.ExecuteInTransactionAsync(async () =>
            {
                user.Status = UserStatus.Active;
                user.UpdatedAt = DateTime.UtcNow;
                _uow.Users.Update(user);

                tokenEntity.IsUsed = true;
                _uow.UserTokens.Update(tokenEntity);

                await _uow.SaveChangesAsync();

                var userWithRoles = await _uow.Users.GetWithRolesAsync(user.UserId) ?? user;
                var jwtToken = _authService.GenerateJwtToken(userWithRoles);

                success = true;
                resultOrError = jwtToken;
            });

            return (success, resultOrError);
        }

        public async Task<(bool IsSuccess, string Message, string? Token)> QuickSignUpAsync(QuickRegisterRequest request)
        {
            var existingUser = await _uow.Users.GetByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return (false, "Email already in use", null);
            }

            // Validation: Cannot quick-create Admin via this endpoint
            if (request.RoleId == 1) // Admin
            {
                return (false, "Cannot create Admin account via QuickSignUp", null);
            }

            var user = new User
            {
                UserId = Guid.NewGuid(),
                Email = request.Email,
                FullName = request.FullName,
                PasswordHash = _authService.HashPassword(request.Password),
                RoleId = request.RoleId,
                WorkplaceId = request.WorkplaceId,
                Status = UserStatus.Active, // Quick sign-up defaults to Active
                Provider = "Local",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _uow.Users.AddAsync(user);
            await _uow.SaveChangesAsync();

            // Fetch with roles for token generation
            var userWithRoles = await _uow.Users.GetWithRolesAsync(user.UserId) ?? user;
            var token = _authService.GenerateJwtToken(userWithRoles);

            return (true, "Account created successfully", token);
        }
    }
}
