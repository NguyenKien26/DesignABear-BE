using DesignABear.Services.Common;
using DesignABear.Services.DTOs.Requests;
using DesignABear.Services.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DesignABear.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonalizationGroupsController : BaseController
    {
        private readonly IPersonalizationGroupService _groupService;

        public PersonalizationGroupsController(IPersonalizationGroupService groupService)
        {
            _groupService = groupService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var groups = await _groupService.GetAllAsync();
            return HandleResult(Result<object>.Success(groups));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var group = await _groupService.GetByIdAsync(id);
            return HandleResult(Result<object>.Success(group));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePersonalizationGroupRequest request)
        {
            var group = await _groupService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = group.GroupId }, Result<object>.Success(group));
        }

        [HttpPut("{id}")]

        public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePersonalizationGroupRequest request)
        {
            await _groupService.UpdateAsync(id, request);
            return HandleResult(Result.Success());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _groupService.DeleteAsync(id);
            return HandleResult(Result.Success());
        }
    }
}
