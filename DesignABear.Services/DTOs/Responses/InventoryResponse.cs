namespace DesignABear.Services.DTOs.Responses
{
    public class InventoryResponse
    {
        public Guid InventoryId { get; set; }
        public Guid LocationId { get; set; }
        public Guid? ProductId { get; set; }
        public Guid IdentityId { get; set; }
        public bool IsAccessory { get; set; }
        public string? ProductName { get; set; }
        public string? ProductType { get; set; }
        public string? ImageUrl { get; set; }

        public string? SizeTag { get; set; }
        public string? SizeDescription { get; set; }
        public string? Sku { get; set; }
        public int OnHand { get; set; }
        public int Reserved { get; set; }
        public int TotalAvailable => OnHand - Reserved;

        public DateTime UpdatedAt { get; set; }
    }
}
