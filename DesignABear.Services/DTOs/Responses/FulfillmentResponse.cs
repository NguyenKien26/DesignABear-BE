namespace DesignABear.Services.DTOs.Responses
{
    public class FulfillmentResponse
    {
        public Guid FulfillmentId { get; set; }
        public Guid OrderId { get; set; }
        public string Status { get; set; } = "PENDING";
        public string? TrackingNumber { get; set; }
        public string? Carrier { get; set; }
        public DateTime? ShippedAt { get; set; }
        public DateTime? DeliveredAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
