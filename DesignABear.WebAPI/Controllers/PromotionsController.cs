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
    public class PromotionsController : BaseController
    {
        private readonly IPromotionService _promotionService;

        public PromotionsController(IPromotionService promotionService)
        {
            _promotionService = promotionService;
        }

        [HttpGet]
        [Authorize(Roles = "1, 2")]
        public async Task<IActionResult> GetAll()
        {
            var promotions = await _promotionService.GetAllAsync();
            return HandleResult(Result<IEnumerable<PromotionResponse>>.Success(promotions));
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActivePromotions()
        {
            var promotions = await _promotionService.GetActivePromotionsAsync();
            return HandleResult(Result<IEnumerable<PromotionResponse>>.Success(promotions));
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "1, 2")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var promotion = await _promotionService.GetByIdAsync(id);
            return HandleResult(Result<PromotionResponse>.Success(promotion!));
        }

        [HttpGet("code/{code}")]
        public async Task<IActionResult> GetByCode(string code)
        {
            var promotion = await _promotionService.GetByCodeAsync(code);
            return HandleResult(Result<PromotionResponse>.Success(promotion!));
        }

        [HttpPost]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Create([FromBody] CreatePromotionRequest request)
        {
            var promotion = await _promotionService.CreateAsync(request);
            return HandleResult(Result<PromotionResponse>.Success(promotion));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePromotionRequest request)
        {
            var promotion = await _promotionService.UpdateAsync(id, request);
            return HandleResult(Result<PromotionResponse>.Success(promotion));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _promotionService.DeleteAsync(id);
            return HandleResult(Result.Success());
        }

        [HttpPost("validate")]
        public async Task<IActionResult> Validate([FromBody] ValidatePromotionRequest request)
        {
            var isValid = await _promotionService.ValidateAsync(request.Code, request.UserId, request.OrderAmount, request.ShippingAmount);
            if (!isValid)
            {
                return HandleResult(Result<PromotionResponse?>.Failure(new Error("Promotion.Invalid", "Promotion is not valid or expired.")));
            }

            var promotion = await _promotionService.GetByCodeAsync(request.Code);
            return HandleResult(Result<PromotionResponse?>.Success(promotion));
        }

        [HttpPost("apply")]
        public async Task<IActionResult> Apply([FromBody] ApplyPromotionRequest request)
        {
            var res = await _promotionService.ApplyPromotionAsync(request.Code, request.UserId, request.OrderAmount, request.ShippingAmount);
            return HandleResult(Result<PromotionApplyResponse>.Success(res));
        }
    }
}
