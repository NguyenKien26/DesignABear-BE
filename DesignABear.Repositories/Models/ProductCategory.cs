using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DesignABear.Repositories.Models
{
    [Table("product_categories")]
    public class ProductCategory
    {
        [Column("product_id")]
        public Guid ProductId { get; set; }

        [Column("category_id")]
        public Guid CategoryId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; } = null!;

        [ForeignKey("CategoryId")]
        public Category Category { get; set; } = null!;
    }
}
