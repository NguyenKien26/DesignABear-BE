namespace DesignABear.Services.DTOs.Requests
{
    public class QuickRegisterRequest
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public int RoleId { get; set; } = 3; // Default to Customer
        public Guid? WorkplaceId { get; set; }
    }
}
