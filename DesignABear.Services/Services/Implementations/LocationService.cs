using AutoMapper;
using DesignABear.Repositories.Models;
using DesignABear.Repositories.UnitOfWork;
using DesignABear.Services.DTOs.Requests;
using DesignABear.Services.DTOs.Responses;
using DesignABear.Services.Exceptions;
using DesignABear.Services.Services.Interfaces;

namespace DesignABear.Services.Services.Implementations
{
    public class LocationService : ILocationService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public LocationService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<IEnumerable<LocationResponse>> GetAllAsync()
        {
            var locations = await _uow.Locations.GetAllAsync();
            return _mapper.Map<IEnumerable<LocationResponse>>(locations);
        }

        public async Task<LocationResponse?> GetByIdAsync(Guid locationId)
        {
            var location = await _uow.Locations.GetByIdAsync(locationId);
            if (location == null) throw new NotFoundException("Location", locationId);
            return _mapper.Map<LocationResponse>(location);
        }

        public async Task<LocationResponse> CreateAsync(CreateLocationRequest request)
        {
            var location = _mapper.Map<Location>(request);
            location.LocationId = Guid.NewGuid();
            
            await _uow.Locations.AddAsync(location);
            await _uow.SaveChangesAsync();
            
            return _mapper.Map<LocationResponse>(location);
        }

        public async Task UpdateAsync(Guid locationId, UpdateLocationRequest request)
        {
            var location = await _uow.Locations.GetByIdAsync(locationId);
            if (location == null) throw new NotFoundException("Location", locationId);
            
            _mapper.Map(request, location);
            _uow.Locations.Update(location);
            await _uow.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid locationId)
        {
            var location = await _uow.Locations.GetByIdAsync(locationId);
            if (location is not null)
            {
                _uow.Locations.Remove(location);
                await _uow.SaveChangesAsync();
            }
        }
    }
}
