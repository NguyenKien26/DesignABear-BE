namespace DesignABear.Services.DTOs.Requests
{
    public class UpdateCharacterRequest
    {
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public string? LicenseBrand { get; set; }
    }
}
