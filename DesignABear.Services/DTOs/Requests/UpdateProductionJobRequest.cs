namespace DesignABear.Services.DTOs.Requests
{
    public class UpdateProductionJobRequest
    {
        public string Status { get; set; } = null!;
        public Guid? AssignedUser { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
    }
}
