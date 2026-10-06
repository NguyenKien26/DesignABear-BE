using System;

namespace DesignABear.Services.DTOs.Responses
{
    public class AccessoryResponse
    {
        public Guid AccessoryId { get; set; }
        public string Name { get; set; } = null!;
        public string Sku { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        
        // 3 prices as requested
        public decimal BaseCost { get; set; }
        public decimal AssemblyCost { get; set; }
        public decimal TargetPrice { get; set; }
        
        public bool IsActive { get; set; }
        public int OnHand { get; set; }
        public int Reserved { get; set; }
        public int Available => OnHand - Reserved;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
