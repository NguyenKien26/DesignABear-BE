namespace DesignABear.Services.DTOs.Responses
{
    public class ValidateDesignResponse
    {
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }
}
