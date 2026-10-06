using AutoMapper;
using DesignABear.Repositories.Models;
using DesignABear.Repositories.Paging;
using DesignABear.Repositories.UnitOfWork;
using DesignABear.Services.DTOs.Requests;
using DesignABear.Services.DTOs.Responses;
using DesignABear.Services.Exceptions;
using DesignABear.Services.Services.Interfaces;

namespace DesignABear.Services.Services.Implementations
{
    public class ProductIssueReportService : IProductIssueReportService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public ProductIssueReportService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<PagedResult<ProductIssueReportResponse>> GetPagedReportsAsync(int pageIndex, int pageSize, string? status = null, Guid? userId = null, Guid? productId = null, int? currentRoleId = null)
        {
            var pagedReports = await _uow.ProductIssueReports.GetPagedReportsAsync(pageIndex, pageSize, status, userId, productId);
            var responseList = _mapper.Map<List<ProductIssueReportResponse>>(pagedReports.Items);
            
            if (currentRoleId.HasValue)
            {
                foreach (var resp in responseList) SecureResponse(resp, currentRoleId.Value);
            }

            return new PagedResult<ProductIssueReportResponse>
            {
                PageIndex = pagedReports.PageIndex,
                PageSize = pagedReports.PageSize,
                TotalCount = pagedReports.TotalCount,
                Items = responseList
            };
        }

        public async Task<ProductIssueReportResponse?> GetByIdAsync(Guid reportId, int currentRoleId)
        {
            var report = await _uow.ProductIssueReports.GetByIdAsync(reportId);
            if (report == null) throw new NotFoundException("ProductIssueReport", reportId);

            var response = _mapper.Map<ProductIssueReportResponse>(report);
            SecureResponse(response, currentRoleId);
            return response;
        }

        public async Task<IEnumerable<ProductIssueReportResponse>> GetByUserIdAsync(Guid userId, int currentRoleId)
        {
            var reports = await _uow.ProductIssueReports.FindAsync(r => r.UserId == userId);
            var responseList = _mapper.Map<IEnumerable<ProductIssueReportResponse>>(reports).ToList();
            foreach (var resp in responseList) SecureResponse(resp, currentRoleId);
            return responseList;
        }

        public async Task<IEnumerable<ProductIssueReportResponse>> GetByStatusAsync(string status, int currentRoleId)
        {
            var reports = await _uow.ProductIssueReports.FindAsync(r => r.Status == status);
            var responseList = _mapper.Map<IEnumerable<ProductIssueReportResponse>>(reports).ToList();
            foreach (var resp in responseList) SecureResponse(resp, currentRoleId);
            return responseList;
        }

        public async Task<ProductIssueReportResponse> CreateAsync(CreateProductIssueReportRequest request, Guid userId)
        {
            // 1. Validate OrderItem exists
            var orderItem = await _uow.OrderItems.GetByIdAsync(request.OrderItemId);
            if (orderItem == null) throw new NotFoundException("OrderItem", request.OrderItemId);

            // 2. Prevent duplicate active reports
            var existingReport = await _uow.ProductIssueReports.FindAsync(r => 
                r.OrderItemId == request.OrderItemId && 
                (r.Status == "PENDING" || r.Status == "PROCESSING"));
            
            if (existingReport.Any())
            {
                throw new BadRequestException("A report is already being processed for this item.");
            }

            // 3. Fetch ProductId
            Guid productId = orderItem.ProductId ?? Guid.Empty;
            
            if (productId == Guid.Empty && orderItem.BuildId.HasValue)
            {
                var build = await _uow.Builds.GetByIdAsync(orderItem.BuildId.Value);
                if (build != null)
                {
                    productId = build.BaseProductId;
                }
            }
            
            if (productId == Guid.Empty) throw new BadRequestException("Could not determine product for the given order item.");

            // 4. Map and Create
            var report = _mapper.Map<ProductIssueReport>(request);
            report.UserId = userId;
            report.ProductId = productId;
            report.Status = "PENDING";
            report.CreatedAt = DateTime.UtcNow;
            report.UpdatedAt = DateTime.UtcNow;

            await _uow.ProductIssueReports.AddAsync(report);
            await _uow.SaveChangesAsync();

            return _mapper.Map<ProductIssueReportResponse>(report);
        }

        public async Task AssignAsync(Guid reportId, Guid staffId)
        {
            var report = await _uow.ProductIssueReports.GetByIdAsync(reportId);
            if (report == null) throw new NotFoundException("ProductIssueReport", reportId);

            report.AssignedStaffId = staffId;
            report.Status = "PROCESSING";
            report.UpdatedAt = DateTime.UtcNow;

            _uow.ProductIssueReports.Update(report);
            await _uow.SaveChangesAsync();
        }

        public async Task<ProductIssueReportResponse> ResolveAsync(Guid reportId, ResolveProductIssueReportRequest request, Guid staffId)
        {
            var report = await _uow.ProductIssueReports.GetByIdAsync(reportId);
            if (report == null) throw new NotFoundException("ProductIssueReport", reportId);

            report.Resolution = request.Resolution;
            report.StaffNotes = request.StaffNotes;
            report.RepairNotes = request.RepairNotes;
            report.ResolvedAt = DateTime.UtcNow;
            report.Status = "RESOLVED";
            report.UpdatedAt = DateTime.UtcNow;

            _uow.ProductIssueReports.Update(report);
            await _uow.SaveChangesAsync();

            return _mapper.Map<ProductIssueReportResponse>(report);
        }

        public async Task RejectAsync(Guid reportId, RejectProductIssueReportRequest request, Guid staffId)
        {
            var report = await _uow.ProductIssueReports.GetByIdAsync(reportId);
            if (report == null) throw new NotFoundException("ProductIssueReport", reportId);

            report.Status = "REJECTED";
            report.StaffNotes = (report.StaffNotes ?? "") + "\nRejection Reason: " + request.Reason;
            report.ResolvedAt = DateTime.UtcNow;
            report.UpdatedAt = DateTime.UtcNow;

            _uow.ProductIssueReports.Update(report);
            await _uow.SaveChangesAsync();
        }

        public async Task CompleteAsync(Guid reportId, Guid staffId, CompleteProductIssueReportRequest request)
        {
            var report = await _uow.ProductIssueReports.GetByIdAsync(reportId);
            if (report == null) throw new NotFoundException("ProductIssueReport", reportId);

            report.Status = "CLOSED";
            report.UpdatedAt = DateTime.UtcNow;
            
            // If FinalNotes added to model later, we can update it here.
            // For now, we follow the core requirement of moving status to CLOSED.
            if (!string.IsNullOrEmpty(request.FinalNotes))
            {
                report.StaffNotes = (report.StaffNotes ?? "") + "\nFinal Notes: " + request.FinalNotes;
            }

            _uow.ProductIssueReports.Update(report);
            await _uow.SaveChangesAsync();
        }

        private void SecureResponse(ProductIssueReportResponse response, int roleId)
        {
            if (roleId == 3) // Customer
            {
                response.StaffNotes = null;
                response.RepairNotes = null;
            }
        }
    }
}
