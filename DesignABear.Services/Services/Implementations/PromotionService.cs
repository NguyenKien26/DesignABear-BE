using AutoMapper;
using DesignABear.Repositories.Models;
using DesignABear.Repositories.UnitOfWork;
using DesignABear.Services.DTOs.Requests;
using DesignABear.Services.DTOs.Responses;
using DesignABear.Services.Exceptions;
using DesignABear.Services.Services.Interfaces;

namespace DesignABear.Services.Services.Implementations
{
    public class PromotionService : IPromotionService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public PromotionService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PromotionResponse>> GetAllAsync()
        {
            var promotions = await _uow.Promotions.GetAllAsync();
            return _mapper.Map<IEnumerable<PromotionResponse>>(promotions);
        }

        public async Task<IEnumerable<PromotionResponse>> GetActivePromotionsAsync()
        {
            var promotions = await _uow.Promotions.GetActivePromotionsAsync();
            return _mapper.Map<IEnumerable<PromotionResponse>>(promotions);
        }

        public async Task<PromotionResponse?> GetByIdAsync(Guid promotionId)
        {
            var promotion = await _uow.Promotions.GetByIdAsync(promotionId);
            if (promotion == null) throw new NotFoundException("Promotion", promotionId);
            return _mapper.Map<PromotionResponse>(promotion);
        }

        public async Task<PromotionResponse?> GetByCodeAsync(string code)
        {
            var promotion = await _uow.Promotions.GetByCodeAsync(code);
            if (promotion == null) throw new NotFoundException("Promotion", code);
            return _mapper.Map<PromotionResponse>(promotion);
        }

        public async Task<PromotionResponse> CreateAsync(CreatePromotionRequest request)
        {
            var promotion = _mapper.Map<Promotion>(request);
            promotion.PromotionId = Guid.NewGuid();
            promotion.StartsAt = NormalizeToUtc(promotion.StartsAt);
            promotion.EndsAt = NormalizeToUtc(promotion.EndsAt);
            
            await _uow.Promotions.AddAsync(promotion);
            await _uow.SaveChangesAsync();
            
            return _mapper.Map<PromotionResponse>(promotion);
        }

        public async Task<PromotionResponse> UpdateAsync(Guid promotionId, UpdatePromotionRequest request)
        {
            var promotion = await _uow.Promotions.GetByIdAsync(promotionId);
            if (promotion == null) throw new NotFoundException("Promotion", promotionId);
            
            _mapper.Map(request, promotion);
            promotion.StartsAt = NormalizeToUtc(promotion.StartsAt);
            promotion.EndsAt = NormalizeToUtc(promotion.EndsAt);
            
            _uow.Promotions.Update(promotion);
            await _uow.SaveChangesAsync();

            return _mapper.Map<PromotionResponse>(promotion);
        }

        public async Task DeleteAsync(Guid promotionId)
        {
            var promo = await _uow.Promotions.GetByIdAsync(promotionId);
            if (promo is null) throw new NotFoundException("Promotion", promotionId);

            _uow.Promotions.Remove(promo);
            await _uow.SaveChangesAsync();
        }

        public async Task<bool> ValidateAsync(string code, Guid? userId = null, decimal? orderAmount = null, decimal? shippingAmount = null)
        {
            var promo = await _uow.Promotions.GetByCodeAsync(code);
            if (promo is null || !promo.IsActive) return false;

            var now = DateTime.UtcNow;
            if ((promo.StartsAt != null && promo.StartsAt > now) ||
                (promo.EndsAt != null && promo.EndsAt < now))
            {
                return false;
            }

            // Check minimum order amount
            if (orderAmount.HasValue && orderAmount.Value < promo.MinOrderAmount)
            {
                return false;
            }

            // Check total usage count
            if (promo.MaxUsageCount.HasValue && promo.UsageCount >= promo.MaxUsageCount.Value)
            {
                return false;
            }

            // Check per-user usage count
            if (userId.HasValue && promo.MaxUsagePerUser.HasValue)
            {
                var userUsageCount = await _uow.OrderPromotions.CountUserUsageAsync(userId.Value, promo.PromotionId);
                if (userUsageCount >= promo.MaxUsagePerUser.Value)
                {
                    return false;
                }
            }

            return true;
        }

        public async Task<PromotionApplyResponse> ApplyPromotionAsync(string code, Guid userId, decimal orderAmount, decimal shippingAmount)
        {
            var promo = await _uow.Promotions.GetByCodeAsync(code);
            if (promo == null) throw new NotFoundException("Promotion", code);

            if (!await ValidateAsync(code, userId, orderAmount, shippingAmount))
            {
                throw new BadRequestException("Promotion is not valid for this order.");
            }

            decimal productDiscount = 0;
            decimal shippingDiscount = 0;

            switch (promo.DiscountType)
            {
                case "PERCENTAGE":
                    productDiscount = orderAmount * (promo.Value / 100);
                    break;
                case "FIXED":
                    productDiscount = promo.Value;
                    break;
                case "FIXED_PRICE":
                    // Final price for order is fixed at promo.Value. 
                    // Discount is the difference.
                    productDiscount = orderAmount - promo.Value;
                    break;
                case "SHIPPING":
                    // Percentage discount on shipping fee (e.g., Value=100 for Free Ship)
                    shippingDiscount = shippingAmount * (promo.Value / 100);
                    break;
                case "SHIPPING_FIXED":
                    // Fixed amount discount on shipping fee
                    shippingDiscount = promo.Value;
                    break;
                default:
                    throw new BadRequestException($"Unknown discount type: {promo.DiscountType}");
            }

            // Clamp results
            productDiscount = Math.Clamp(productDiscount, 0, orderAmount);
            shippingDiscount = Math.Clamp(shippingDiscount, 0, shippingAmount);

            return new PromotionApplyResponse
            {
                ProductDiscount = productDiscount,
                ShippingDiscount = shippingDiscount,
                TotalDiscount = productDiscount + shippingDiscount,
                DiscountType = promo.DiscountType
            };
        }

        public async Task UsePromotionAsync(string code, Guid userId, Guid orderId, decimal discountAmount)
        {
            var promo = await _uow.Promotions.GetByCodeAsync(code);
            if (promo == null) throw new NotFoundException("Promotion", code);

            // Double check validation before recording usage
            if (!await ValidateAsync(code, userId))
            {
                throw new BadRequestException("Promotion is no longer valid.");
            }

            // Increment usage
            promo.UsageCount++;
            _uow.Promotions.Update(promo);

            // Record history
            await _uow.OrderPromotions.AddAsync(new OrderPromotion
            {
                OrderId = orderId,
                PromotionId = promo.PromotionId,
                DiscountAmount = discountAmount
            });

            await _uow.SaveChangesAsync();
        }

        private static DateTime? NormalizeToUtc(DateTime? value)
        {
            if (!value.HasValue)
            {
                return null;
            }

            return value.Value.Kind switch
            {
                DateTimeKind.Utc => value,
                DateTimeKind.Local => value.Value.ToUniversalTime(),
                _ => DateTime.SpecifyKind(value.Value, DateTimeKind.Utc)
            };
        }
    }
}
