using DesignABear.Repositories.Paging;
using DesignABear.Services.Common;
using DesignABear.Services.DTOs.Requests;
using DesignABear.Services.DTOs.Responses;
using DesignABear.Services.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DesignABear.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductIssueReportsController : BaseController
    {
        private readonly IProductIssueReportService _reportService;

        public ProductIssueReportsController(IProductIssueReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet]
        [Authorize(Roles = "1,2")]
        public async Task<IActionResult> GetAll([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10, [FromQuery] string? status = null, [FromQuery] Guid? userId = null, [FromQuery] Guid? productId = null)
        {
            var roleId = GetCurrentRoleId();
            var pagedResult = await _reportService.GetPagedReportsAsync(pageIndex, pageSize, status, userId, productId, roleId);
            return HandleResult(Result<PagedResult<ProductIssueReportResponse>>.Success(pagedResult));
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(Guid id)
        {
            var roleId = GetCurrentRoleId();
            var report = await _reportService.GetByIdAsync(id, roleId);
            return HandleResult(Result<ProductIssueReportResponse>.Success(report!));
        }

        [HttpGet("my")]
        [Authorize]
        public async Task<IActionResult> GetMyReports()
        {
            var userId = GetCurrentUserId();
            var roleId = GetCurrentRoleId();
            var reports = await _reportService.GetByUserIdAsync(userId, roleId);
            return HandleResult(Result<IEnumerable<ProductIssueReportResponse>>.Success(reports));
        }

        [HttpGet("status/{status}")]
        [Authorize(Roles = "1,2")]
        public async Task<IActionResult> GetByStatus(string status)
        {
            var roleId = GetCurrentRoleId();
            var reports = await _reportService.GetByStatusAsync(status, roleId);
            return HandleResult(Result<IEnumerable<ProductIssueReportResponse>>.Success(reports));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateProductIssueReportRequest request)
        {
            var userId = GetCurrentUserId();
            var report = await _reportService.CreateAsync(request, userId);
            return CreatedAtAction(nameof(GetById), new { id = report.ReportId }, Result<ProductIssueReportResponse>.Success(report));
        }

        [HttpPut("{id}/assign")]
        [Authorize(Roles = "1,2")]
        public async Task<IActionResult> Assign(Guid id)
        {
            var staffId = GetCurrentUserId();
            await _reportService.AssignAsync(id, staffId);
            return HandleResult(Result.Success());
        }

        [HttpPut("{id}/resolve")]
        [Authorize(Roles = "1,2")]
        public async Task<IActionResult> Resolve(Guid id, [FromBody] ResolveProductIssueReportRequest request)
        {
            var staffId = GetCurrentUserId();
            var report = await _reportService.ResolveAsync(id, request, staffId);
            return HandleResult(Result<ProductIssueReportResponse>.Success(report));
        }

        [HttpPut("{id}/reject")]
        [Authorize(Roles = "1,2")]
        public async Task<IActionResult> Reject(Guid id, [FromBody] RejectProductIssueReportRequest request)
        {
            var staffId = GetCurrentUserId();
            await _reportService.RejectAsync(id, request, staffId);
            return HandleResult(Result.Success());
        }

        [HttpPut("{id}/complete")]
        [Authorize(Roles = "1,2")]
        public async Task<IActionResult> Complete(Guid id, [FromBody] CompleteProductIssueReportRequest request)
        {
            var staffId = GetCurrentUserId();
            await _reportService.CompleteAsync(id, staffId, request);
            return HandleResult(Result.Success());
        }
    }
}
