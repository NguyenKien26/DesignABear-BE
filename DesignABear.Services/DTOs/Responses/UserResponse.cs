namespace DesignABear.Services.DTOs.Responses
{
    public class UserResponse
    {
        public Guid UserId { get; set; }
        public string Email { get; set; } = null!;
        public string? FullName { get; set; }
        public string? AvatarUrl { get; set; }
        public string Provider { get; set; } = null!;
        public string Status { get; set; } = null!;
        public string RoleName { get; set; } = null!;
        public string? WorkplaceName { get; set; }
        public int TotalJobsCompleted { get; set; }
        public decimal TotalEarned { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
