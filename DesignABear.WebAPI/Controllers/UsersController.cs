using DesignABear.Services.Common;
using DesignABear.Services.DTOs.Requests;
using DesignABear.Services.DTOs.Responses;
using DesignABear.Services.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DesignABear.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : BaseController
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }
        
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userIdClaim = User.FindFirst("id")?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                return HandleResult(Result<UserResponse>.Failure(new Error("Unauthorized", "User ID not found in token.")));
            }

            var user = await _userService.GetByIdAsync(userId);
            if (user == null)
            {
                return HandleResult(Result<UserResponse>.Failure(new Error("NotFound", "User profile not found.")));
            }
            return HandleResult(Result<UserResponse>.Success(user));
        }

        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateUserRequest request)
        {
            var userIdClaim = User.FindFirst("id")?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                return HandleResult(Result<UserResponse>.Failure(new Error("Unauthorized", "User ID not found in token.")));
            }

            // Remove any attempt to change status via this endpoint
            request.Status = null;

            await _userService.UpdateAsync(userId, request);
            return HandleResult(Result.Success());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
            {
                return HandleResult(Result<UserResponse>.Failure(new Error("NotFound", $"User with ID {id} not found.")));
            }
            return HandleResult(Result<UserResponse>.Success(user));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllAsync();
            return HandleResult(Result<IEnumerable<UserResponse>>.Success(users));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
        {
            var newUser = await _userService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = newUser.UserId }, Result<UserResponse>.Success(newUser));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserRequest request)
        {
            await _userService.UpdateAsync(id, request);
            return HandleResult(Result.Success());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _userService.DeleteAsync(id);
            return HandleResult(Result.Success());
        }

        [HttpPut("{id}/block")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Block(Guid id)
        {
            await _userService.BlockUserAsync(id);
            return HandleResult(Result.Success());
        }

        [HttpPut("{id}/unblock")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Unblock(Guid id)
        {
            await _userService.UnblockUserAsync(id);
            return HandleResult(Result.Success());
        }
    }
}
