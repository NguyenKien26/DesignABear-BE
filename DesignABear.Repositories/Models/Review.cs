using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DesignABear.Repositories.Models
{
    [Table("reviews")]
    public class Review
    {
        [Key]
        [Column("review_id")]
        public Guid ReviewId { get; set; }

        [Required]
        [Column("product_id")]
        public Guid ProductId { get; set; }

        [Column("user_id")]
        public Guid? UserId { get; set; }

        [Column("order_item_id")]
        public Guid? OrderItemId { get; set; }

        [Required]
        [Column("rating")]
        public int Rating { get; set; }

        [MaxLength(200)]
        [Column("title")]
        public string? Title { get; set; }

        [Column("body")]
        public string? Body { get; set; }

        [MaxLength(20)]
        [Column("status")]
        public string Status { get; set; } = "PUBLISHED";

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("ProductId")]
        public Product Product { get; set; } = null!;

        [ForeignKey("UserId")]
        public User? User { get; set; }

        [ForeignKey("OrderItemId")]
        public OrderItem? OrderItem { get; set; }

        public ICollection<ReviewReply> ReviewReplies { get; set; } = new List<ReviewReply>();
    }
}
