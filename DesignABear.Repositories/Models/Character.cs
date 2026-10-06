using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DesignABear.Repositories.Models
{
    [Table("characters")]
    public class Character
    {
        [Key]
        [Column("character_id")]
        public Guid CharacterId { get; set; }

        [Required]
        [MaxLength(120)]
        [Column("name")]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(160)]
        [Column("slug")]
        public string Slug { get; set; } = null!;

        [MaxLength(120)]
        [Column("license_brand")]
        public string? LicenseBrand { get; set; }

        [Column("description")]
        public string? Description { get; set; }

        [MaxLength(255)]
        [Column("image_url")]
        public string? ImageUrl { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        public ICollection<ProductCharacter> ProductCharacters { get; set; } = new List<ProductCharacter>();
    }
}
