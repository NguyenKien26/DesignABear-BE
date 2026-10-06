namespace DesignABear.Services.DTOs.Requests
{
    public class CreateBuildComponentRequest
    {
        public Guid BuildId { get; set; }
        public Guid GroupId { get; set; }
        public Guid OptionProductId { get; set; }
        public int Quantity { get; set; } = 1;
        public decimal? AdditionalPrice { get; set; }
    }
}
