using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DesignABear.Repositories.Models
{
    [Table("order_promotions")]
    public class OrderPromotion
    {
        [Column("order_id")]
        public Guid OrderId { get; set; }

        [Column("promotion_id")]
        public Guid PromotionId { get; set; }

        [Column("discount_amount")]
        public decimal DiscountAmount { get; set; } = 0;

        [ForeignKey("OrderId")]
        public Order Order { get; set; } = null!;

        [ForeignKey("PromotionId")]
        public Promotion Promotion { get; set; } = null!;
    }
}
