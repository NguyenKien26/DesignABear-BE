using DesignABear.Services.DTOs.Requests;
using DesignABear.Services.DTOs.Responses;

namespace DesignABear.Services.Services.Interfaces
{
    public interface IInventoryService
    {
        Task<InventoryResponse?> GetAsync(Guid locationId, Guid identityId, bool isAccessory);
        Task<IEnumerable<InventoryResponse>> GetByProductIdAsync(Guid productId);
        Task<IEnumerable<InventoryResponse>> GetByAccessoryIdAsync(Guid accessoryId);
        Task<int> GetTotalAvailableAsync(Guid identityId, bool isAccessory);
        
        Task AdjustStockAsync(Guid identityId, bool isAccessory, int delta, Guid locationId);
        Task AdjustStockByIdAsync(Guid identityId, bool isAccessory, int delta, Guid? locationId = null);

        Task ReserveStockAsync(Guid identityId, bool isAccessory, int quantity, Guid locationId);
        Task ReserveStockBatchAsync(Guid locationId, IEnumerable<StockReservationItem> items);
        Task ReleaseReservationAsync(Guid identityId, bool isAccessory, int quantity, Guid locationId);
        Task ConfirmDeductionAsync(Guid identityId, bool isAccessory, int quantity, Guid locationId);
    }
}
