namespace DesignABear.Services.DTOs.Requests
{
    public class RefundDecisionRequest
    {
        public string? AdminNotes { get; set; }
        public bool IsDefectVerified { get; set; } = false;
    }
}
