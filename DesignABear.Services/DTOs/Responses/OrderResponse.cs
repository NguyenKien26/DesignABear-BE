namespace DesignABear.Services.DTOs.Responses
{
    public class OrderResponse
    {
        public Guid OrderId { get; set; }
        public string OrderNumber { get; set; } = null!;
        public Guid? UserId { get; set; }
        public Guid? ShippingAddressId { get; set; }
        public Guid? BillingAddressId { get; set; }
        public string Status { get; set; } = "PENDING";
        public string Currency { get; set; } = "VND";
        public decimal Subtotal { get; set; }
        public decimal DiscountTotal { get; set; }
        public decimal TaxTotal { get; set; }
        public decimal ShippingTotal { get; set; }
        public decimal GrandTotal { get; set; }
        public bool IsPaid { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        public IEnumerable<OrderItemResponse> OrderItems { get; set; } = new List<OrderItemResponse>();
    }
}
