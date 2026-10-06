namespace DesignABear.Services.DTOs.Responses
{
    public class ProductMediaResponse
    {
        public Guid MediaId { get; set; }
        public Guid ProductId { get; set; }
        public string Url { get; set; } = null!;
        public int SortOrder { get; set; }
        public string? AltText { get; set; }
    }
}
