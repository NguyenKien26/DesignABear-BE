namespace DesignABear.Services.DTOs.Requests
{
    public class CreatePaymentRequest
    {
        public Guid OrderId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = null!;
        public string? TransactionId { get; set; }
    }
}
