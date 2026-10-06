namespace DesignABear.Services.DTOs.Requests
{
    public class UpdateReviewReplyRequest
    {
        public Guid StaffUserId { get; set; }
        public string Content { get; set; } = null!;
    }
}
