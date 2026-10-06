namespace DesignABear.Services.DTOs.Responses
{
    public class CategoryResponse
    {
        public Guid CategoryId { get; set; }
        public Guid? ParentId { get; set; }
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}
