using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DesignABear.Repositories.Models
{
    [Table("addresses")]
    public class Address
    {
        [Key]
        [Column("address_id")]
        public Guid AddressId { get; set; }

        [Required]
        [Column("user_id")]
        public Guid UserId { get; set; }

        [MaxLength(60)]
        [Column("label")]
        public string? Label { get; set; }

        [Required]
        [MaxLength(120)]
        [Column("full_name")]
        public string FullName { get; set; } = null!;

        [Required]
        [MaxLength(20)]
        [Column("phone_number")]
        public string PhoneNumber { get; set; } = null!;

        [MaxLength(120)]
        [Column("email")]
        public string? Email { get; set; }

        [Required]
        [MaxLength(255)]
        [Column("line1")]
        public string Line1 { get; set; } = null!;

        [MaxLength(255)]
        [Column("line2")]
        public string? Line2 { get; set; }

        [MaxLength(80)]
        [Column("city")]
        public string? City { get; set; }

        [MaxLength(80)]
        [Column("state")]
        public string? State { get; set; }

        [MaxLength(20)]
        [Column("postal_code")]
        public string? PostalCode { get; set; }

        [MaxLength(80)]
        [Column("country")]
        public string? Country { get; set; }

        [Column("is_default_shipping")]
        public bool IsDefaultShipping { get; set; } = false;

        [Column("is_default_billing")]
        public bool IsDefaultBilling { get; set; } = false;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("UserId")]
        public User User { get; set; } = null!;
    }
}
