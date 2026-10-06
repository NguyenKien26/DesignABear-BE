namespace DesignABear.Services.DTOs.Responses
{
    public class OrderItemResponse
    {
        public Guid OrderItemId { get; set; }
        public Guid OrderId { get; set; }
        public Guid? ProductId { get; set; }
        public Guid? BuildId { get; set; }
        public int Quantity { get; set; }
        public string? ProductName { get; set; }
        public string? ProductImageUrl { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal { get; set; }
        public string? ProductNameSnapshot { get; set; }
        public string? PersonalizationSnapshot { get; set; }
        public float? WeightSnapshot { get; set; }
        public bool IncludesSmartChip { get; set; }

        public string? Sku { get; set; }
        public string? Size { get; set; }
        public string? SizeTag { get; set; }
        public BuildResponse? BuildDetails { get; set; }
        
        public IEnumerable<ProductionJobResponse> ProductionJobs { get; set; } = new List<ProductionJobResponse>();
    }
}
