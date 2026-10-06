namespace DesignABear.Services.DTOs.Responses
{
    public class CalculatePriceResponse
    {
        public decimal BasePrice { get; set; }
        public decimal AddonPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
