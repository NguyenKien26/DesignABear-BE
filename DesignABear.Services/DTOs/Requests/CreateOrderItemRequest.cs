namespace DesignABear.Services.DTOs.Requests
{
    public class CreateOrderItemRequest
    {
        public Guid OrderId { get; set; }
        public Guid? ProductId { get; set; }
        public Guid? BuildId { get; set; }
        public int Quantity { get; set; } = 1;
        public decimal UnitPrice { get; set; }
        public decimal LineTotal { get; set; }
        public string? ProductNameSnapshot { get; set; }
        public string? PersonalizationSnapshot { get; set; }
    }
}
