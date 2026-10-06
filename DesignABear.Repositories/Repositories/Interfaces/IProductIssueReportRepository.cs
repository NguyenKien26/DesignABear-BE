using DesignABear.Repositories.Models;
using DesignABear.Repositories.Paging;

namespace DesignABear.Repositories.Repositories.Interfaces
{
    public interface IProductIssueReportRepository : IGenericRepository<ProductIssueReport>
    {
        Task<PagedResult<ProductIssueReport>> GetPagedReportsAsync(int pageIndex, int pageSize, string? status = null, Guid? userId = null, Guid? productId = null);
    }
}
