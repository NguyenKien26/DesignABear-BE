using DesignABear.Services.Common;
using DesignABear.Services.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DesignABear.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoriesController : BaseController
    {
        private readonly IInventoryService _inventoryService;

        public InventoriesController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        [HttpGet("location/{locationId}/identity/{identityId}")]
        public async Task<IActionResult> GetAsync(Guid locationId, Guid identityId, [FromQuery] bool isAccessory = false)
        {
            var inventory = await _inventoryService.GetAsync(locationId, identityId, isAccessory);
            return HandleResult(Result<object>.Success(inventory));
        }

        [HttpGet("product/{productId}")]
        public async Task<IActionResult> GetByProduct(Guid productId)
        {
            var inventories = await _inventoryService.GetByProductIdAsync(productId);
            return HandleResult(Result<object>.Success(inventories));
        }

        [HttpGet("accessory/{accessoryId}")]
        public async Task<IActionResult> GetByAccessory(Guid accessoryId)
        {
            var inventories = await _inventoryService.GetByAccessoryIdAsync(accessoryId);
            return HandleResult(Result<object>.Success(inventories));
        }

        [HttpGet("total")]
        public async Task<IActionResult> GetTotalAvailable([FromQuery] Guid identityId, [FromQuery] bool isAccessory = false)
        {
            var total = await _inventoryService.GetTotalAvailableAsync(identityId, isAccessory);
            return HandleResult(Result<object>.Success(new { TotalAvailable = total }));
        }

        [HttpPost("adjust")]
        public async Task<IActionResult> AdjustStock([FromQuery] Guid identityId, [FromQuery] bool isAccessory, [FromQuery] int delta, [FromQuery] Guid? locationId)
        {
            await _inventoryService.AdjustStockByIdAsync(identityId, isAccessory, delta, locationId);
            return HandleResult(Result.Success());
        }

        [HttpPost("variant/adjust")]
        public async Task<IActionResult> AdjustVariantStock([FromQuery] Guid id, [FromQuery] int delta, [FromQuery] Guid? locationId)
        {
            await _inventoryService.AdjustStockByIdAsync(id, false, delta, locationId);
            return HandleResult(Result.Success());
        }

        [HttpPost("accessory/adjust")]
        public async Task<IActionResult> AdjustAccessoryStock([FromQuery] Guid id, [FromQuery] int delta, [FromQuery] Guid? locationId)
        {
            await _inventoryService.AdjustStockByIdAsync(id, true, delta, locationId);
            return HandleResult(Result.Success());
        }

        [HttpPost("reserve")]
        public async Task<IActionResult> ReserveStock([FromQuery] Guid identityId, [FromQuery] bool isAccessory, [FromQuery] int quantity, [FromQuery] Guid locationId)
        {
            await _inventoryService.ReserveStockAsync(identityId, isAccessory, quantity, locationId);
            return HandleResult(Result.Success());
        }

        [HttpPost("release")]
        public async Task<IActionResult> ReleaseReservation([FromQuery] Guid identityId, [FromQuery] bool isAccessory, [FromQuery] int quantity, [FromQuery] Guid locationId)
        {
            await _inventoryService.ReleaseReservationAsync(identityId, isAccessory, quantity, locationId);
            return HandleResult(Result.Success());
        }
    }
}
