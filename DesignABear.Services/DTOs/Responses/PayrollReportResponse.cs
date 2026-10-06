namespace DesignABear.Services.DTOs.Responses
{
    public class PayrollReportResponse
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public decimal TotalCraftsmanPayroll { get; set; }
        public decimal TotalQCPayroll { get; set; }
        public decimal TotalStaffPayroll { get; set; }
        public decimal TotalPayroll { get; set; }

        public int TotalFinishedJobs { get; set; }

        public List<PayrollEmployeeLine> Employees { get; set; } = new();
        public List<DailyPayroll> DailyBreakdown { get; set; } = new();
    }

    public class PayrollEmployeeLine
    {
        public Guid EmployeeId { get; set; }
        public string EmployeeName { get; set; } = "N/A";
        public string? Email { get; set; }

        public int JobsCount { get; set; }
        public decimal CraftsmanAmount { get; set; }
        public decimal QCAmount { get; set; }
        public decimal StaffAmount { get; set; }
        public decimal TotalAmount { get; set; }
    }

    public class DailyPayroll
    {
        public DateTime Date { get; set; }
        public int FinishedJobs { get; set; }

        public decimal CraftsmanPayroll { get; set; }
        public decimal QCPayroll { get; set; }
        public decimal StaffPayroll { get; set; }
        public decimal TotalPayroll { get; set; }
    }
}