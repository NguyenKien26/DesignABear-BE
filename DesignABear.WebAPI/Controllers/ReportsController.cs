using DesignABear.Services.Common;
using DesignABear.Services.DTOs.Responses;
using DesignABear.Services.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DesignABear.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReportsController : BaseController
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("revenue")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> GetRevenueReport([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            var start = DateTime.SpecifyKind(startDate ?? DateTime.UtcNow.AddDays(-30), DateTimeKind.Utc);
            var end = DateTime.SpecifyKind(endDate ?? DateTime.UtcNow, DateTimeKind.Utc);
            
            var report = await _reportService.GetRevenueReportAsync(start, end);
            return HandleResult(Result<RevenueReportResponse>.Success(report));
        }

        [HttpGet("payroll")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> GetPayrollReport([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            var start = DateTime.SpecifyKind(startDate ?? DateTime.UtcNow.AddDays(-30), DateTimeKind.Utc);
            var end = DateTime.SpecifyKind(endDate ?? DateTime.UtcNow, DateTimeKind.Utc);

            var report = await _reportService.GetPayrollReportAsync(start, end);
            return HandleResult(Result<PayrollReportResponse>.Success(report));
        }

        [HttpGet("payroll/me")]
        public async Task<IActionResult> GetMyPayroll([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            var start = DateTime.SpecifyKind(startDate ?? DateTime.UtcNow.AddDays(-30), DateTimeKind.Utc);
            var end = DateTime.SpecifyKind(endDate ?? DateTime.UtcNow, DateTimeKind.Utc);

            var userId = GetCurrentUserId();
            var report = await _reportService.GetUserPayrollAsync(userId, start, end);
            return HandleResult(Result<PayrollUserResponse>.Success(report));
        }

        [HttpGet("payroll/user/{userId}")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> GetPayrollByUser(Guid userId, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            var start = DateTime.SpecifyKind(startDate ?? DateTime.UtcNow.AddDays(-30), DateTimeKind.Utc);
            var end = DateTime.SpecifyKind(endDate ?? DateTime.UtcNow, DateTimeKind.Utc);

            var report = await _reportService.GetUserPayrollAsync(userId, start, end);
            return HandleResult(Result<PayrollUserResponse>.Success(report));
        }
    }
}
