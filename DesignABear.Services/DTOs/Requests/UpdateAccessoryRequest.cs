using System.ComponentModel.DataAnnotations;

namespace DesignABear.Services.DTOs.Requests
{
    public class UpdateAccessoryRequest
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = null!;

        [MaxLength(80)]
        public string Sku { get; set; } = string.Empty;

        public string? Description { get; set; }

        [MaxLength(255)]
        public string? ImageUrl { get; set; }

        public decimal BaseCost { get; set; }
        public decimal AssemblyCost { get; set; }
        public decimal TargetPrice { get; set; }
        public int StockQuantity { get; set; }
        public bool IsActive { get; set; }
    }
}
