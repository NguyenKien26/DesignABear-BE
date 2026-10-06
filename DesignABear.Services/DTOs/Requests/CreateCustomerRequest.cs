namespace DesignABear.Services.DTOs.Requests
{
    public class CreateCustomerRequest
    {
        public Guid? UserId { get; set; }
        public string? Phone { get; set; }
    }
}
