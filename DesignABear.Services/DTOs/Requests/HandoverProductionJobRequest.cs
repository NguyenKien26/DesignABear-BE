namespace DesignABear.Services.DTOs.Requests
{
    public class HandoverProductionJobRequest
    {
        public List<string> PhotoUrls { get; set; } = new();
        public string? Note { get; set; }
        public string? Metadata { get; set; }
    }
}
