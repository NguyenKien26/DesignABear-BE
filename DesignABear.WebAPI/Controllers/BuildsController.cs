using DesignABear.Services.Common;
using DesignABear.Services.DTOs.Requests;
using DesignABear.Services.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DesignABear.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BuildsController : BaseController
    {
        private readonly IBuildService _buildService;

        public BuildsController(IBuildService buildService)
        {
            _buildService = buildService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var build = await _buildService.GetByIdAsync(id);
            if (build == null)
            {
                return HandleResult(Result<object>.Failure(new Error("NotFound", $"Build with ID {id} not found.")));
            }
            return HandleResult(Result<object>.Success(build));
        }

        [HttpGet("{id}/components")]
        public async Task<IActionResult> GetWithComponents(Guid id)
        {
            var build = await _buildService.GetWithComponentsAsync(id);
            if (build == null)
            {
                return HandleResult(Result<object>.Failure(new Error("NotFound", $"Build with ID {id} not found.")));
            }
            return HandleResult(Result<object>.Success(build));
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUser(Guid userId)
        {
            var builds = await _buildService.GetByUserIdAsync(userId);
            return HandleResult(Result<IEnumerable<object>>.Success(builds));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBuildRequest request)
        {
            var build = await _buildService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = build.BuildId }, Result<object>.Success(build));
        }

        [HttpPost("components")]
        public async Task<IActionResult> AddComponent([FromBody] CreateBuildComponentRequest request)
        {
            var component = await _buildService.AddComponentAsync(request);
            return HandleResult(Result<object>.Success(component));
        }

        [HttpDelete("components/{id}")]
        public async Task<IActionResult> RemoveComponent(Guid id)
        {
            await _buildService.RemoveComponentAsync(id);
            return HandleResult(Result.Success());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateBuildRequest request)
        {
            await _buildService.UpdateAsync(id, request);
            return HandleResult(Result.Success());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _buildService.DeleteAsync(id);
            return HandleResult(Result.Success());
        }

        [HttpPost("{id}/calculate-price")]
        public async Task<IActionResult> CalculatePrice(Guid id)
        {
            var price = await _buildService.CalculatePriceAsync(id);
            return HandleResult(Result<object>.Success(new { Price = price }));
        }
    }
}
