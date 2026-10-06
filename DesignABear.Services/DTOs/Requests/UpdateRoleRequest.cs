using System.ComponentModel.DataAnnotations;

namespace DesignABear.Services.DTOs.Requests
{
    public class UpdateRoleRequest
    {
        [Required]
        [MaxLength(50)]
        public string RoleName { get; set; } = null!;
    }
}
