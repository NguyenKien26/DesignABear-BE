namespace DesignABear.Services.DTOs.Requests
{
    public class UpdatePersonalizationGroupRequest
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }
}
