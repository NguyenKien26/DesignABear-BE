namespace DesignABear.Services.DTOs.Responses
{
    public class PersonalizationGroupResponse
    {
        public Guid GroupId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }
}
