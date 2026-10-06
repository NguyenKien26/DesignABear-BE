namespace DesignABear.Services.DTOs.Responses
{
    public class BuildResponse
    {
        public Guid BuildId { get; set; }
        public Guid? UserId { get; set; }
        public Guid BaseProductId { get; set; }
        public string? BuildName { get; set; }
        public string? PersonalizationNote { get; set; }
        public float? TotalWeightGram { get; set; }
        public decimal? CalculatedPrice { get; set; }
        public bool IncludesSmartChip { get; set; }
        public string Status { get; set; } = "DRAFT";
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        // Custom fields from Build
        public string? SizeTag { get; set; }
        public string? SizeDescription { get; set; }
        
        public string? BaseProductName { get; set; }
        public string? BaseProductImageUrl { get; set; }

        public IEnumerable<BuildComponentResponse> BuildComponents { get; set; } = new List<BuildComponentResponse>();
    }
}
