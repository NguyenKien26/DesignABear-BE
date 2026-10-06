using DesignABear.Repositories.Paging;
using DesignABear.Services.DTOs.Requests;
using DesignABear.Services.DTOs.Responses;

namespace DesignABear.Services.Services.Interfaces
{
    public interface IPersonalizationRuleService
    {
        Task<PagedResult<PersonalizationRuleResponse>> GetPagedRulesAsync(int pageIndex, int pageSize, Guid? baseProductId = null, Guid? addonProductId = null, Guid? groupId = null);
        Task<IEnumerable<PersonalizationRuleResponse>> GetByBaseProductAsync(Guid baseProductId);
        Task<IEnumerable<PersonalizationRuleResponse>> GetActiveRulesForProductAsync(Guid baseProductId);
        Task<PersonalizationRuleResponse?> GetByIdAsync(Guid ruleId);
        Task<PersonalizationRuleResponse> CreateAsync(CreatePersonalizationRuleRequest request);
        Task UpdateAsync(Guid ruleId, UpdatePersonalizationRuleRequest request);
        Task DeleteAsync(Guid ruleId);
        Task<CalculatePriceResponse> CalculatePriceAsync(CustomizeDesignRequest request);
        Task<ValidateDesignResponse> ValidateDesignAsync(CustomizeDesignRequest request);
        Task<string?> GetCombinedImageUrlAsync(Guid baseProductId, List<Guid> accessoryProductIds);
    }
}
