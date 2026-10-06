namespace DesignABear.Services.DTOs.Responses
{
    public class LocationResponse
    {
        public Guid LocationId { get; set; }
        public string Name { get; set; } = null!;
        public string Type { get; set; } = null!;
        public string? AddressLine1 { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }
    }
}
