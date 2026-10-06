using DesignABear.Repositories.Models;
using DesignABear.Repositories.Paging;

namespace DesignABear.Repositories.Repositories.Interfaces
{
    public interface IPersonalizationRuleRepository : IGenericRepository<PersonalizationRule>
    {
        Task<PagedResult<PersonalizationRule>> GetPagedRulesAsync(int pageIndex, int pageSize, Guid? baseProductId = null, Guid? addonProductId = null, Guid? groupId = null);
        Task<IEnumerable<PersonalizationRule>> GetByBaseProductAsync(Guid baseProductId);
        Task<IEnumerable<PersonalizationRule>> GetActiveRulesForProductAsync(Guid baseProductId);
    }
}
