using System.ComponentModel.DataAnnotations;

namespace DesignABear.Services.DTOs.Requests
{
    public class CalculateGhtkFeeRequest
    {
        [Required]
        public string Province { get; set; } = string.Empty;

        [Required]
        public string District { get; set; } = string.Empty;

        public string? Ward { get; set; }
        public string? Address { get; set; }

        [Range(1, int.MaxValue)]
        public int Weight { get; set; }

        public int? Value { get; set; }
        public string? Transport { get; set; }
    }
}