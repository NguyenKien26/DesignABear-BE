namespace DesignABear.Services.DTOs.Requests
{
    public class CreateUserSubscriptionRequest
    {
        public Guid UserId { get; set; }
        public Guid DeviceId { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } = "Active";
    }

    public class UpdateUserSubscriptionRequest
    {
        public DateTime EndDate { get; set; }
        public string Status { get; set; } = null!;
    }
}
