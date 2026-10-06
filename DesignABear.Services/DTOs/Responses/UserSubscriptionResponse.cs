namespace DesignABear.Services.DTOs.Responses
{
    public class UserSubscriptionResponse
    {
        public Guid SubscriptionId { get; set; }
        public Guid UserId { get; set; }
        public Guid DeviceId { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } = null!;
    }
}
