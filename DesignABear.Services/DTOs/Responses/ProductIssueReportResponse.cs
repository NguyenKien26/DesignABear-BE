using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignABear.Services.DTOs.Responses
{
    public class ProductIssueReportResponse
    {
        public Guid ReportId { get; set; }
        public Guid OrderItemId { get; set; }
        public Guid ProductId { get; set; }
        public Guid UserId { get; set; }
        public Guid? AssignedStaffId { get; set; }
        public string Status { get; set; } = "PENDING";
        public string? Resolution { get; set; }
        public bool RequestRefund { get; set; }
        public string? Description { get; set; }
        public string[]? EvidenceUrls { get; set; }
        public string? StaffNotes { get; set; }
        public string? RepairNotes { get; set; }
        public Guid? RefundId { get; set; }
        public Guid? ReplacementFulfillmentId { get; set; }
        public DateTime? ResolvedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
