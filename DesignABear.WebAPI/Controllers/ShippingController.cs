using DesignABear.Services.Common;
using DesignABear.Services.DTOs.Requests;
using DesignABear.Services.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DesignABear.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShippingController : BaseController
    {
        private readonly IGhtkShippingService _ghtkShippingService;

        public ShippingController(IGhtkShippingService ghtkShippingService)
        {
            _ghtkShippingService = ghtkShippingService;
        }

        [HttpPost("ghtk/fee")]
        [AllowAnonymous]
        public async Task<IActionResult> CalculateGhtkFee([FromBody] CalculateGhtkFeeRequest request, CancellationToken cancellationToken)
        {
            var result = await _ghtkShippingService.GetShippingFeeAsync(request, cancellationToken);
            return HandleResult(result);
        }

        [HttpPost("calculate-fee")]
        public async Task<IActionResult> CalculateFee([FromQuery] Guid cartId, [FromQuery] Guid addressId, [FromQuery] string? transport, CancellationToken cancellationToken)
        {
            var result = await _ghtkShippingService.CalculateFeeForCartAsync(cartId, addressId, transport, cancellationToken);
            return HandleResult(result);
        }

        [HttpPost("calculate-order-fee/{orderId}")]
        public async Task<IActionResult> CalculateOrderFee(Guid orderId, [FromQuery] string? transport, CancellationToken cancellationToken)
        {
            var result = await _ghtkShippingService.CalculateFeeForOrderAsync(orderId, transport, cancellationToken);
            return HandleResult(result);
        }

        [HttpPost("ghtk/orders/express")]
        public async Task<IActionResult> SubmitExpressOrder([FromBody] SubmitGhtkExpressOrderRequest request, CancellationToken cancellationToken)
        {
            var result = await _ghtkShippingService.SubmitExpressOrderAsync(request, cancellationToken);
            return HandleResult(result);
        }

        [HttpGet("ghtk/orders/{label}/print-label")]
        public async Task<IActionResult> PrintLabel(string label, CancellationToken cancellationToken)
        {
            var result = await _ghtkShippingService.DownloadPrintLabelAsync(label, cancellationToken);
            if (result.IsFailure || result.Value == null)
            {
                return HandleResult(result);
            }

            return File(result.Value.Content, result.Value.ContentType, result.Value.FileName);
        }

        [HttpGet("ghtk/orders/{label}/tracking-status")]
        [AllowAnonymous]
        public async Task<IActionResult> TrackingStatus(string label, CancellationToken cancellationToken)
        {
            var result = await _ghtkShippingService.GetTrackingStatusAsync(label, cancellationToken);
            return HandleResult(result);
        }
    }
}