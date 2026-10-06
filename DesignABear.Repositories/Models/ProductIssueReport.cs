using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DesignABear.Repositories.Models
{
    [Table("product_issue_reports")]
    public class ProductIssueReport
    {
        [Key]
        [Column("report_id")]
        public Guid ReportId { get; set; }

        [Required]
        [Column("order_item_id")]
        public Guid OrderItemId { get; set; }

        [Required]
        [Column("product_id")]
        public Guid ProductId { get; set; }

        [Required]
        [Column("user_id")]
        public Guid UserId { get; set; }

        [Column("assigned_staff_id")]
        public Guid? AssignedStaffId { get; set; }

        [Required]
        [MaxLength(20)]
        [Column("status")]
        public string Status { get; set; } = "PENDING";

        [Column("resolution")]
        public string? Resolution { get; set; }

        [Column("request_refund")]
        public bool RequestRefund { get; set; } = false;

        [Required]
        [Column("description")]
        public string Description { get; set; } = null!;

        [Column("staff_notes")]
        public string? StaffNotes { get; set; }

        [Column("repair_notes")]
        public string? RepairNotes { get; set; }

        [Column("resolved_at")]
        public DateTime? ResolvedAt { get; set; }

        [Column("evidence_urls")]
        public string? EvidenceUrls { get; set; } // JSON string

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("OrderItemId")]
        public virtual OrderItem OrderItem { get; set; } = null!;

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; } = null!;

        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;

        [ForeignKey("AssignedStaffId")]
        public virtual User? AssignedStaff { get; set; }
    }
}
