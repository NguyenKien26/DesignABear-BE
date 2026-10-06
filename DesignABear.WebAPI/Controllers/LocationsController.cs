using DesignABear.Services.Common;
using DesignABear.Services.DTOs.Requests;
using DesignABear.Services.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DesignABear.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocationsController : BaseController
    {
        private readonly ILocationService _locationService;

        public LocationsController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var locations = await _locationService.GetAllAsync();
            return HandleResult(Result<object>.Success(locations));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var location = await _locationService.GetByIdAsync(id);
            return HandleResult(Result<object>.Success(location));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateLocationRequest request)
        {
            var location = await _locationService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = location.LocationId }, Result<object>.Success(location));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateLocationRequest request)
        {
            await _locationService.UpdateAsync(id, request);
            return HandleResult(Result.Success());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _locationService.DeleteAsync(id);
            return HandleResult(Result.Success());
        }
    }
}
