using System.ComponentModel.DataAnnotations;

namespace DesignABear.Services.DTOs.Requests
{
    public class GoogleCompleteProfileRequest
    {
        [Required]
        public string RegistrationToken { get; set; } = null!;

        [Required]
        [RegularExpression(@"^(?!0123456789|09999999)0[0-9]{9}$", ErrorMessage = "Phone number must be exactly 10 digits, start with 0, and not be a popular fake number.")]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
        public string Password { get; set; } = null!;

        [Required]
        public string FullName { get; set; } = null!;

        public DateTime? DateOfBirth { get; set; }
        
        public string? Gender { get; set; }
    }
}
