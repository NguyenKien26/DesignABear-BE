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
    [Authorize(Roles = "1")]
    public class RolesController : BaseController
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var role = await _roleService.GetByIdAsync(id);
            if (role == null)
            {
                return HandleResult(Result<RoleResponse>.Failure(new Error("NotFound", $"Role with ID {id} not found.")));
            }
            return HandleResult(Result<RoleResponse>.Success(role));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var roles = await _roleService.GetAllAsync();
            return HandleResult(Result<IEnumerable<RoleResponse>>.Success(roles));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRoleRequest request)
        {
            var newRole = await _roleService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = newRole.RoleId }, Result<RoleResponse>.Success(newRole));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateRoleRequest request)
        {
            await _roleService.UpdateAsync(id, request);
            return HandleResult(Result.Success());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _roleService.DeleteAsync(id);
            return HandleResult(Result.Success());
        }
    }
}
