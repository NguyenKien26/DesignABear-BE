namespace DesignABear.Services.DTOs.Requests
{
    public class UpdateUserRequest
    {
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? Language { get; set; }
        public string? Timezone { get; set; }
        public string? AvatarUrl { get; set; }
        public string? Status { get; set; }
    }
}
