namespace DesignABear.Services.DTOs.Responses
{
    public class PromotionApplyResponse
    {
        public decimal ProductDiscount { get; set; }
        public decimal ShippingDiscount { get; set; }
        public decimal TotalDiscount { get; set; }
        public string DiscountType { get; set; } = null!;
    }
}
