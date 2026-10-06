using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DesignABear.Repositories.Models
{
    [Table("product_accessories")]
    public class ProductAccessory
    {
        [Required]
        [Column("product_id")]
        public Guid ProductId { get; set; }

        [Required]
        [Column("accessory_id")]
        public Guid AccessoryId { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; } = null!;

        [ForeignKey("AccessoryId")]
        public virtual Accessory Accessory { get; set; } = null!;
    }
}
