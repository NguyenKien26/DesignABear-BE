namespace DesignABear.Services.DTOs.Responses
{
    public class CartItemResponse
    {
        public Guid CartItemId { get; set; }
        public Guid CartId { get; set; }
        public Guid? ProductId { get; set; }
        public Guid? BuildId { get; set; }
        public int Quantity { get; set; }
        public decimal? UnitPriceSnapshot { get; set; }
        public bool IncludesSmartChip { get; set; }

        // Enriched product info
        public string? ProductName { get; set; }
        public string? ProductSlug { get; set; }
        public string? Sku { get; set; }
        public string? Size { get; set; }
        public string? SizeTag { get; set; }
        public string? ProductImageUrl { get; set; }
        public int AvailableStock { get; set; }
        public BuildResponse? BuildDetails { get; set; }
    }
}
