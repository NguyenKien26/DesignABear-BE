using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DesignABear.Repositories.Models
{
    [Table("build_components")]
    public class BuildComponent
    {
        [Key]
        [Column("build_component_id")]
        public Guid BuildComponentId { get; set; }

        [Required]
        [Column("build_id")]
        public Guid BuildId { get; set; }

        [Required]
        [Column("group_id")]
        public Guid GroupId { get; set; }

        [Required]
        [Column("option_product_id")]
        public Guid OptionProductId { get; set; }

        [Column("quantity")]
        public int Quantity { get; set; } = 1;

        [Column("additional_price")]
        public decimal? AdditionalPrice { get; set; }

        [Column("option_variant_id")]
        public Guid? OptionVariantId { get; set; }

        [ForeignKey("BuildId")]
        public Build Build { get; set; } = null!;

        [ForeignKey("GroupId")]
        public PersonalizationGroup Group { get; set; } = null!;

        [ForeignKey("OptionProductId")]
        public Product OptionProduct { get; set; } = null!;

        [ForeignKey("OptionVariantId")]
        public virtual ProductVariant? OptionVariant { get; set; }
    }
}
