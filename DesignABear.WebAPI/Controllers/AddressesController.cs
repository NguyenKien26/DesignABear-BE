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
    public class AddressesController : BaseController
    {
        private readonly IAddressService _addressService;

        public AddressesController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var address = await _addressService.GetByIdAsync(id);
            if (address == null)
            {
                return HandleResult(Result<AddressResponse>.Failure(new Error("NotFound", $"Address with ID {id} not found.")));
            }
            return HandleResult(Result<AddressResponse>.Success(address));
        }

        [HttpGet("my-addresses")]
        public async Task<IActionResult> GetMyAddresses()
        {
            var userIdClaim = User.FindFirst("id")?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                return HandleResult(Result<IEnumerable<AddressResponse>>.Failure(new Error("Unauthorized", "User ID not found in token.")));
            }
            
            var addresses = await _addressService.GetByUserIdAsync(userId);
            return HandleResult(Result<IEnumerable<AddressResponse>>.Success(addresses));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAddressRequest request)
        {
            var userIdClaim = User.FindFirst("id")?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                return HandleResult(Result<AddressResponse>.Failure(new Error("Unauthorized", "User ID not found in token.")));
            }

            request.UserId = userId;

            var newAddress = await _addressService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = newAddress.AddressId }, Result<AddressResponse>.Success(newAddress));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateAddressRequest request)
        {
            await _addressService.UpdateAsync(id, request);
            return HandleResult(Result.Success());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var userIdClaim = User.FindFirst("id")?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                return HandleResult(Result.Failure(new Error("Unauthorized", "User ID not found in token.")));
            }

            var address = await _addressService.GetByIdAsync(id);
            if (address == null) return HandleResult(Result.Success()); // idempotent
            if (address.UserId != userId) return Forbid(); // Should not delete others' addresses

            await _addressService.DeleteAsync(id);
            return HandleResult(Result.Success());
        }
    }
}
