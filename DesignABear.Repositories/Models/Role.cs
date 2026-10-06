using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DesignABear.Repositories.Models
{
    [Table("roles")]
    public class Role
    {
        [Key]
        [Column("role_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoleId { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("role_name")]
        public string RoleName { get; set; } = null!;

        [MaxLength(255)]
        [Column("description")]
        public string? Description { get; set; }

        // true = system-defined, cannot be deleted
        [Column("is_system")]
        public bool IsSystem { get; set; } = false;

        // Lower value = higher priority
        [Column("priority")]
        public int Priority { get; set; } = 100;

        // Hex color for UI badge, e.g. "#FF5733"
        [MaxLength(7)]
        [Column("color_hex")]
        public string? ColorHex { get; set; }

        // Icon name for UI, e.g. "admin_panel_settings"
        [MaxLength(60)]
        [Column("icon")]
        public string? Icon { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [Column("created_by")]
        public Guid? CreatedBy { get; set; }

        [Column("updated_by")]
        public Guid? UpdatedBy { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
