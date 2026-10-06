namespace DesignABear.Services.Configurations
{
    public class BusinessSettings
    {
        public string DefaultWarehouseId { get; set; } = string.Empty;
        public string AiProcessorProductId { get; set; } = string.Empty;

        public Guid WarehouseGuid => Guid.Parse(DefaultWarehouseId);
        public Guid AiProcessorGuid => Guid.Parse(AiProcessorProductId);
    }
}
