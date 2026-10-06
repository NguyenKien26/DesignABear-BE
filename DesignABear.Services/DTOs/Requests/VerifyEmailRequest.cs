using System.ComponentModel.DataAnnotations;

namespace DesignABear.Services.DTOs.Requests
{
    public class VerifyEmailRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [StringLength(6, MinimumLength = 6)]
        public string Otp { get; set; } = null!;
    }
}
