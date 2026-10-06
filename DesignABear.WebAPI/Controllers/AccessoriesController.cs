using DesignABear.Services.DTOs.Requests;
using DesignABear.Services.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace DesignABear.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccessoriesController : BaseController
    {
        private readonly IAccessoryService _accessoryService;

        public AccessoriesController(IAccessoryService accessoryService)
        {
            _accessoryService = accessoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _accessoryService.GetAllAsync();
            return HandleResult(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _accessoryService.GetByIdAsync(id);
            return HandleResult(result);
        }

        [HttpGet("product/{productId}")]
        public async Task<IActionResult> GetByProduct(Guid productId)
        {
            var result = await _accessoryService.GetByProductIdAsync(productId);
            return HandleResult(result);
        }

        [HttpGet("smart-chip")]
        public async Task<IActionResult> GetSmartChip()
        {
            var result = await _accessoryService.GetSmartChipAsync();
            return HandleResult(result);
        }

        [HttpPost]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Create([FromBody] CreateAccessoryRequest request)
        {
            var result = await _accessoryService.CreateAsync(request);
            if (!result.IsSuccess) return HandleResult(result);
            
            return CreatedAtAction(nameof(GetById), new { id = result.Value.AccessoryId }, result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateAccessoryRequest request)
        {
            var result = await _accessoryService.UpdateAsync(id, request);
            return HandleResult(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _accessoryService.DeleteAsync(id);
            return HandleResult(result);
        }

        [HttpPost("matrix")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> AddToProduct([FromQuery] Guid productId, [FromQuery] Guid accessoryId)
        {
            var result = await _accessoryService.AddToProductAsync(productId, accessoryId);
            return HandleResult(result);
        }

        [HttpDelete("matrix")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> RemoveFromProduct([FromQuery] Guid productId, [FromQuery] Guid accessoryId)
        {
            var result = await _accessoryService.RemoveFromProductAsync(productId, accessoryId);
            return HandleResult(result);
        }
    }
}
