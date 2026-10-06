namespace DesignABear.Services.DTOs.Requests
{
    public class CreateBearDeviceRequest
    {
        public string SerialNumber { get; set; } = null!;
        public string Model { get; set; } = null!;
    }

    public class UpdateBearDeviceRequest
    {
        public string Model { get; set; } = null!;
    }
}
