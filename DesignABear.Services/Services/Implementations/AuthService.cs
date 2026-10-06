using DesignABear.Repositories.Models;
using DesignABear.Repositories.UnitOfWork;
using DesignABear.Services.Services.Interfaces;
using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DesignABear.Services.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _uow;
        private readonly IConfiguration _configuration;

        public AuthService(IUnitOfWork uow, IConfiguration configuration)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        // ─────────────────────────────────────────────────────────
        // JWT
        // ─────────────────────────────────────────────────────────
        public string GenerateJwtToken(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            var jwtKey = _configuration["Jwt:Key"]
                         ?? throw new InvalidOperationException("JWT key is missing in configuration.");

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("id",       user.UserId.ToString()),
                new Claim("email",    user.Email),
                new Claim("fullname", user.FullName ?? string.Empty),
                new Claim("provider", user.Provider)
            };

            // Attach role claim
            if (user.RoleId > 0)
            {
                claims.Add(new Claim(ClaimTypes.Role, user.RoleId.ToString()));
                if (user.Role != null)
                {
                    claims.Add(new Claim("role_name", user.Role.RoleName));
                }
            }

            var key   = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer:             _configuration["Jwt:Issuer"],
                audience:           _configuration["Jwt:Audience"],
                claims:             claims,
                expires:            DateTime.UtcNow.AddMinutes(120),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // ─────────────────────────────────────────────────────────
        // Password (BCrypt)
        // ─────────────────────────────────────────────────────────
        public string HashPassword(string password) =>
            BCrypt.Net.BCrypt.HashPassword(password);

        public bool VerifyPassword(string plainPassword, string hashedPassword) =>
            BCrypt.Net.BCrypt.Verify(plainPassword, hashedPassword);

        // ─────────────────────────────────────────────────────────
        // Google OAuth
        // ─────────────────────────────────────────────────────────
        public async Task<DesignABear.Services.DTOs.Responses.GoogleAuthResponse> GoogleLoginAsync(string idToken)
        {
            try
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new[] { _configuration["Authentication:Google:ClientId"] }
                };

                var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);

                // Try finding user by Google provider first
                var user = await _uow.Users.GetByProviderAsync("Google", payload.Subject);

                if (user == null)
                {
                    // Fall back to email match (user may have registered locally before)
                    user = await _uow.Users.GetByEmailAsync(payload.Email);

                    if (user == null)
                    {
                        // User does not exist at all -> return temporary token for profile completion
                        var tempClaims = new List<Claim>
                        {
                            new Claim("purpose", "profile_completion"),
                            new Claim(JwtRegisteredClaimNames.Sub, payload.Email),
                            new Claim("email", payload.Email),
                            new Claim("fullname", payload.Name ?? string.Empty),
                            new Claim("provider", "Google"),
                            new Claim("provider_id", payload.Subject)
                        };

                        if (!string.IsNullOrEmpty(payload.Picture))
                        {
                            tempClaims.Add(new Claim("avatar_url", payload.Picture));
                        }

                        var tempKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
                        var tempCreds = new SigningCredentials(tempKey, SecurityAlgorithms.HmacSha256);
                        var tempToken = new JwtSecurityToken(
                            issuer: _configuration["Jwt:Issuer"],
                            audience: _configuration["Jwt:Audience"],
                            claims: tempClaims,
                            expires: DateTime.UtcNow.AddMinutes(30), // Short-lived token
                            signingCredentials: tempCreds
                        );

                        var tempTokenString = new JwtSecurityTokenHandler().WriteToken(tempToken);

                        return new DesignABear.Services.DTOs.Responses.GoogleAuthResponse
                        {
                            IsNewUser = true,
                            RegistrationToken = tempTokenString,
                            Email = payload.Email,
                            FullName = payload.Name,
                            AvatarUrl = payload.Picture
                        };
                    }
                    else
                    {
                        // Link Google provider to existing local account
                        user.Provider   = "Google";
                        user.ProviderId = payload.Subject;
                        user.AvatarUrl  ??= payload.Picture;
                        user.UpdatedAt  = DateTime.UtcNow;
                        _uow.Users.Update(user);
                        await _uow.SaveChangesAsync();
                    }
                }
                else
                {
                    // Existing Google user – refresh avatar if changed
                    if (user.AvatarUrl != payload.Picture)
                    {
                        user.AvatarUrl = payload.Picture;
                        user.UpdatedAt = DateTime.UtcNow;
                        _uow.Users.Update(user);
                        await _uow.SaveChangesAsync();
                    }
                }

                if (user.Status != DesignABear.Repositories.Models.UserStatus.Active)
                    throw new UnauthorizedAccessException("This account has been disabled.");

                // Re-load with roles for claims
                var userWithRoles = await _uow.Users.GetWithRolesAsync(user.UserId) ?? user;
                return new DesignABear.Services.DTOs.Responses.GoogleAuthResponse
                {
                    IsNewUser = false,
                    Token = GenerateJwtToken(userWithRoles),
                    Email = userWithRoles.Email,
                    FullName = userWithRoles.FullName,
                    AvatarUrl = userWithRoles.AvatarUrl
                };
            }
            catch (InvalidJwtException ex)
            {
                throw new UnauthorizedAccessException("Invalid Google token.", ex);
            }
            catch (UnauthorizedAccessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Google authentication error.", ex);
            }
        }

        public async Task<string> CompleteGoogleProfileAsync(DesignABear.Services.DTOs.Requests.GoogleCompleteProfileRequest request)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtKey = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT key is missing.");

            try
            {
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                    ValidateIssuer = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = _configuration["Jwt:Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                var principal = handler.ValidateToken(request.RegistrationToken, validationParameters, out var validatedToken);

                var purpose = principal.FindFirst("purpose")?.Value;
                if (purpose != "profile_completion")
                {
                    throw new UnauthorizedAccessException("Invalid token purpose.");
                }

                var email = principal.FindFirst(ClaimTypes.Email)?.Value ?? principal.FindFirst("email")?.Value;
                var providerId = principal.FindFirst("provider_id")?.Value;
                var avatarUrl = principal.FindFirst("avatar_url")?.Value;

                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(providerId))
                {
                    var claimsList = string.Join(", ", principal.Claims.Select(c => c.Type));
                    throw new UnauthorizedAccessException($"Token is missing required claims (email or provider_id). Claims present: {claimsList}");
                }

                // Double check if user already exists
                var existingUser = await _uow.Users.GetByEmailAsync(email);
                if (existingUser != null)
                {
                    throw new InvalidOperationException("User already exists.");
                }

                string token = null!;
                await _uow.ExecuteInTransactionAsync(async () =>
                {
                    var user = new User
                    {
                        UserId = Guid.NewGuid(),
                        Email = email,
                        PasswordHash = HashPassword(request.Password),
                        FullName = request.FullName,
                        PhoneNumber = request.PhoneNumber,
                        AvatarUrl = avatarUrl,
                        Gender = request.Gender,
                        DateOfBirth = request.DateOfBirth.HasValue ? DateOnly.FromDateTime(request.DateOfBirth.Value) : null,
                        Provider = "Google",
                        ProviderId = providerId,
                        Status = DesignABear.Repositories.Models.UserStatus.Active,
                        RoleId = 3, // Default Parent role
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };

                    await _uow.Users.AddAsync(user);
                    await _uow.SaveChangesAsync();

                    var userWithRoles = await _uow.Users.GetWithRolesAsync(user.UserId) ?? user;
                    token = GenerateJwtToken(userWithRoles);
                });

                return token;
            }
            catch (SecurityTokenException ex)
            {
                throw new UnauthorizedAccessException($"Invalid or expired registration token: {ex.Message}");
            }
        }
    }
}
