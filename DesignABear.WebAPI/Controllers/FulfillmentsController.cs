using DesignABear.Services.Common;
using DesignABear.Services.DTOs.Requests;
using DesignABear.Services.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DesignABear.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FulfillmentsController : BaseController
    {
        private readonly IFulfillmentService _fulfillmentService;

        public FulfillmentsController(IFulfillmentService fulfillmentService)
        {
            _fulfillmentService = fulfillmentService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var fulfillment = await _fulfillmentService.GetByIdAsync(id);
            return HandleResult(Result<object>.Success(fulfillment));
        }

        [HttpGet("order/{orderId}")]
        public async Task<IActionResult> GetByOrder(Guid orderId)
        {
            var fulfillments = await _fulfillmentService.GetByOrderIdAsync(orderId);
            return HandleResult(Result<object>.Success(fulfillments));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateFulfillmentRequest request)
        {
            var fulfillment = await _fulfillmentService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = fulfillment.FulfillmentId }, Result<object>.Success(fulfillment));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateFulfillmentRequest request)
        {
            await _fulfillmentService.UpdateAsync(id, request);
            return HandleResult(Result.Success());
        }
    }
}
