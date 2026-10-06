    namespace DesignABear.Services.DTOs.Responses
{
    public class ProductionJobResponse
    {
        public Guid JobId { get; set; }
        public Guid OrderItemId { get; set; }
        public string Status { get; set; } = "PENDING";
        public string? QCStatus { get; set; }
        public bool HasSmartChip { get; set; }
        public Guid? AssignedUser { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public string? SerialNumber { get; set; }
        public string? RejectionReason { get; set; }

        public Guid? QCInspectorId { get; set; }

        // NEW
        public Guid? EspTechnicianId { get; set; }
        public string EspStatus { get; set; } = "IN_QUEUE";
        public DateTime? EspFinishedAt { get; set; }
        public string? EspHandoverPhotoUrls { get; set; }
        public string? EspHandoverNote { get; set; }

        public Guid? ShellTechnicianId { get; set; }
        public string ShellStatus { get; set; } = "IN_QUEUE";
        public DateTime? ShellFinishedAt { get; set; }
        public string? ShellHandoverPhotoUrls { get; set; }
        public string? ShellHandoverNote { get; set; }
        public string? QCHandoverNote { get; set; }
        public DateTime? QCFinishedAt { get; set; }

        // Extra info for UI
        public string? ProductName { get; set; }
        public string? BaseProductName { get; set; }
        public string? BuildName { get; set; }
        public string? ImageUrl { get; set; }
        public string? Size { get; set; }
        public string? SizeTag { get; set; }
        public float WeightGram { get; set; }
        public string? TechnicianName { get; set; }
        
        public decimal TotalAssemblyCost { get; set; }
        public decimal CraftsmanCommission { get; set; }
        public decimal QCCommission { get; set; }
        public string? EvidenceImages { get; set; }

        public IEnumerable<BuildComponentResponse> Components { get; set; } = new List<BuildComponentResponse>();
    }
}
