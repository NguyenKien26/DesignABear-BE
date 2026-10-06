using System.ComponentModel.DataAnnotations;

namespace DesignABear.Repositories.Integrations.Ghtk.Models
{
    public class GhtkFeeRequest
    {
        public string? PickProvince { get; set; }
        public string? PickDistrict { get; set; }
        public string? PickWard { get; set; }
        public string? PickAddress { get; set; }

        [Required]
        public string Province { get; set; } = string.Empty;

        [Required]
        public string District { get; set; } = string.Empty;

        public string? Ward { get; set; }
        public string? Address { get; set; }

        [Range(1, int.MaxValue)]
        public int Weight { get; set; } // gram

        public int? Value { get; set; } // VND
        public string? Transport { get; set; } // road / fly
    }
}