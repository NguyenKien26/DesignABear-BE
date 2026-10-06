using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DesignABear.Repositories.Models
{
    [Table("product_combo_images")]
    public class ProductComboImage
    {
        [Key]
        [Column("combo_id")]
        public Guid ComboId { get; set; }

        [Required]
        [Column("base_product_id")]
        public Guid BaseProductId { get; set; }

        [Required]
        [MaxLength(500)]
        [Column("combination_key")]
        public string CombinationKey { get; set; } = null!;

        [Required]
        [MaxLength(400)]
        [Column("image_url")]
        public string ImageUrl { get; set; } = null!;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("BaseProductId")]
        public Product BaseProduct { get; set; } = null!;
    }
}
