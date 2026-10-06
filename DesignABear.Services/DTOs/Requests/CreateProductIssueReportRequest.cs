using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DesignABear.Services.DTOs.Requests
{
    public class CreateProductIssueReportRequest
    {
        [Required]
        public Guid OrderItemId { get; set; }

        [Required]
        public string Description { get; set; } = null!;

        public bool RequestRefund { get; set; } = false;

        public string[]? EvidenceUrls { get; set; }
    }
}
