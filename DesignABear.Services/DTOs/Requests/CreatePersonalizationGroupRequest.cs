namespace DesignABear.Services.DTOs.Requests
{
    public class CreatePersonalizationGroupRequest
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }
}
