namespace DesignABear.Services.DTOs.Requests
{
    public class CreateFulfillmentRequest
    {
        public Guid OrderId { get; set; }
        public string? TrackingNumber { get; set; }
        public string? Carrier { get; set; }
    }
}
