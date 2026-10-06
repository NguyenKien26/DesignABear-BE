using DesignABear.Services.Common;
using DesignABear.Services.DTOs.Requests;
using DesignABear.Services.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DesignABear.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonalizationRulesController : BaseController
    {
        private readonly IPersonalizationRuleService _ruleService;

        public PersonalizationRulesController(IPersonalizationRuleService ruleService)
        {
            _ruleService = ruleService;
        }
        
        [HttpGet]
        [Authorize(Roles = "1,2")]
        public async Task<IActionResult> GetAll([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10, [FromQuery] Guid? baseProductId = null, [FromQuery] Guid? addonProductId = null, [FromQuery] Guid? groupId = null)
        {
            var pagedResult = await _ruleService.GetPagedRulesAsync(pageIndex, pageSize, baseProductId, addonProductId, groupId);
            return HandleResult(Result<object>.Success(pagedResult));
        }

        [HttpGet("product/{baseProductId}")]
        public async Task<IActionResult> GetByBaseProduct(Guid baseProductId)
        {
            var rules = await _ruleService.GetByBaseProductAsync(baseProductId);
            return HandleResult(Result<object>.Success(rules));
        }

        [HttpGet("product/{baseProductId}/active")]
        public async Task<IActionResult> GetActiveRules(Guid baseProductId)
        {
            var rules = await _ruleService.GetActiveRulesForProductAsync(baseProductId);
            return HandleResult(Result<object>.Success(rules));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var rule = await _ruleService.GetByIdAsync(id);
            return HandleResult(Result<object>.Success(rule));
        }

        [HttpPost]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Create([FromBody] CreatePersonalizationRuleRequest request)
        {
            var rule = await _ruleService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = rule.RuleId }, Result<object>.Success(rule));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePersonalizationRuleRequest request)
        {
            await _ruleService.UpdateAsync(id, request);
            return HandleResult(Result.Success());
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _ruleService.DeleteAsync(id);
            return HandleResult(Result.Success());
        }

        [HttpPost("calculate-price")]
        public async Task<IActionResult> CalculatePrice([FromBody] CustomizeDesignRequest request)
        {
            var response = await _ruleService.CalculatePriceAsync(request);
            return HandleResult(Result<object>.Success(response));
        }

        [HttpPost("validate-design")]
        public async Task<IActionResult> ValidateDesign([FromBody] CustomizeDesignRequest request)
        {
            var response = await _ruleService.ValidateDesignAsync(request);
            if (!response.IsValid) return HandleResult(Result<object>.Failure(new Error("BadRequest", "Invalid design")));
            
            return HandleResult(Result<object>.Success(response));
        }
    }
}
