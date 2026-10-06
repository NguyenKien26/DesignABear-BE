using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DesignABear.Repositories.Models
{
    [Table("inventory")]
    public class Inventory
    {
        [Key]
        [Column("inventory_id")]
        public Guid InventoryId { get; set; }

        [Required]
        [Column("location_id")]
        public Guid LocationId { get; set; }

        [Column("product_id")]
        public Guid? ProductId { get; set; }

        [Column("identity_id")]
        public Guid IdentityId { get; set; }

        [Column("is_accessory")]
        public bool IsAccessory { get; set; } = false;

        [Column("on_hand")]
        public int OnHand { get; set; } = 0;

        [Column("reserved")]
        public int Reserved { get; set; } = 0;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("LocationId")]
        public Location Location { get; set; } = null!;

        [ForeignKey("ProductId")]
        public virtual Product? Product { get; set; }
    }
}
