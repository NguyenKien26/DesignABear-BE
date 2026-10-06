namespace DesignABear.Services.DTOs.Requests
{
    public class CreateBuildRequest
    {
        public Guid? UserId { get; set; }
        public Guid BaseProductId { get; set; }
        public string? BuildName { get; set; }
        public string? PersonalizationNote { get; set; }
        public string? SizeTag { get; set; }
        public string? SizeDescription { get; set; }
        public bool IncludesSmartChip { get; set; } = false;
    }
}
