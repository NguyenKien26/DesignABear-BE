using System.ComponentModel.DataAnnotations;

namespace DesignABear.Services.DTOs.Requests
{
    public class RegisterRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = null!;

        public string? FullName { get; set; }
        
        [RegularExpression(@"^(?!0123456789|09999999)0[0-9]{9}$", ErrorMessage = "Phone number must be exactly 10 digits, start with 0, and not be a popular fake number.")]
        public string? PhoneNumber { get; set; }
        
        public string? AvatarUrl { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
    }
}
