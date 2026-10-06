namespace DesignABear.Services.DTOs.Responses
{
    public class PayrollUserResponse
    {
        public Guid UserId { get; set; }
        public string EmployeeName { get; set; } = "N/A";
        public string? Email { get; set; }
        public int RoleId { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int JobsCount { get; set; }
        public decimal CraftsmanAmount { get; set; }
        public decimal QCAmount { get; set; }
        public decimal StaffAmount { get; set; }
        public decimal TotalAmount { get; set; }
    }
}