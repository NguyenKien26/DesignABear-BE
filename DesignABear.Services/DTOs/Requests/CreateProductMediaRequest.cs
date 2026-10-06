namespace DesignABear.Services.DTOs.Requests
{
    public class CreateProductMediaRequest
    {
        public Guid ProductId { get; set; }
        public string Url { get; set; } = null!;
        public int SortOrder { get; set; } = 0;
        public string? AltText { get; set; }
    }
}
