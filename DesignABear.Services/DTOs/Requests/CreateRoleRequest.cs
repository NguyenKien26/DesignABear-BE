using System.ComponentModel.DataAnnotations;

namespace DesignABear.Services.DTOs.Requests
{
    public class CreateRoleRequest
    {
        [Required]
        [MaxLength(50)]
        public string RoleName { get; set; } = null!;
    }
}
