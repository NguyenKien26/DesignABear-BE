using DesignABear.Services.DTOs.Responses;

namespace DesignABear.Services.Services.Interfaces
{
    public interface IReportService
    {
        Task<RevenueReportResponse> GetRevenueReportAsync(DateTime startDate, DateTime endDate);
        Task<PayrollReportResponse> GetPayrollReportAsync(DateTime startDate, DateTime endDate);
        Task<PayrollUserResponse> GetUserPayrollAsync(Guid userId, DateTime startDate, DateTime endDate);
    }
}
