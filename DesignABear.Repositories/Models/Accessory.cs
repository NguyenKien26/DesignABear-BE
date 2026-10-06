using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DesignABear.Repositories.Models
{
    [Table("accessories")]
    public class Accessory
    {
        [Key]
        [Column("accessory_id")]
        public Guid AccessoryId { get; set; }

        [Required]
        [MaxLength(200)]
        [Column("name")]
        public string Name { get; set; } = null!;



        [MaxLength(80)]
        [Column("sku")]
        public string Sku { get; set; } = string.Empty;

        [Column("description")]
        public string? Description { get; set; }

        [MaxLength(255)]
        [Column("image_url")]
        public string? ImageUrl { get; set; }

        [Column("base_cost")]
        public decimal BaseCost { get; set; } = 0m;

        [Column("assembly_cost")]
        public decimal AssemblyCost { get; set; } = 0m;

        [Column("target_price")]
        public decimal TargetPrice { get; set; } = 0m;

        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<ProductAccessory> ProductAccessories { get; set; } = new List<ProductAccessory>();
    }
}
