using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DesignABear.Repositories.Models
{
    [Table("product_media")]
    public class ProductMedia
    {
        [Key]
        [Column("media_id")]
        public Guid MediaId { get; set; }

        [Required]
        [Column("product_id")]
        public Guid ProductId { get; set; }

        [Required]
        [MaxLength(400)]
        [Column("url")]
        public string Url { get; set; } = null!;

        [Column("sort_order")]
        public int SortOrder { get; set; } = 0;

        [MaxLength(200)]
        [Column("alt_text")]
        public string? AltText { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; } = null!;
    }
}
