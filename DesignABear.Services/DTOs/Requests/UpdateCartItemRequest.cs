namespace DesignABear.Services.DTOs.Requests
{
    public class UpdateCartItemRequest
    {
        public int Quantity { get; set; }
        public bool? IncludesSmartChip { get; set; }
    }
}
