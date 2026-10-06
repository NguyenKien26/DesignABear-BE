using System.ComponentModel.DataAnnotations.Schema;

namespace DesignABear.Services.DTOs.Responses
{
    public class PaymentResponse
    {
        public Guid PaymentId { get; set; }
        public Guid OrderId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = null!;
        public string Status { get; set; } = "PENDING";
        public string? TransactionId { get; set; }
        public DateTime? PaidAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
