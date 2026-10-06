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
    public class PersonalizationRuleService : IPersonalizationRuleService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public PersonalizationRuleService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<PagedResult<PersonalizationRuleResponse>> GetPagedRulesAsync(int pageIndex, int pageSize, Guid? baseProductId = null, Guid? addonProductId = null, Guid? groupId = null)
        {
            var pagedRules = await _uow.PersonalizationRules.GetPagedRulesAsync(pageIndex, pageSize, baseProductId, addonProductId, groupId);
            return new PagedResult<PersonalizationRuleResponse>
            {
                PageIndex = pagedRules.PageIndex,
                PageSize = pagedRules.PageSize,
                TotalCount = pagedRules.TotalCount,
                Items = _mapper.Map<List<PersonalizationRuleResponse>>(pagedRules.Items)
            };
        }

        public async Task<IEnumerable<PersonalizationRuleResponse>> GetByBaseProductAsync(Guid baseProductId)
        {
            var rules = await _uow.PersonalizationRules.GetByBaseProductAsync(baseProductId);
            return _mapper.Map<IEnumerable<PersonalizationRuleResponse>>(rules);
        }

        public async Task<IEnumerable<PersonalizationRuleResponse>> GetActiveRulesForProductAsync(Guid baseProductId)
        {
            var rules = await _uow.PersonalizationRules.GetActiveRulesForProductAsync(baseProductId);
            return _mapper.Map<IEnumerable<PersonalizationRuleResponse>>(rules);
        }

        public async Task<PersonalizationRuleResponse?> GetByIdAsync(Guid ruleId)
        {
            var rule = await _uow.PersonalizationRules.GetByIdAsync(ruleId);
            if (rule == null) throw new NotFoundException("PersonalizationRule", ruleId);
            return _mapper.Map<PersonalizationRuleResponse>(rule);
        }

        public async Task<PersonalizationRuleResponse> CreateAsync(CreatePersonalizationRuleRequest request)
        {
            // Check for existing rule to avoid unique constraint violation
            var existingRules = await _uow.PersonalizationRules.GetByBaseProductAsync(request.BaseProductId);
            var duplicate = existingRules.FirstOrDefault(r => 
                r.AddonProductId == request.AllowedComponentProductId && 
                r.GroupId == request.GroupId);

            if (duplicate != null)
            {
                return _mapper.Map<PersonalizationRuleResponse>(duplicate);
            }

            var rule = _mapper.Map<PersonalizationRule>(request);
            rule.RuleId = Guid.NewGuid();
            
            await _uow.PersonalizationRules.AddAsync(rule);
            await _uow.SaveChangesAsync();
            
            return _mapper.Map<PersonalizationRuleResponse>(rule);
        }

        public async Task UpdateAsync(Guid ruleId, UpdatePersonalizationRuleRequest request)
        {
            var rule = await _uow.PersonalizationRules.GetByIdAsync(ruleId);
            if (rule == null) throw new NotFoundException("PersonalizationRule", ruleId);
            
            _mapper.Map(request, rule);
            _uow.PersonalizationRules.Update(rule);
            await _uow.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid ruleId)
        {
            var rule = await _uow.PersonalizationRules.GetByIdAsync(ruleId);
            if (rule is not null)
            {
                _uow.PersonalizationRules.Remove(rule);
                await _uow.SaveChangesAsync();
            }
        }

        public async Task<CalculatePriceResponse> CalculatePriceAsync(CustomizeDesignRequest request)
        {
            var baseProduct = await _uow.Products.GetWithDetailsAsync(request.BaseProductId);
            if (baseProduct == null) throw new NotFoundException("Product", request.BaseProductId);

            // Get Base Price (Assuming base product has variants, taking the first or lowest price)
            var basePrice = baseProduct.Variants.FirstOrDefault()?.Price ?? 0;

            decimal addonPrice = 0;
            var validatedRules = await _uow.PersonalizationRules.GetActiveRulesForProductAsync(request.BaseProductId);

            foreach (var ruleId in request.SelectedRuleIds)
            {
                var ruleInfo = validatedRules.FirstOrDefault(r => r.RuleId == ruleId);
                // If it's a valid rule for this base product, add the addon product's price
                if (ruleInfo != null)
                {
                    var addonProduct = await _uow.Products.GetWithDetailsAsync(ruleInfo.AddonProductId);
                    addonPrice += addonProduct?.Variants.FirstOrDefault()?.Price ?? 0;
                }
            }

            return new CalculatePriceResponse
            {
                BasePrice = basePrice,
                AddonPrice = addonPrice,
                TotalPrice = basePrice + addonPrice
            };
        }

        public async Task<ValidateDesignResponse> ValidateDesignAsync(CustomizeDesignRequest request)
        {
            var errors = new List<string>();
            var baseProduct = await _uow.Products.GetByIdAsync(request.BaseProductId);
            if (baseProduct == null)
            {
                errors.Add($"Base product with ID {request.BaseProductId} not found.");
                return new ValidateDesignResponse { IsValid = false, Errors = errors };
            }

            var activeRulesForProduct = await _uow.PersonalizationRules.GetActiveRulesForProductAsync(request.BaseProductId);
            var activeRuleIds = activeRulesForProduct.Select(r => r.RuleId).ToList();

            var ruleUsageCount = new Dictionary<Guid, int>();

            foreach (var requestedRuleId in request.SelectedRuleIds)
            {
                if (!activeRuleIds.Contains(requestedRuleId))
                {
                    errors.Add($"Rule {requestedRuleId} is not valid or active for this base product.");
                    continue;
                }

                if (!ruleUsageCount.ContainsKey(requestedRuleId))
                    ruleUsageCount[requestedRuleId] = 0;

                ruleUsageCount[requestedRuleId]++;

                var ruleDetail = activeRulesForProduct.First(r => r.RuleId == requestedRuleId);
                if (ruleUsageCount[requestedRuleId] > ruleDetail.MaxQty)
                {
                    errors.Add($"Rule {requestedRuleId} exceeds the maximum allowed quantity of {ruleDetail.MaxQty}.");
                }
            }

            return new ValidateDesignResponse
            {
                IsValid = !errors.Any(),
                Errors = errors
            };
        }

        public async Task<string?> GetCombinedImageUrlAsync(Guid baseProductId, List<Guid> accessoryProductIds)
        {
            if (accessoryProductIds == null || !accessoryProductIds.Any())
            {
                return null;
            }

            // Sort IDs to ensure consistent combination key (e.g. A,B is same as B,A)
            var sortedIds = accessoryProductIds.OrderBy(id => id).Select(id => id.ToString()).ToList();
            var combinationKey = string.Join("|", sortedIds);

            var combo = await _uow.ProductComboImages.GetByCombinationKeyAsync(baseProductId, combinationKey);
            return combo?.ImageUrl;
        }
    }
}
