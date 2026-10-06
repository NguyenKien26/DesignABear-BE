namespace DesignABear.Services.DTOs.Responses
{
    public class BearDeviceResponse
    {
        public Guid DeviceId { get; set; }
        public string SerialNumber { get; set; } = null!;
        public string Model { get; set; } = null!;
    }
}
