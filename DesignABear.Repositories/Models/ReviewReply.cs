using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DesignABear.Repositories.Models
{
    [Table("review_replies")]
    public class ReviewReply
    {
        [Key]
        [Column("reply_id")]
        public Guid ReplyId { get; set; }

        [Required]
        [Column("review_id")]
        public Guid ReviewId { get; set; }

        [Required]
        [Column("staff_user_id")]
        public Guid StaffUserId { get; set; }

        [Required]
        [Column("content")]
        public string Content { get; set; } = null!;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("ReviewId")]
        public Review Review { get; set; } = null!;

        [ForeignKey("StaffUserId")]
        public User StaffUser { get; set; } = null!;
    }
}
