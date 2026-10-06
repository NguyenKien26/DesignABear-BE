using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DesignABear.Repositories.Models
{
    [Table("user_tokens")]
    public class UserToken
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [Column("user_id")]
        public Guid UserId { get; set; }

        [Required]
        [MaxLength(255)]
        [Column("token")]
        public string Token { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        [Column("type")]
        public string Type { get; set; } = null!; // e.g., "EmailVerification", "PasswordReset"

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("expires_at")]
        public DateTime ExpiresAt { get; set; }

        [Column("is_used")]
        public bool IsUsed { get; set; } = false;

        [ForeignKey("UserId")]
        public User User { get; set; } = null!;
    }
}
