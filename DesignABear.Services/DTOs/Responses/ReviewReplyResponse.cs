namespace DesignABear.Services.DTOs.Responses
{
    public class ReviewReplyResponse
    {
        public Guid ReplyId { get; set; }
        public Guid ReviewId { get; set; }
        public Guid StaffUserId { get; set; }
        public string Content { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
