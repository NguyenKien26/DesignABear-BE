namespace DesignABear.Services.DTOs.Requests
{
    public class UpdateProductionPartStatusRequest
    {
        public string PartType { get; set; } = null!;
        public string Status { get; set; } = null!;
    }
}