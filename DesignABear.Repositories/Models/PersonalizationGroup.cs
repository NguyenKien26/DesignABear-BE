using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DesignABear.Repositories.Models
{
    [Table("personalization_groups")]
    public class PersonalizationGroup
    {
        [Key]
        [Column("group_id")]
        public Guid GroupId { get; set; }

        [Required]
        [MaxLength(120)]
        [Column("name")]
        public string Name { get; set; } = null!;

        [Column("sort_order")]
        public int SortOrder { get; set; } = 0;

        public ICollection<PersonalizationRule> PersonalizationRules { get; set; } = new List<PersonalizationRule>();
        public ICollection<BuildComponent> BuildComponents { get; set; } = new List<BuildComponent>();
    }
}
