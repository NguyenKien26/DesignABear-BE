using System.ComponentModel.DataAnnotations;

namespace DesignABear.Services.DTOs.Requests
{
    public class UpdateAddressRequest
    {
        public string? Label { get; set; }

        [Required]
        [MaxLength(120)]
        public string FullName { get; set; } = null!;

        [Required]
        [MaxLength(20)]
        public string PhoneNumber { get; set; } = null!;

        [MaxLength(120)]
        public string? Email { get; set; }

        [Required]
        public string Line1 { get; set; } = null!;
        public string? Line2 { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }

        public bool IsDefaultShipping { get; set; }
        public bool IsDefaultBilling { get; set; }
    }
}
