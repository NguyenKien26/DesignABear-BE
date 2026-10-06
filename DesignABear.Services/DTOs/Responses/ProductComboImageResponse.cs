namespace DesignABear.Services.DTOs.Responses
{
    public class ProductComboImageResponse
    {
        public Guid ComboId { get; set; }
        public string CombinationKey { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
    }
}
