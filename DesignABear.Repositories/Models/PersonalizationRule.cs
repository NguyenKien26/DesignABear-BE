using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DesignABear.Repositories.Models
{
    [Table("personalization_rules")]
    public class PersonalizationRule
    {
        [Key]
        [Column("rule_id")]
        public Guid RuleId { get; set; }

        [Required]
        [Column("base_product_id")]
        public Guid BaseProductId { get; set; }

        [Required]
        [Column("addon_product_id")]
        public Guid AddonProductId { get; set; }

        [Required]
        [Column("group_id")]
        public Guid GroupId { get; set; }

        [Required]
        [MaxLength(20)]
        [Column("rule_type")]
        public string RuleType { get; set; } = null!;

        [Column("max_qty")]
        public int MaxQty { get; set; } = 1;

        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        [ForeignKey("BaseProductId")]
        public Product BaseProduct { get; set; } = null!;

        [ForeignKey("AddonProductId")]
        public Product AddonProduct { get; set; } = null!;

        [ForeignKey("GroupId")]
        public PersonalizationGroup Group { get; set; } = null!;
    }
}
