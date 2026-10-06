using Microsoft.EntityFrameworkCore;
using DesignABear.Repositories.Data;
using DesignABear.Repositories.Models;
using DesignABear.Repositories.Repositories.Interfaces;

using DesignABear.Repositories.Paging;

namespace DesignABear.Repositories.Repositories.Implementations
{
    public class PersonalizationRuleRepository : GenericRepository<PersonalizationRule>, IPersonalizationRuleRepository
    {
        public PersonalizationRuleRepository(AppDbContext context) : base(context) { }

        public async Task<PagedResult<PersonalizationRule>> GetPagedRulesAsync(int pageIndex, int pageSize, Guid? baseProductId = null, Guid? addonProductId = null, Guid? groupId = null)
        {
            IQueryable<PersonalizationRule> query = _dbSet
                .Include(r => r.BaseProduct)
                .Include(r => r.AddonProduct)
                .Include(r => r.Group);

            if (baseProductId.HasValue)
                query = query.Where(r => r.BaseProductId == baseProductId.Value);

            if (addonProductId.HasValue)
                query = query.Where(r => r.AddonProductId == addonProductId.Value);

            if (groupId.HasValue)
                query = query.Where(r => r.GroupId == groupId.Value);

            var totalCount = await query.CountAsync();
            var items = await query
                .OrderBy(r => r.BaseProduct.Name)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<PersonalizationRule>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = totalCount,
                Items = items
            };
        }

        public async Task<IEnumerable<PersonalizationRule>> GetByBaseProductAsync(Guid baseProductId)
            => await _dbSet
                .Include(r => r.Group)
                .Include(r => r.AddonProduct)
                .Where(r => r.BaseProductId == baseProductId)
                .ToListAsync();

        public async Task<IEnumerable<PersonalizationRule>> GetActiveRulesForProductAsync(Guid baseProductId)
            => await _dbSet
                .Include(r => r.Group)
                .Include(r => r.AddonProduct)
                .Where(r => r.BaseProductId == baseProductId && r.IsActive)
                .ToListAsync();
    }
}
