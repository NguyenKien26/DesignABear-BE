namespace DesignABear.Services.DTOs.Requests
{
    public class HandoverProductionPartRequest
    {
        public string PartType { get; set; } = null!;
        public List<string> PhotoUrls { get; set; } = new();
        public string? Note { get; set; }
    }
}