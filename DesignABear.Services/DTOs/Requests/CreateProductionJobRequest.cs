namespace DesignABear.Services.DTOs.Requests
{
    public class CreateProductionJobRequest
    {
        public Guid OrderItemId { get; set; }
        public Guid? AssignedUser { get; set; }
    }
}
