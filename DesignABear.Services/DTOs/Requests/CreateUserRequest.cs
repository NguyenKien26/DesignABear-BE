namespace DesignABear.Services.DTOs.Requests
{
    public class CreateUserRequest
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? FullName { get; set; }
    }
}
