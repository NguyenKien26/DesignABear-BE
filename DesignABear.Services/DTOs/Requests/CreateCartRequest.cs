namespace DesignABear.Services.DTOs.Requests
{
    public class CreateCartRequest
    {
        public Guid? UserId { get; set; }
        public string Currency { get; set; } = "VND";
    }
}
