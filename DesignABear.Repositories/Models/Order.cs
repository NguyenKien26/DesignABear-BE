using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DesignABear.Repositories.Models
{
    [Table("orders")]
    public class Order
    {
        [Key]
        [Column("order_id")]
        public Guid OrderId { get; set; }

        [Required]
        [MaxLength(30)]
        [Column("order_number")]
        public string OrderNumber { get; set; } = null!;

        [Column("user_id")]
        public Guid? UserId { get; set; }

        [Column("shipping_address_id")]
        public Guid? ShippingAddressId { get; set; }

        [Column("billing_address_id")]
        public Guid? BillingAddressId { get; set; }

        [MaxLength(30)]
        [Column("status")]
        public string Status { get; set; } = "PENDING";

        [MaxLength(10)]
        [Column("currency")]
        public string Currency { get; set; } = "VND";

        [Column("subtotal")]
        public decimal Subtotal { get; set; } = 0;

        [Column("discount_total")]
        public decimal DiscountTotal { get; set; } = 0;

        [Column("tax_total")]
        public decimal TaxTotal { get; set; } = 0;

        [Column("shipping_total")]
        public decimal ShippingTotal { get; set; } = 0;

        [Column("grand_total")]
        public decimal GrandTotal { get; set; } = 0;

        [MaxLength(1000)]
        [Column("notes")]
        public string? Notes { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_by")]
        public Guid? UpdatedBy { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }

        [ForeignKey("ShippingAddressId")]
        public Address? ShippingAddress { get; set; }

        [ForeignKey("BillingAddressId")]
        public Address? BillingAddress { get; set; }

        [ForeignKey("UpdatedBy")]
        public User? UpdatedByUser { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public ICollection<OrderPromotion> OrderPromotions { get; set; } = new List<OrderPromotion>();
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
        public ICollection<Fulfillment> Fulfillments { get; set; } = new List<Fulfillment>();
    }
}
