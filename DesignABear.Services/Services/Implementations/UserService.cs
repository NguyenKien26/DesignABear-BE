using AutoMapper;
using DesignABear.Repositories.Models;
using DesignABear.Repositories.UnitOfWork;
using DesignABear.Services.DTOs.Requests;
using DesignABear.Services.DTOs.Responses;
using DesignABear.Services.Exceptions;
using DesignABear.Services.Services.Interfaces;

using Microsoft.Extensions.Logging;

namespace DesignABear.Services.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        private readonly ILogger<UserService> _logger;

        public UserService(IUnitOfWork uow, IMapper mapper, IAuthService authService, ILogger<UserService> logger)
        {
            _uow = uow;
            _mapper = mapper;
            _authService = authService;
            _logger = logger;
        }

        public async Task<UserResponse?> GetByIdAsync(Guid userId)
        {
            var user = await _uow.Users.GetWithRolesAsync(userId);
            if (user == null) throw new NotFoundException("User", userId);

            _logger.LogInformation("Fetched profile for user {UserId}", userId);
            return _mapper.Map<UserResponse>(user);
        }

        public async Task<UserResponse?> GetByEmailAsync(string email)
        {
            var user = await _uow.Users.GetByEmailAsync(email);
            if (user == null) return null;
            return _mapper.Map<UserResponse>(user);
        }

        public async Task<IEnumerable<UserResponse>> GetAllAsync()
        {
            var users = await _uow.Users.GetAllWithRolesAsync();
            return _mapper.Map<IEnumerable<UserResponse>>(users);
        }

        public async Task<UserResponse> CreateAsync(CreateUserRequest request)
        {
            var existing = await _uow.Users.GetByEmailAsync(request.Email);
            if (existing != null) throw new BadRequestException("Email is already in use.");

            var user = _mapper.Map<User>(request);
            user.UserId = Guid.NewGuid();
            user.PasswordHash = _authService.HashPassword(request.Password);
            user.Provider = "Local";
            user.Status = DesignABear.Repositories.Models.UserStatus.Active;
            user.CreatedAt = DateTime.UtcNow;
            user.UpdatedAt = DateTime.UtcNow;

            await _uow.Users.AddAsync(user);
            await _uow.SaveChangesAsync();

            return _mapper.Map<UserResponse>(user);
        }

        public async Task UpdateAsync(Guid userId, UpdateUserRequest request)
        {
            var user = await _uow.Users.GetByIdAsync(userId);
            if (user == null) throw new NotFoundException("User", userId);

            if (request.FullName != null) user.FullName = request.FullName;
            if (request.PhoneNumber != null) user.PhoneNumber = request.PhoneNumber;
            if (request.DateOfBirth != null) user.DateOfBirth = DateOnly.FromDateTime(request.DateOfBirth.Value);
            if (request.Gender != null) user.Gender = request.Gender;
            if (request.Language != null) user.Language = request.Language;
            if (request.Timezone != null) user.Timezone = request.Timezone;
            if (request.AvatarUrl != null) user.AvatarUrl = request.AvatarUrl;
            
            // Note: UpdateUserRequest.Status mapping skipped here as it should be handled defensively.

            user.UpdatedAt = DateTime.UtcNow;

            _uow.Users.Update(user);
            await _uow.SaveChangesAsync();

            _logger.LogInformation("Updated profile for user {UserId}", userId);
        }

        public async Task DeleteAsync(Guid userId)
        {
            var user = await _uow.Users.GetByIdAsync(userId);
            if (user != null)
            {
                _uow.Users.Remove(user);
                await _uow.SaveChangesAsync();
            }
        }

        public async Task AssignRoleAsync(Guid userId, int roleId)
        {
            var user = await _uow.Users.GetByIdAsync(userId);
            if (user == null) throw new NotFoundException("User", userId);

            user.RoleId = roleId;
            user.UpdatedAt = DateTime.UtcNow;
            
            _uow.Users.Update(user);
            await _uow.SaveChangesAsync();
        }

        public async Task RemoveRoleAsync(Guid userId, int roleId)
        {
            var user = await _uow.Users.GetByIdAsync(userId);
            if (user != null && user.RoleId == roleId)
            {
                user.RoleId = 3; // Reset to Parent
                user.UpdatedAt = DateTime.UtcNow;
                _uow.Users.Update(user);
                await _uow.SaveChangesAsync();
            }
        }
        
        public async Task BlockUserAsync(Guid userId)
        {
            var user = await _uow.Users.GetByIdAsync(userId);
            if (user == null) throw new NotFoundException("User", userId);

            user.Status = UserStatus.Blocked;
            user.UpdatedAt = DateTime.UtcNow;

            _uow.Users.Update(user);
            await _uow.SaveChangesAsync();

            _logger.LogInformation("Blocked user {UserId}", userId);
        }

        public async Task UnblockUserAsync(Guid userId)
        {
            var user = await _uow.Users.GetByIdAsync(userId);
            if (user == null) throw new NotFoundException("User", userId);

            user.Status = UserStatus.Active;
            user.UpdatedAt = DateTime.UtcNow;

            _uow.Users.Update(user);
            await _uow.SaveChangesAsync();

            _logger.LogInformation("Unblocked user {UserId}", userId);
        }
    }
}
