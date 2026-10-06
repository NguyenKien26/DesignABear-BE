namespace DesignABear.Services.DTOs.Requests
{
    public class CreateReviewReplyRequest
    {
        public Guid ReviewId { get; set; }
        public Guid StaffUserId { get; set; }
        public string Content { get; set; } = null!;
    }
}
