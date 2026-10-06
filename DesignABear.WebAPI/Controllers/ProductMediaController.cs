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
    public class ProductMediaController : BaseController
    {
        private readonly IProductMediaService _mediaService;

        public ProductMediaController(IProductMediaService mediaService)
        {
            _mediaService = mediaService;
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(Guid id)
        {
            var media = await _mediaService.GetByIdAsync(id);
            if (media == null)
            {
                return HandleResult(Result<object>.Failure(new Error("NotFound", "Product media not found")));
            }
            return HandleResult(Result<object>.Success(media));
        }

        [HttpGet("product/{productId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByProduct(Guid productId)
        {
            var media = await _mediaService.GetByProductIdAsync(productId);
            return HandleResult(Result<object>.Success(media));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductMediaRequest request)
        {
            var newMedia = await _mediaService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = newMedia.MediaId }, Result<object>.Success(newMedia));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductMediaRequest request)
        {
            await _mediaService.UpdateAsync(id, request);
            return HandleResult(Result.Success());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediaService.DeleteAsync(id);
            return HandleResult(Result.Success());
        }
    }
}
