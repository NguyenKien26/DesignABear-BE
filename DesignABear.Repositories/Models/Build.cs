using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DesignABear.Repositories.Models
{
    [Table("builds")]
    public class Build
    {
        [Key]
        [Column("build_id")]
        public Guid BuildId { get; set; }

        [Column("user_id")]
        public Guid? UserId { get; set; }

        [Required]
        [Column("base_product_id")]
        public Guid BaseProductId { get; set; }

        [MaxLength(160)]
        [Column("build_name")]
        public string? BuildName { get; set; }

        [Column("personalization_note")]
        public string? PersonalizationNote { get; set; }

        [Column("total_weight_gram")]
        public float? TotalWeightGram { get; set; }

        [Column("calculated_price")]
        public decimal? CalculatedPrice { get; set; }

        [Column("includes_smart_chip")]
        public bool IncludesSmartChip { get; set; } = false;

        [MaxLength(30)]
        [Column("status")]
        public string Status { get; set; } = "DRAFT";

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [Column("base_variant_id")]
        public Guid? BaseVariantId { get; set; }

        [MaxLength(10)]
        [Column("size_tag")]
        public string? SizeTag { get; set; }

        [MaxLength(100)]
        [Column("size_description")]
        public string? SizeDescription { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }

        [ForeignKey("BaseProductId")]
        public Product BaseProduct { get; set; } = null!;

        [ForeignKey("BaseVariantId")]
        public virtual ProductVariant? BaseVariant { get; set; }

        public ICollection<BuildComponent> BuildComponents { get; set; } = new List<BuildComponent>();
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public ICollection<ProductionJob> ProductionJobs { get; set; } = new List<ProductionJob>();
    }
}
