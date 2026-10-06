using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DesignABear.Repositories.Models
{
    [Table("product_collections")]
    public class ProductCollection
    {
        [Column("product_id")]
        public Guid ProductId { get; set; }

        [Column("collection_id")]
        public Guid CollectionId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; } = null!;

        [ForeignKey("CollectionId")]
        public Collection Collection { get; set; } = null!;
    }
}
