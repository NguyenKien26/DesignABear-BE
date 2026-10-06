using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DesignABear.Repositories.Models
{
    [Table("collections")]
    public class Collection
    {
        [Key]
        [Column("collection_id")]
        public Guid CollectionId { get; set; }

        [Required]
        [MaxLength(120)]
        [Column("name")]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(160)]
        [Column("slug")]
        public string Slug { get; set; } = null!;

        public ICollection<ProductCollection> ProductCollections { get; set; } = new List<ProductCollection>();
    }
}
