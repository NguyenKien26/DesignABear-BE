using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DesignABear.Repositories.Models
{
    [Table("product_variants")]
    public class ProductVariant
    {
        [Key]
        [Column("variant_id")]
        public Guid VariantId { get; set; }

        [Required]
        [Column("product_id")]
        public Guid ProductId { get; set; }

        [MaxLength(10)]
        [Column("size_tag")]
        public string SizeTag { get; set; } = null!;

        [MaxLength(100)]
        [Column("size_description")]
        public string SizeDescription { get; set; } = null!;

        [Required]
        [Column("price")]
        public decimal Price { get; set; }

        [Column("base_cost")]
        public decimal BaseCost { get; set; } = 0m;

        [Column("assembly_cost")]
        public decimal AssemblyCost { get; set; } = 0m;

        [Column("weight_gram")]
        public float WeightGram { get; set; } = 500f;

        [MaxLength(80)]
        [Column("sku")]
        public string Sku { get; set; } = string.Empty;

        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; } = null!;

        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
