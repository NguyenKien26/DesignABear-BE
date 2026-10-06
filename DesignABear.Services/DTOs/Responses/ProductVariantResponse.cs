using System;

namespace DesignABear.Services.DTOs.Responses
{
    public class ProductVariantResponse
    {
        public Guid VariantId { get; set; }
        public Guid ProductId { get; set; }
        public string? SizeTag { get; set; }
        public string? SizeDescription { get; set; }
        public string Sku { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal BaseCost { get; set; }
        public decimal AssemblyCost { get; set; }
        public float WeightGram { get; set; }
        public bool IsActive { get; set; }
        public int OnHand { get; set; }
        public int Reserved { get; set; }
        public int Available => OnHand - Reserved;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
