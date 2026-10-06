using DesignABear.Services.DTOs.Requests;
using DesignABear.Services.DTOs.Responses;

namespace DesignABear.Services.Services.Interfaces
{
    public interface IAddressService
    {
        Task<AddressResponse?> GetByIdAsync(Guid addressId);
        Task<IEnumerable<AddressResponse>> GetByUserIdAsync(Guid userId);
        Task<AddressResponse> CreateAsync(CreateAddressRequest request);
        Task UpdateAsync(Guid addressId, UpdateAddressRequest request);
        Task DeleteAsync(Guid addressId);
        Task SetDefaultShippingAsync(Guid userId, Guid addressId);
        Task SetDefaultBillingAsync(Guid userId, Guid addressId);
    }
}
