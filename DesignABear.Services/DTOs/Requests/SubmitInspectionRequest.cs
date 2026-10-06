namespace DesignABear.Services.DTOs.Requests
{
    public class SubmitInspectionRequest
    {
        public int Score { get; set; }
        public string? Notes { get; set; }
        public bool IsApproved { get; set; }

        // ESP_CORE | PLUSH_SHELL | ALL (optional, default ALL khi reject)
        public string? RejectedPartType { get; set; }
    }
}
