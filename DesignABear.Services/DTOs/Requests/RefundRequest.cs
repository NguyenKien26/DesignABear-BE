namespace DesignABear.Services.DTOs.Requests
{
    public class RefundRequest
    {
        public Guid OrderId { get; set; }
        public decimal? Amount { get; set; }
        public string Reason { get; set; } = null!;
        public List<string>? EvidenceUrls { get; set; }
    }
}
