namespace DesignABear.Services.DTOs.Requests
{
    public class UpdateOrderRequest
    {
        public string Status { get; set; } = null!;
        public string? Notes { get; set; }
    }
}
