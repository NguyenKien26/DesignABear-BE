namespace DesignABear.Services.DTOs.Requests
{
    public class CreateCartItemRequest
    {
        public Guid? CartId { get; set; }
        public Guid? ProductId { get; set; }
        public Guid? VariantId { get; set; }
        public Guid? BuildId { get; set; }
        public int Quantity { get; set; } = 1;
        public decimal? UnitPriceSnapshot { get; set; }
        public bool IncludesSmartChip { get; set; } = false;
        public string? SizeTag { get; set; }
        public string? SizeDescription { get; set; }
    }
}
