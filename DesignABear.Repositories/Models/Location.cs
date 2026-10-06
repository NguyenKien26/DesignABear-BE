using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DesignABear.Repositories.Models
{
    [Table("locations")]
    public class Location
    {
        [Key]
        [Column("location_id")]
        public Guid LocationId { get; set; }

        [Required]
        [MaxLength(20)]
        [Column("location_type")]
        public string LocationType { get; set; } = null!;

        [Required]
        [MaxLength(120)]
        [Column("name")]
        public string Name { get; set; } = null!;

        [MaxLength(400)]
        [Column("address_text")]
        public string? AddressText { get; set; }

        [MaxLength(30)]
        [Column("phone")]
        public string? Phone { get; set; }

        public ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();
        public ICollection<Fulfillment> Fulfillments { get; set; } = new List<Fulfillment>();
    }
}
