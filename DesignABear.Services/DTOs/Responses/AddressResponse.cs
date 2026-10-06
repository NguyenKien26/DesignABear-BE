namespace DesignABear.Services.DTOs.Responses
{
    public class AddressResponse
    {
        public Guid AddressId { get; set; }
        public Guid UserId { get; set; }
        public string? Label { get; set; }
        public string FullName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string? Email { get; set; }
        public string Line1 { get; set; } = null!;
        public string? Line2 { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }
        public bool IsDefaultShipping { get; set; }
        public bool IsDefaultBilling { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
