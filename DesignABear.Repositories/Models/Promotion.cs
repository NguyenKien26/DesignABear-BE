using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DesignABear.Repositories.Models
{
    [Table("promotions")]
    public class Promotion
    {
        [Key]
        [Column("promotion_id")]
        public Guid PromotionId { get; set; }

        [Required]
        [MaxLength(40)]
        [Column("code")]
        public string Code { get; set; } = null!;

        [Required]
        [MaxLength(20)]
        [Column("discount_type")]
        public string DiscountType { get; set; } = null!;

        [Required]
        [Column("value")]
        public decimal Value { get; set; }

        [Column("starts_at")]
        public DateTime? StartsAt { get; set; }

        [Column("ends_at")]
        public DateTime? EndsAt { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        [Column("min_order_amount")]
        public decimal MinOrderAmount { get; set; } = 0;

        [Column("max_usage_count")]
        public int? MaxUsageCount { get; set; }

        [Column("usage_count")]
        public int UsageCount { get; set; } = 0;

        [Column("max_usage_per_user")]
        public int? MaxUsagePerUser { get; set; }

        [MaxLength(500)]
        [Column("description")]
        public string? Description { get; set; }

        public ICollection<OrderPromotion> OrderPromotions { get; set; } = new List<OrderPromotion>();
    }
}
