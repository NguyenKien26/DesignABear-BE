using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DesignABear.Repositories.Models
{
    [Table("production_jobs")]
    public class ProductionJob
    {
        [Key]
        [Column("job_id")]
        public Guid JobId { get; set; }

        [Required]
        [Column("order_item_id")]
        public Guid OrderItemId { get; set; }

        [Column("build_id")]
        public Guid? BuildId { get; set; }

        [Column("technician_id")]
        public Guid? TechnicianId { get; set; }

        [MaxLength(30)]
        [Column("status")]
        public string Status { get; set; } = "IN_QUEUE";

        [Column("has_smart_chip")]
        public bool HasSmartChip { get; set; } = false;

        [MaxLength(100)]
        [Column("serial_number")]
        public string? SerialNumber { get; set; }

        [Column("material_consumption_gram")]
        public float? MaterialConsumptionGram { get; set; }

        [Column("actual_production_cost")]
        public decimal? ActualProductionCost { get; set; }

        [Column("started_at")]
        public DateTime? StartedAt { get; set; }

        [Column("finished_at")]
        public DateTime? FinishedAt { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_by")]
        public Guid? UpdatedBy { get; set; }

        // JSON: { "child_name": "Bin", "color": "Blue", "embroidery": "heart" }
        [Column("print_data", TypeName = "nvarchar(max)")]
        public string? PrintData { get; set; }

        // 1 = low, 5 = urgent
        [Column("priority")]
        public int Priority { get; set; } = 3;

        [MaxLength(50)]
        [Column("batch_number")]
        public string? BatchNumber { get; set; }

        [MaxLength(60)]
        [Column("machine_id")]
        public string? MachineId { get; set; }

        [Column("estimated_duration_min")]
        public int? EstimatedDurationMin { get; set; }

        [Column("actual_duration_min")]
        public int? ActualDurationMin { get; set; }

        // QC score 1-100, null = not inspected
        [Column("quality_score")]
        public int? QualityScore { get; set; }

        [MaxLength(500)]
        [Column("rejection_reason")]
        public string? RejectionReason { get; set; }

        [Column("retry_count")]
        public int RetryCount { get; set; } = 0;

        [MaxLength(1000)]
        [Column("notes")]
        public string? Notes { get; set; }

        // ---- NEW: split by production part ----
        [Column("esp_technician_id")]
        public Guid? EspTechnicianId { get; set; }

        [MaxLength(30)]
        [Column("esp_status")]
        public string EspStatus { get; set; } = "IN_QUEUE";

        [Column("esp_finished_at")]
        public DateTime? EspFinishedAt { get; set; }

        [MaxLength(1000)]
        [Column("esp_handover_photo_urls")]
        public string? EspHandoverPhotoUrls { get; set; } // JSON string array

        [MaxLength(500)]
        [Column("esp_handover_note")]
        public string? EspHandoverNote { get; set; }

        [Column("shell_technician_id")]
        public Guid? ShellTechnicianId { get; set; }

        [MaxLength(30)]
        [Column("shell_status")]
        public string ShellStatus { get; set; } = "IN_QUEUE";

        [Column("shell_finished_at")]
        public DateTime? ShellFinishedAt { get; set; }

        [MaxLength(1000)]
        [Column("shell_handover_photo_urls")]
        public string? ShellHandoverPhotoUrls { get; set; } // JSON string array

        [MaxLength(500)]
        [Column("shell_handover_note")]
        public string? ShellHandoverNote { get; set; }

        [Column("qc_inspector_id")]
        public Guid? QCInspectorId { get; set; }

        [MaxLength(30)]
        [Column("qc_status")]
        public string QCStatus { get; set; } = "IN_QUEUE";

        [Column("qc_finished_at")]
        public DateTime? QCFinishedAt { get; set; }

        [MaxLength(1000)]
        [Column("qc_handover_photo_urls")]
        public string? QCHandoverPhotoUrls { get; set; } // JSON string array

        [MaxLength(500)]
        [Column("qc_handover_note")]
        public string? QCHandoverNote { get; set; }

        // ---- NEW: Commission & Evidence ----
        [Column("total_assembly_cost")]
        public decimal TotalAssemblyCost { get; set; } = 0m;

        [Column("craftsman_commission")]
        public decimal CraftsmanCommission { get; set; } = 0m;

        [Column("qc_commission")]
        public decimal QCCommission { get; set; } = 0m;

        [Column("evidence_images", TypeName = "jsonb")]
        public string? EvidenceImages { get; set; } // JSON: [{"tag": "stitching", "url": "..."}]

        [ForeignKey("OrderItemId")]
        public OrderItem OrderItem { get; set; } = null!;

        [ForeignKey("BuildId")]
        public Build? Build { get; set; }

        [ForeignKey("TechnicianId")]
        public User? Technician { get; set; }

        [ForeignKey("UpdatedBy")]
        public User? UpdatedByUser { get; set; }
    }
}
