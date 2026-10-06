namespace DesignABear.Services.DTOs.Requests
{
    public class CreatePromotionRequest
    {
        public string Code { get; set; } = null!;
        public string DiscountType { get; set; } = null!;
        public decimal Value { get; set; }
        public DateTime? StartsAt { get; set; }
        public DateTime? EndsAt { get; set; }
        public bool IsActive { get; set; } = true;
        public decimal MinOrderAmount { get; set; }
        public int? MaxUsageCount { get; set; }
        public int? MaxUsagePerUser { get; set; }
        public string? Description { get; set; }
    }
}
