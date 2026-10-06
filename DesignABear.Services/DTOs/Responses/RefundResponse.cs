namespace DesignABear.Services.DTOs.Responses
{
    public class RefundResponse
    {
        public Guid RefundId { get; set; }
        public Guid OrderId { get; set; }
        public Guid RequestedBy { get; set; }
        public Guid? ApprovedBy { get; set; }
        public decimal Amount { get; set; }
        public string Reason { get; set; } = null!;
        public string[]? EvidenceUrls { get; set; }
        public string Status { get; set; } = "PENDING";
        public string? AdminNotes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
