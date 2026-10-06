namespace DesignABear.Services.DTOs.Responses
{
    public class CartResponse
    {
        public Guid CartId { get; set; }
        public Guid? UserId { get; set; }
        public string Currency { get; set; } = "VND";   
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public IEnumerable<CartItemResponse> CartItems { get; set; } = new List<CartItemResponse>();
    }
}
