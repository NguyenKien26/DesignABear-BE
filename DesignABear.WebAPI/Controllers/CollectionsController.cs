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
    public class CollectionsController : BaseController
    {
        private readonly ICollectionService _collectionService;

        public CollectionsController(ICollectionService collectionService)
        {
            _collectionService = collectionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var collections = await _collectionService.GetAllAsync();
            return HandleResult(Result<IEnumerable<CollectionResponse>>.Success(collections));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var collection = await _collectionService.GetByIdAsync(id);
            if (collection == null)
            {
                return HandleResult(Result<CollectionResponse>.Failure(new Error("NotFound", $"Collection with ID {id} not found.")));
            }
            return HandleResult(Result<CollectionResponse>.Success(collection));
        }

        [HttpGet("slug/{slug}")]
        public async Task<IActionResult> GetBySlug(string slug)
        {
            var collection = await _collectionService.GetBySlugAsync(slug);
            if (collection == null)
            {
                return HandleResult(Result<CollectionResponse>.Failure(new Error("NotFound", $"Collection with slug {slug} not found.")));
            }
            return HandleResult(Result<CollectionResponse>.Success(collection));
        }

        [HttpPost]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Create([FromBody] CreateCollectionRequest request)
        {
            var collection = await _collectionService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = collection.CollectionId }, Result<CollectionResponse>.Success(collection));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCollectionRequest request)
        {
            await _collectionService.UpdateAsync(id, request);
            return HandleResult(Result.Success());
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _collectionService.DeleteAsync(id);
            return HandleResult(Result.Success());
        }

        [HttpPost("matrix")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> AddProduct([FromQuery] Guid collectionId, [FromQuery] Guid productId)
        {
            await _collectionService.AddProductAsync(collectionId, productId);
            return HandleResult(Result.Success());
        }

        [HttpDelete("matrix")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> RemoveProduct([FromQuery] Guid collectionId, [FromQuery] Guid productId)
        {
            await _collectionService.RemoveProductAsync(collectionId, productId);
            return HandleResult(Result.Success());
        }
    }
}
