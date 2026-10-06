using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DesignABear.Repositories.Models
{
    [Table("categories")]
    public class Category
    {
        [Key]
        [Column("category_id")]
        public Guid CategoryId { get; set; }

        [Column("parent_id")]
        public Guid? ParentId { get; set; }

        [Required]
        [MaxLength(120)]
        [Column("name")]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(160)]
        [Column("slug")]
        public string Slug { get; set; } = null!;

        [ForeignKey("ParentId")]
        public Category? Parent { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        public ICollection<Category> Children { get; set; } = new List<Category>();
        public ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();
    }
}
