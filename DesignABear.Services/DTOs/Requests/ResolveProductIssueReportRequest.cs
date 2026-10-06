using System;
using System.ComponentModel.DataAnnotations;

namespace DesignABear.Services.DTOs.Requests
{
    public class ResolveProductIssueReportRequest
    {
        [Required]
        public string Resolution { get; set; } = null!;
        public string? StaffNotes { get; set; }
        public string? RepairNotes { get; set; }
    }
}
