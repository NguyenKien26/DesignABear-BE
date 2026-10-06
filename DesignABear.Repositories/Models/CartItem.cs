using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DesignABear.Repositories.Models
{
    [Table("cart_items")]
    public class CartItem
    {
        [Key]
        [Column("cart_item_id")]
        public Guid CartItemId { get; set; }

        [Required]
        [Column("cart_id")]
        public Guid CartId { get; set; }

        [Column("product_id")]
        public Guid? ProductId { get; set; }

        [Column("build_id")]
        public Guid? BuildId { get; set; }

        [Column("quantity")]
        public int Quantity { get; set; } = 1;

        [Column("includes_smart_chip")]
        public bool IncludesSmartChip { get; set; } = false;

        [Column("unit_price_snapshot")]
        public decimal? UnitPriceSnapshot { get; set; }



        [Column("variant_id")]
        public Guid? VariantId { get; set; }
        
        [ForeignKey("CartId")]
        public Cart Cart { get; set; } = null!;

        [ForeignKey("ProductId")]
        public Product? Product { get; set; }

        [ForeignKey("VariantId")]
        public virtual ProductVariant? Variant { get; set; }

        [ForeignKey("BuildId")]
        public Build? Build { get; set; }
    }
}
