namespace DesignABear.Services.DTOs.Responses
{
    public class CollectionResponse
    {
        public Guid CollectionId { get; set; }
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;
    }
}
