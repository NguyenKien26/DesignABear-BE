namespace DesignABear.Services.DTOs.Requests
{
    public class UpdatePaymentRequest
    {
        public string Status { get; set; } = null!;
        public string? TransactionId { get; set; }
    }
}
