namespace DesignABear.Services.DTOs.Requests
{
    public class AssignProductionPartRequest
    {
        public string PartType { get; set; } = null!;
        public Guid TechnicianId { get; set; }
    }
}