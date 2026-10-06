using DesignABear.Services.Common;
using DesignABear.Services.DTOs.Requests;
using DesignABear.Services.DTOs.Responses;
using DesignABear.Services.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DesignABear.Repositories.Paging;

namespace DesignABear.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : BaseController
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Retrieves a product with its details by ID.
        /// </summary>
        /// <param name="id">The product ID.</param>
        /// <returns>The product detail response.</returns>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(Guid id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null)
            {
                return HandleResult(Result<ProductDetailResponse>.Failure(new Error("NotFound", $"Product with ID {id} not found.")));
            }
            return HandleResult(Result<ProductDetailResponse>.Success(product));
        }

        [HttpGet("{id}/full")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> GetFull(Guid id)
        {
            var product = await _productService.GetFullByIdAsync(id);
            if (product == null)
            {
                return HandleResult(Result<ProductFullDetailResponse>.Failure(new Error("NotFound", $"Product with ID {id} not found.")));
            }
            return HandleResult(Result<ProductFullDetailResponse>.Success(product));
        }

        /// <summary>
        /// Retrieves a product with its details by Slug.
        /// </summary>
        /// <param name="slug">The product slug.</param>
        /// <returns>The product detail response.</returns>
        [HttpGet("slug/{slug}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetBySlug(string slug)
        {
            var product = await _productService.GetBySlugAsync(slug);
            if (product == null)
            {
                return HandleResult(Result<ProductDetailResponse>.Failure(new Error("NotFound", $"Product with slug '{slug}' not found.")));
            }
            return HandleResult(Result<ProductDetailResponse>.Success(product));
        }

        /// <summary>
        /// Retrieves all active products.
        /// </summary>
        /// <param name="pageIndex">The current page (default is 1).</param>
        /// <param name="pageSize">The number of items per page (default is 10).</param>
        /// <param name="sortBy">Sorting criteria (price_asc, price_desc, sales_desc, views_desc, rating_desc, newest).</param>
        /// <returns>A paginated list of products.</returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10, [FromQuery] string? sortBy = null)
        {
            var roleId = GetCurrentRoleId();
            var pagedProducts = await _productService.GetProductsAsync(pageIndex, pageSize, sortBy, roleId);
            return HandleResult(Result<PagedResult<ProductListResponse>>.Success(pagedProducts));
        }

        /// <summary>
        /// Retrieves the most viewed products.
        /// </summary>
        /// <param name="count">Number of products to retrieve (default 20).</param>
        /// <returns>A list of top products.</returns>
        [HttpGet("top")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTopProducts([FromQuery] int count = 20)
        {
            var products = await _productService.GetTopProductsAsync(count);
            return HandleResult(Result<List<ProductListResponse>>.Success(products));
        }

        [HttpPut("{id}/total-sales")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> UpdateTotalSales(Guid id, [FromBody] int totalSales)
        {
            await _productService.UpdateTotalSalesAsync(id, totalSales);
            return HandleResult(Result.Success());
        }

        [HttpPost("{id}/view")]
        [AllowAnonymous]
        public async Task<IActionResult> IncrementView(Guid id)
        {
            await _productService.IncrementViewCountAsync(id);
            return HandleResult(Result.Success());
        }

        /// <summary>
        /// Creates a new product.
        /// </summary>
        /// <param name="request">The product creation details.</param>
        /// <returns>The newly created product.</returns>
        [HttpPost]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Create([FromBody] CreateProductRequest request)
        {
            var newProduct = await _productService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = newProduct.ProductId }, Result<ProductDetailResponse>.Success(newProduct));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductRequest request)
        {
            await _productService.UpdateAsync(id, request);
            return HandleResult(Result.Success());
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _productService.DeleteAsync(id);
            return HandleResult(Result.Success());
        }
    }
}
