namespace DesignABear.Services.DTOs.Requests
{
    public class ValidatePromotionRequest
    {
        public string Code { get; set; } = null!;
        public Guid? UserId { get; set; }
        public decimal? OrderAmount { get; set; }
        public decimal? ShippingAmount { get; set; }
    }
}
