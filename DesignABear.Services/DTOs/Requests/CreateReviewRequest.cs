namespace DesignABear.Services.DTOs.Requests
{
    public class CreateReviewRequest
    {
        public Guid ProductId { get; set; }
        public Guid? UserId { get; set; }
        public Guid? OrderItemId { get; set; }
        public int Rating { get; set; }
        public string? Title { get; set; }
        public string? Body { get; set; }
    }
}
