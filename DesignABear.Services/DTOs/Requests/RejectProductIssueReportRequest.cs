using System.ComponentModel.DataAnnotations;

namespace DesignABear.Services.DTOs.Requests
{
    public class RejectProductIssueReportRequest
    {
        [Required]
        [MaxLength(500)]
        public string Reason { get; set; } = null!;
    }
}
