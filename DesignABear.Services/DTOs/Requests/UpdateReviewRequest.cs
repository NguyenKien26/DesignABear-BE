namespace DesignABear.Services.DTOs.Requests
{
    public class UpdateReviewRequest
    {
        public int Rating { get; set; }
        public string? Title { get; set; }
        public string? Body { get; set; }
        
    }
}
