using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DesignABear.Repositories.Models
{
    [Table("favorites")]
    public class Favorite
    {
        [Key]
        [Column("favorite_id")]
        public Guid FavoriteId { get; set; }

        [Required]
        [Column("user_id")]
        public Guid UserId { get; set; }

        [Required]
        [Column("product_id")]
        public Guid ProductId { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; } = null!;
    }
}
