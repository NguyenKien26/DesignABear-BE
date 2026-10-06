using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DesignABear.Repositories.Models
{
    [Table("order_refunds")]
    public class OrderRefund
    {
        [Key]
        [Column("refund_id")]
        public Guid RefundId { get; set; }

        [Required]
        [Column("order_id")]
        public Guid OrderId { get; set; }

        [Required]
        [Column("requested_by")]
        public Guid RequestedBy { get; set; }

        [Column("approved_by")]
        public Guid? ApprovedBy { get; set; }

        [Required]
        [Column("amount")]
        public decimal Amount { get; set; }

        [Required]
        [MaxLength(500)]
        [Column("reason")]
        public string Reason { get; set; } = null!;

        [MaxLength(1000)]
        [Column("evidence_urls")] // JSON array of image URLs
        public string? EvidenceUrls { get; set; }

        [Required]
        [MaxLength(20)]
        [Column("status")] // PENDING, APPROVED, REJECTED, COMPLETED
        public string Status { get; set; } = "PENDING";

        [Column("is_defect_verified")]
        public bool IsDefectVerified { get; set; } = false;

        [Column("admin_notes")]
        public string? AdminNotes { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("OrderId")]
        public Order Order { get; set; } = null!;

        [ForeignKey("RequestedBy")]
        public User Requester { get; set; } = null!;

        [ForeignKey("ApprovedBy")]
        public User? Approver { get; set; }
    }
}
