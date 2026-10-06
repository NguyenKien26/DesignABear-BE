namespace DesignABear.Services.DTOs.Responses
{
    public class BuildComponentResponse
    {
        public Guid BuildComponentId { get; set; }
        public Guid BuildId { get; set; }
        public Guid GroupId { get; set; }
        public Guid OptionProductId { get; set; }
        public int Quantity { get; set; }
        public decimal? AdditionalPrice { get; set; }

        public string? ProductName { get; set; }
        public string? ImageUrl { get; set; }
        public string? Size { get; set; }
        public float WeightGram { get; set; }
    }
}
