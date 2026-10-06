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
    public class CategoriesController : BaseController
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var isAdmin = GetCurrentRoleId() == 1;
            var category = await _categoryService.GetByIdAsync(id, includeInactive: isAdmin);
            if (category == null)
            {
                return HandleResult(Result<CategoryResponse>.Failure(new Error("NotFound", $"Category with ID {id} not found.")));
            }
            return HandleResult(Result<CategoryResponse>.Success(category));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var isAdmin = GetCurrentRoleId() == 1;
            var categories = await _categoryService.GetAllAsync(includeInactive: isAdmin);
            return HandleResult(Result<IEnumerable<CategoryResponse>>.Success(categories));
        }

        [HttpPost]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Create([FromBody] CreateCategoryRequest request)
        {
            var newCategory = await _categoryService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = newCategory.CategoryId }, Result<CategoryResponse>.Success(newCategory));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCategoryRequest request)
        {
            await _categoryService.UpdateAsync(id, request);
            return HandleResult(Result.Success());
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _categoryService.DeleteAsync(id);
            return HandleResult(Result.Success());
        }

        [HttpPut("{id}/restore")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Restore(Guid id)
        {
            await _categoryService.RestoreAsync(id);
            return HandleResult(Result.Success());
        }
    }
}
