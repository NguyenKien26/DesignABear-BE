namespace DesignABear.Services.DTOs.Requests
{
    public class CreateOrderRequest
    {
        public Guid? UserId { get; set; }
        public Guid? ShippingAddressId { get; set; }
        public Guid? BillingAddressId { get; set; }
        public string Currency { get; set; } = "VND";
        public decimal Subtotal { get; set; }
        public decimal DiscountTotal { get; set; }
        public decimal TaxTotal { get; set; }
        public decimal ShippingTotal { get; set; }
        public decimal GrandTotal { get; set; }
        public string? Notes { get; set; }
        public List<string>? PromoCodes { get; set; }
    }
}
