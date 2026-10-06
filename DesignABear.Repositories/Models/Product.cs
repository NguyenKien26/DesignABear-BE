using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DesignABear.Repositories.Models
{
    [Table("products")]
    public class Product
    {
        [Key]
        [Column("product_id")]
        public Guid ProductId { get; set; }

        [Required]
        [MaxLength(200)]
        [Column("name")]
        public string Name { get; set; } = null!;

        [Column("slug")]
        [MaxLength(220)]
        public string Slug { get; set; } = null!;

        [Column("sku")]
        [MaxLength(40)]
        public string Sku { get; set; } = string.Empty;


        [Column("description")]
        public string? Description { get; set; }

        [Column("is_personalizable")]
        public bool IsPersonalizable { get; set; } = false;

        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        [Column("model_3d_url")]
        public string? Model3DUrl { get; set; }

        [Column("total_sales")]
        public int TotalSales { get; set; } = 0;


        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<ProductMedia> Media { get; set; } = new List<ProductMedia>();
        public ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();
        public ICollection<ProductCollection> ProductCollections { get; set; } = new List<ProductCollection>();
        public ICollection<ProductCharacter> ProductCharacters { get; set; } = new List<ProductCharacter>();
        public ICollection<ProductAccessory> ProductAccessories { get; set; } = new List<ProductAccessory>();
        public ICollection<PersonalizationRule> BasePersonalizationRules { get; set; } = new List<PersonalizationRule>();
        public ICollection<PersonalizationRule> AddonPersonalizationRules { get; set; } = new List<PersonalizationRule>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<ProductComboImage> ComboImages { get; set; } = new List<ProductComboImage>();
        public virtual ProductDynamicStats? DynamicStats { get; set; }
        
        public virtual ICollection<ProductVariant> Variants { get; set; } = new List<ProductVariant>();
        public ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public ICollection<BuildComponent> BuildComponents { get; set; } = new List<BuildComponent>();
    }
}
