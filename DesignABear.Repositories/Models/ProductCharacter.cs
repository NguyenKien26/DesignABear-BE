using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DesignABear.Repositories.Models
{
    [Table("product_characters")]
    public class ProductCharacter
    {
        [Column("product_id")]
        public Guid ProductId { get; set; }

        [Column("character_id")]
        public Guid CharacterId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; } = null!;

        [ForeignKey("CharacterId")]
        public Character Character { get; set; } = null!;
    }
}
