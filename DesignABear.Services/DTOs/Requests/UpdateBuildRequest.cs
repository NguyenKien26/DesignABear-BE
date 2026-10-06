namespace DesignABear.Services.DTOs.Requests
{
    public class UpdateBuildRequest
    {
        public string? BuildName { get; set; }
        public string? PersonalizationNote { get; set; }
        public bool? IncludesSmartChip { get; set; }
        public string Status { get; set; } = null!;
    }
}
