namespace DesignABear.Services.DTOs.Requests
{
    public class UpdateFulfillmentRequest
    {
        public string Status { get; set; } = null!;
        public string? TrackingNumber { get; set; }
        public string? Carrier { get; set; }
        public DateTime? ShippedAt { get; set; }
        public DateTime? DeliveredAt { get; set; }
    }
}
