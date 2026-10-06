namespace DesignABear.Services.DTOs.Responses
{
    public class CharacterResponse
    {
        public Guid CharacterId { get; set; }
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public string? LicenseBrand { get; set; }
        public bool IsActive { get; set; }
    }
}
