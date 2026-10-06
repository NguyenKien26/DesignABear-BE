namespace DesignABear.Services.DTOs.Requests
{
    public class UpdateProductMediaRequest
    {
        public string Url { get; set; } = null!;
        public int SortOrder { get; set; }
        public string? AltText { get; set; }
    }
}
