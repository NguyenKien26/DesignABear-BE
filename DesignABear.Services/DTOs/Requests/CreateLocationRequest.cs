namespace DesignABear.Services.DTOs.Requests
{
    public class CreateLocationRequest
    {
        public string Name { get; set; } = null!;
        public string Type { get; set; } = null!;
        public string? AddressLine1 { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }
    }
}
