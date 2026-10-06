using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DesignABear.Repositories.Models
{
    [Table("fulfillments")]
    public class Fulfillment
    {
        [Key]
        [Column("fulfillment_id")]
        public Guid FulfillmentId { get; set; }

        [Required]
        [Column("order_id")]
        public Guid OrderId { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("carrier")]
        public string Carrier { get; set; } = null!;

        [Column("location_id")]
        public Guid? LocationId { get; set; }

        [MaxLength(30)]
        [Column("status")]
        public string Status { get; set; } = "PENDING";

        [MaxLength(120)]
        [Column("tracking_number")]
        public string? TrackingNumber { get; set; }

        [Column("shipped_at")]
        public DateTime? ShippedAt { get; set; }

        [Column("delivered_at")]
        public DateTime? DeliveredAt { get; set; }

        [Column("estimated_delivery_at")]
        public DateTime? EstimatedDeliveryAt { get; set; }

        [Column("actual_delivery_at")]
        public DateTime? ActualDeliveryAt { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("OrderId")]
        public Order Order { get; set; } = null!;

        [ForeignKey("LocationId")]
        public Location? Location { get; set; }
    }
}
