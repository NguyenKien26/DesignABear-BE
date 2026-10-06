using DesignABear.Services.DTOs.Requests;
using DesignABear.Services.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DesignABear.Repositories.Paging;

namespace DesignABear.Services.Services.Interfaces
{
    public interface IProductIssueReportService
    {
        Task<PagedResult<ProductIssueReportResponse>> GetPagedReportsAsync(int pageIndex, int pageSize, string? status = null, Guid? userId = null, Guid? productId = null, int? currentRoleId = null);
        Task<ProductIssueReportResponse?> GetByIdAsync(Guid reportId, int currentRoleId);
        Task<IEnumerable<ProductIssueReportResponse>> GetByUserIdAsync(Guid userId, int currentRoleId);
        Task<IEnumerable<ProductIssueReportResponse>> GetByStatusAsync(string status, int currentRoleId);
        Task<ProductIssueReportResponse> CreateAsync(CreateProductIssueReportRequest request, Guid userId);
        Task AssignAsync(Guid reportId, Guid staffId);
        Task<ProductIssueReportResponse> ResolveAsync(Guid reportId, ResolveProductIssueReportRequest request, Guid staffId);
        Task RejectAsync(Guid reportId, RejectProductIssueReportRequest request, Guid staffId);
        Task CompleteAsync(Guid reportId, Guid staffId, CompleteProductIssueReportRequest request);
    }
}
