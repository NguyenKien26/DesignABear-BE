using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DesignABear.Repositories.Models
{
    [Table("carts")]
    public class Cart
    {
        [Key]
        [Column("cart_id")]
        public Guid CartId { get; set; }

        [Column("user_id")]
        public Guid? UserId { get; set; }

        [MaxLength(10)]
        [Column("currency")]
        public string Currency { get; set; } = "VND";

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("UserId")]
        public User? User { get; set; }

        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
