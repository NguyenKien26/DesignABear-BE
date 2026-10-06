namespace DesignABear.Services.DTOs.Responses
{
    public class GoogleAuthResponse
    {
        public bool IsNewUser { get; set; }
        public string? Token { get; set; }
        public string? RegistrationToken { get; set; }
        public string? Email { get; set; }
        public string? FullName { get; set; }
        public string? AvatarUrl { get; set; }
    }
}
