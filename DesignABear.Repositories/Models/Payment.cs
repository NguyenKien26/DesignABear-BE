using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DesignABear.Repositories.Models
{
    [Table("payments")]
    public class Payment
    {
        [Key]
        [Column("payment_id")]
        public Guid PaymentId { get; set; }

        [Required]
        [Column("order_id")]
        public Guid OrderId { get; set; }

        [MaxLength(30)]
        [Column("provider")]
        public string? Provider { get; set; }

        [MaxLength(30)]
        [Column("status")]
        public string? Status { get; set; }

        [Required]
        [Column("amount")]
        public decimal Amount { get; set; }

        [MaxLength(10)]
        [Column("currency")]
        public string Currency { get; set; } = "VND";

        [MaxLength(255)]
        [Column("external_txn_id")]
        public string? ExternalTxnId { get; set; }

        [Column("payment_url")]
        public string? PaymentUrl { get; set; }

        [Column("paid_at")]
        public DateTime? PaidAt { get; set; }

        [Column("webhook_raw", TypeName = "jsonb")]
        public string? WebhookRaw { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("OrderId")]
        public Order Order { get; set; } = null!;
    }
}
