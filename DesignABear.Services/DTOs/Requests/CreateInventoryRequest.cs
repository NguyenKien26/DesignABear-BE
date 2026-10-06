namespace DesignABear.Services.DTOs.Requests
{
    public class CreateInventoryRequest
    {
        public Guid LocationId { get; set; }
        public Guid ProductId { get; set; }
        public int OnHand { get; set; }
        public int Reserved { get; set; }
    }
}
