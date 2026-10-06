namespace DesignABear.Services.DTOs.Responses
{
    public class PromotionResponse
    {
        public Guid PromotionId { get; set; }
        public string Code { get; set; } = null!;
        public string DiscountType { get; set; } = null!;
        public decimal Value { get; set; }
        public DateTime? StartsAt { get; set; }
        public DateTime? EndsAt { get; set; }
        public bool IsActive { get; set; }
        public decimal MinOrderAmount { get; set; }
        public int? MaxUsageCount { get; set; }
        public int UsageCount { get; set; }
        public int? MaxUsagePerUser { get; set; }
        public string? Description { get; set; }
    }
}
