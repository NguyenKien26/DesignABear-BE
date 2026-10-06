using DesignABear.Services.DTOs.Requests;
using DesignABear.Services.DTOs.Responses;
using DesignABear.Services.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DesignABear.Services.Services.Interfaces
{
    public interface IAccessoryService
    {
        Task<Result<IEnumerable<AccessoryResponse>>> GetAllAsync();
        Task<Result<AccessoryResponse>> GetByIdAsync(Guid id);
        Task<Result<IEnumerable<AccessoryResponse>>> GetByProductIdAsync(Guid productId);
        Task<Result<AccessoryResponse>> CreateAsync(CreateAccessoryRequest request);
        Task<Result<AccessoryResponse>> UpdateAsync(Guid id, UpdateAccessoryRequest request);
        Task<Result<bool>> DeleteAsync(Guid id);
        
        // Matrix management
        Task<Result<bool>> AddToProductAsync(Guid productId, Guid accessoryId);
        Task<Result<bool>> RemoveFromProductAsync(Guid productId, Guid accessoryId);

        // Internal helper for Smart Chip
        Task<Result<AccessoryResponse>> GetSmartChipAsync();
    }
}
