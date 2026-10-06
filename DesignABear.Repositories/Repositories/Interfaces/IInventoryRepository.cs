using DesignABear.Repositories.Models;

namespace DesignABear.Repositories.Repositories.Interfaces
{
    public interface IInventoryRepository : IGenericRepository<Inventory>
    {
        Task<Inventory?> GetByLocationAndIdentityAsync(Guid locationId, Guid identityId, bool isAccessory);
        Task<IEnumerable<Inventory>> GetByLocationAndIdentitiesAsync(Guid locationId, IEnumerable<(Guid id, bool isAccessory)> identities);
        Task<IEnumerable<Inventory>> GetByProductIdAsync(Guid productId);
        Task<IEnumerable<Inventory>> GetByIdentityIdAsync(Guid identityId, bool isAccessory);
        Task<int> GetTotalAvailableAsync(Guid identityId, bool isAccessory);
        Task<Dictionary<Guid, int>> GetTotalAvailableBatchAsync(IEnumerable<(Guid id, bool isAccessory)> identities);

        // Atomic Operations
        Task<int> AtomicReserveAsync(Guid identityId, bool isAccessory, int quantity, Guid locationId);
        Task<int> AtomicReleaseAsync(Guid identityId, bool isAccessory, int quantity, Guid locationId);
        Task<int> AtomicDeductAsync(Guid identityId, bool isAccessory, int quantity, Guid locationId);
        Task<int> AtomicAdjustOnHandAsync(Guid identityId, bool isAccessory, int delta, Guid locationId);
        Task<Dictionary<Guid, (int OnHand, int Reserved)>> GetStockSummaryBatchAsync(IEnumerable<Guid> accessoryIds);
    }
}
