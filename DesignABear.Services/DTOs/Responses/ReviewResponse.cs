namespace DesignABear.Services.DTOs.Responses
{
    public class ReviewResponse
    {
        public Guid ReviewId { get; set; }
        public Guid ProductId { get; set; }
        public Guid? UserId { get; set; }
        public string? AuthorName { get; set; }
        public string? AuthorAvatar { get; set; }
        public Guid? OrderItemId { get; set; }
        public int Rating { get; set; }
        public string? Title { get; set; }
        public string? Body { get; set; }
        public string Status { get; set; } = "PUBLISHED";
        public DateTime CreatedAt { get; set; }

        public IEnumerable<ReviewReplyResponse> ReviewReplies { get; set; } = new List<ReviewReplyResponse>();
    }
}
