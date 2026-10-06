using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DesignABear.Repositories.Models
{
    [Table("users")]
    public class User
    {
        [Key]
        [Column("user_id")]
        public Guid UserId { get; set; }

        [Required]
        [MaxLength(120)]
        [Column("email")]
        public string Email { get; set; } = null!;

        [MaxLength(255)]
        [Column("password_hash")]
        public string? PasswordHash { get; set; }

        [MaxLength(120)]
        [Column("full_name")]
        public string? FullName { get; set; }

        [MaxLength(255)]
        [Column("avatar_url")]
        public string? AvatarUrl { get; set; }

        [MaxLength(30)]
        [Column("provider")]
        public string Provider { get; set; } = "Local";

        [MaxLength(255)]
        [Column("provider_id")]
        public string? ProviderId { get; set; }

        [MaxLength(20)]
        [Column("status")]
        public UserStatus Status { get; set; } = UserStatus.Active;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [MaxLength(30)]
        [Column("phone_number")]
        public string? PhoneNumber { get; set; }

        [Column("date_of_birth")]
        public DateOnly? DateOfBirth { get; set; }

        // M | F | O
        [MaxLength(1)]
        [Column("gender")]
        public string? Gender { get; set; }

        [MaxLength(10)]
        [Column("language")]
        public string Language { get; set; } = "vi";

        [MaxLength(60)]
        [Column("timezone")]
        public string Timezone { get; set; } = "Asia/Ho_Chi_Minh";

        [Column("last_login_at")]
        public DateTime? LastLoginAt { get; set; }

        [Column("failed_login_count")]
        public int FailedLoginCount { get; set; } = 0;

        [Column("is_locked")]
        public bool IsLocked { get; set; } = false;

        [MaxLength(512)]
        [Column("refresh_token")]
        public string? RefreshToken { get; set; }

        [Column("two_factor_enabled")]
        public bool TwoFactorEnabled { get; set; } = false;

        [Required]
        [Column("role_id")]
        public int RoleId { get; set; } = 3; // Default to Customer/Parent

        [Column("workplace_id")]
        public Guid? WorkplaceId { get; set; }

        [ForeignKey("RoleId")]
        public Role Role { get; set; } = null!;

        [ForeignKey("WorkplaceId")]
        public virtual Location? Workplace { get; set; }

        [Column("total_jobs_completed")]
        public int TotalJobsCompleted { get; set; } = 0;

        [Column("total_earned")]
        public decimal TotalEarned { get; set; } = 0m;

        public ICollection<Address> Addresses { get; set; } = new List<Address>();
        public ICollection<Build> Builds { get; set; } = new List<Build>();
        public ICollection<Cart> Carts { get; set; } = new List<Cart>();
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<UserToken> UserTokens { get; set; } = new List<UserToken>();
    }
}
