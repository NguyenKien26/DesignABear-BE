using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DesignABear.Repositories.Models
{
    [Table("order_items")]
    public class OrderItem
    {
        [Key]
        [Column("order_item_id")]
        public Guid OrderItemId { get; set; }

        [Required]
        [Column("order_id")]
        public Guid OrderId { get; set; }

        [Column("product_id")]
        public Guid? ProductId { get; set; }

        [Column("build_id")]
        public Guid? BuildId { get; set; }

        [Column("quantity")]
        public int Quantity { get; set; } = 1;

        [Required]
        [Column("unit_price")]
        public decimal UnitPrice { get; set; }

        [Required]
        [Column("line_total")]
        public decimal LineTotal { get; set; }

        [MaxLength(255)]
        [Column("product_name_snapshot")]
        public string? ProductNameSnapshot { get; set; }

        [Column("personalization_snapshot")]
        public string? PersonalizationSnapshot { get; set; }

        [Column("weight_snapshot")]
        public float? WeightSnapshot { get; set; }

        [Column("includes_smart_chip")]
        public bool IncludesSmartChip { get; set; } = false;



        [Column("variant_id")]
        public Guid? VariantId { get; set; }
        
        [ForeignKey("OrderId")]
        public Order Order { get; set; } = null!;

        [ForeignKey("ProductId")]
        public Product? Product { get; set; }

        [ForeignKey("VariantId")]
        public virtual ProductVariant? Variant { get; set; }

        [ForeignKey("BuildId")]
        public Build? Build { get; set; }

        public ICollection<ProductionJob> ProductionJobs { get; set; } = new List<ProductionJob>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
