namespace DesignABear.Services.DTOs.Requests
{
    public class StockReservationItem
    {
        public Guid ProductId { get; set; }
        public Guid IdentityId { get; set; }
        public bool IsAccessory { get; set; }
        public int Quantity { get; set; }
        public string? ProductName { get; set; } // For error reporting
    }
}
