using Microsoft.EntityFrameworkCore;
using DesignABear.Repositories.Data;
using DesignABear.Repositories.Models;
using DesignABear.Repositories.Repositories.Interfaces;

namespace DesignABear.Repositories.Repositories.Implementations
{
    public class InventoryRepository : GenericRepository<Inventory>, IInventoryRepository
    {
        public InventoryRepository(AppDbContext context) : base(context) { }

        public async Task<Inventory?> GetByLocationAndIdentityAsync(Guid locationId, Guid identityId, bool isAccessory)
            => await _dbSet.FirstOrDefaultAsync(i => i.LocationId == locationId && i.IdentityId == identityId && i.IsAccessory == isAccessory);

        public async Task<IEnumerable<Inventory>> GetByLocationAndIdentitiesAsync(Guid locationId, IEnumerable<(Guid id, bool isAccessory)> identities)
        {
            var idList = identities.Select(x => x.id).ToList();
            // Since identity_id is shared, we filter by IDs first then refine in-memory or with complex ORs.
            // For efficiency with small batches, we can fetch all by ID and filter.
            var candidates = await _dbSet
                .Where(i => i.LocationId == locationId && idList.Contains(i.IdentityId))
                .ToListAsync();

            return candidates.Where(i => identities.Any(id => id.id == i.IdentityId && id.isAccessory == i.IsAccessory));
        }

        public async Task<IEnumerable<Inventory>> GetByProductIdAsync(Guid productId)
            => await _dbSet.Include(i => i.Location)
                           .Include(i => i.Product)
                                .ThenInclude(p => p != null ? p.Media : null)
                           .Where(i => i.ProductId == productId)
                           .ToListAsync();
                           
        public async Task<IEnumerable<Inventory>> GetByIdentityIdAsync(Guid identityId, bool isAccessory)
            => await _dbSet.Include(i => i.Location)
                           .Where(i => i.IdentityId == identityId && i.IsAccessory == isAccessory)
                           .ToListAsync();

        public async Task<int> GetTotalAvailableAsync(Guid identityId, bool isAccessory)
        {
            return await _dbSet
                .Where(i => i.IdentityId == identityId && i.IsAccessory == isAccessory)
                .SumAsync(i => i.OnHand - i.Reserved);
        }

        public async Task<Dictionary<Guid, int>> GetTotalAvailableBatchAsync(IEnumerable<(Guid id, bool isAccessory)> identities)
        {
            var idList = identities.Select(x => x.id).ToList();
            var stocks = await _dbSet
                .Where(i => idList.Contains(i.IdentityId))
                .ToListAsync();

            return stocks
                .Where(i => identities.Any(id => id.id == i.IdentityId && id.isAccessory == i.IsAccessory))
                .GroupBy(i => i.IdentityId)
                .ToDictionary(g => g.Key, g => g.Sum(i => i.OnHand - i.Reserved));
        }

        public async Task<Dictionary<Guid, (int OnHand, int Reserved)>> GetStockSummaryBatchAsync(IEnumerable<Guid> accessoryIds)
        {
            var ids = accessoryIds.ToList();
            var stock = await _dbSet
                .AsNoTracking()
                .Where(i => i.IsAccessory && ids.Contains(i.IdentityId))
                .GroupBy(i => i.IdentityId)
                .Select(g => new
                {
                    AccessoryId = g.Key,
                    OnHand = g.Sum(x => x.OnHand),
                    Reserved = g.Sum(x => x.Reserved)
                })
                .ToListAsync();

            return stock.ToDictionary(x => x.AccessoryId, x => (x.OnHand, x.Reserved));
        }


        public async Task<int> AtomicReserveAsync(Guid identityId, bool isAccessory, int quantity, Guid locationId)
        {
            var query = BuildAtomicQuery(identityId, isAccessory, locationId);

            return await query
                .Where(i => (i.OnHand - i.Reserved) >= quantity)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(i => i.Reserved, i => i.Reserved + quantity)
                    .SetProperty(i => i.UpdatedAt, i => DateTime.UtcNow));
        }

        public async Task<int> AtomicReleaseAsync(Guid identityId, bool isAccessory, int quantity, Guid locationId)
        {
            var query = BuildAtomicQuery(identityId, isAccessory, locationId);

            return await query
                .ExecuteUpdateAsync(s => s
                    .SetProperty(i => i.Reserved, i => Math.Max(0, i.Reserved - quantity))
                    .SetProperty(i => i.UpdatedAt, i => DateTime.UtcNow));
        }

        public async Task<int> AtomicDeductAsync(Guid identityId, bool isAccessory, int quantity, Guid locationId)
        {
            var query = BuildAtomicQuery(identityId, isAccessory, locationId);

            return await query
                .ExecuteUpdateAsync(s => s
                    .SetProperty(i => i.Reserved, i => Math.Max(0, i.Reserved - quantity))
                    .SetProperty(i => i.OnHand, i => Math.Max(0, i.OnHand - quantity))
                    .SetProperty(i => i.UpdatedAt, i => DateTime.UtcNow));
        }

        public async Task<int> AtomicAdjustOnHandAsync(Guid identityId, bool isAccessory, int delta, Guid locationId)
        {
            var query = BuildAtomicQuery(identityId, isAccessory, locationId);

            if (delta < 0)
            {
                query = query.Where(i => i.OnHand >= Math.Abs(delta));
            }

            return await query.ExecuteUpdateAsync(s => s
                .SetProperty(i => i.OnHand, i => i.OnHand + delta)
                .SetProperty(i => i.UpdatedAt, i => DateTime.UtcNow));
        }

        private IQueryable<Inventory> BuildAtomicQuery(Guid identityId, bool isAccessory, Guid locationId)
        {
            return _dbSet.Where(i => i.LocationId == locationId && i.IdentityId == identityId && i.IsAccessory == isAccessory);
        }
    }
}
