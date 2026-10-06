using AutoMapper;
using DesignABear.Repositories.Models;
using DesignABear.Repositories.UnitOfWork;
using DesignABear.Services.DTOs.Requests;
using DesignABear.Services.DTOs.Responses;
using DesignABear.Services.Exceptions;
using DesignABear.Services.Services.Interfaces;

using Microsoft.Extensions.Logging;

namespace DesignABear.Services.Services.Implementations
{
    public class AddressService : IAddressService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<AddressService> _logger;

        public AddressService(IUnitOfWork uow, IMapper mapper, ILogger<AddressService> logger)
        {
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<AddressResponse?> GetByIdAsync(Guid addressId)
        {
            var address = await _uow.Addresses.GetByIdAsync(addressId);
            if (address == null) throw new NotFoundException("Address", addressId);
            return _mapper.Map<AddressResponse>(address);
        }

        public async Task<IEnumerable<AddressResponse>> GetByUserIdAsync(Guid userId)
        {
            var addresses = await _uow.Addresses.GetByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<AddressResponse>>(addresses);
        }

        public async Task<AddressResponse> CreateAsync(CreateAddressRequest request)
        {
            // Clean data
            request.Line1 = request.Line1?.Trim() ?? "";
            request.Line2 = request.Line2?.Trim();
            request.City = request.City?.Trim();
            request.State = request.State?.Trim();
            request.FullName = request.FullName?.Trim() ?? "";
            request.PhoneNumber = request.PhoneNumber?.Trim() ?? "";
            request.PostalCode = request.PostalCode?.Trim();
            request.Country = request.Country?.Trim();

            var allAddresses = (await _uow.Addresses.GetByUserIdAsync(request.UserId)).ToList();
            
            // Improve Duplicate Check: Include Line2 and PhoneNumber
            var existingAddress = allAddresses.FirstOrDefault(a =>
                string.Equals(a.Line1?.Trim(), request.Line1, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(a.Line2?.Trim(), request.Line2, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(a.City?.Trim(), request.City, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(a.State?.Trim(), request.State, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(a.FullName?.Trim(), request.FullName, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(a.PhoneNumber?.Trim(), request.PhoneNumber, StringComparison.OrdinalIgnoreCase));

            if (existingAddress != null)
            {
                _logger.LogInformation("Address already exists. Reusing AddressId: {AddressId}", existingAddress.AddressId);
                return _mapper.Map<AddressResponse>(existingAddress);
            }

            // Strict Default Rule: Reset existing defaults if the new one is marked as default
            if (request.IsDefaultShipping || request.IsDefaultBilling)
            {
                foreach (var addr in allAddresses)
                {
                    bool changed = false;
                    if (request.IsDefaultShipping && addr.IsDefaultShipping)
                    {
                        addr.IsDefaultShipping = false;
                        changed = true;
                    }
                    if (request.IsDefaultBilling && addr.IsDefaultBilling)
                    {
                        addr.IsDefaultBilling = false;
                        changed = true;
                    }
                    if (changed) _uow.Addresses.Update(addr);
                }
            }

            var address = _mapper.Map<Address>(request);
            address.AddressId = Guid.NewGuid();
            address.CreatedAt = DateTime.UtcNow;

            // Ensure trimmed values are assigned to the model
            address.Line1 = request.Line1;
            address.Line2 = request.Line2;
            address.City = request.City;
            address.State = request.State;
            address.FullName = request.FullName;
            address.PhoneNumber = request.PhoneNumber;
            address.PostalCode = request.PostalCode;
            address.Country = request.Country;
            address.Email = request.Email?.Trim();

            await _uow.Addresses.AddAsync(address);
            await _uow.SaveChangesAsync();

            _logger.LogInformation("Created new address {AddressId} for user {UserId}", address.AddressId, address.UserId);
            return _mapper.Map<AddressResponse>(address);
        }

        public async Task UpdateAsync(Guid addressId, UpdateAddressRequest request)
        {
            var address = await _uow.Addresses.GetByIdAsync(addressId);
            if (address == null) throw new NotFoundException("Address", addressId);

            _mapper.Map(request, address);
            
            address.FullName = request.FullName;
            address.PhoneNumber = request.PhoneNumber;
            address.Email = request.Email;

            _uow.Addresses.Update(address);
            await _uow.SaveChangesAsync();

            _logger.LogInformation("Updated address {AddressId} for user {UserId}", addressId, address.UserId);
        }

        public async Task DeleteAsync(Guid addressId)
        {
            var address = await _uow.Addresses.GetByIdAsync(addressId);
            if (address is not null)
            {
                _uow.Addresses.Remove(address);
                await _uow.SaveChangesAsync();
            }
        }

        public async Task SetDefaultShippingAsync(Guid userId, Guid addressId)
        {
            var all = await _uow.Addresses.GetByUserIdAsync(userId);
            foreach (var addr in all)
            {
                addr.IsDefaultShipping = addr.AddressId == addressId;
                _uow.Addresses.Update(addr);
            }
            await _uow.SaveChangesAsync();
        }

        public async Task SetDefaultBillingAsync(Guid userId, Guid addressId)
        {
            var all = await _uow.Addresses.GetByUserIdAsync(userId);
            foreach (var addr in all)
            {
                addr.IsDefaultBilling = addr.AddressId == addressId;
                _uow.Addresses.Update(addr);
            }
            await _uow.SaveChangesAsync();
        }
    }
}
