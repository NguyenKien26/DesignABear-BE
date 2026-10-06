using AutoMapper;
using DesignABear.Repositories.Models;
using DesignABear.Repositories.UnitOfWork;
using DesignABear.Services.DTOs.Requests;
using DesignABear.Services.DTOs.Responses;
using DesignABear.Services.Exceptions;
using DesignABear.Services.Services.Interfaces;

namespace DesignABear.Services.Services.Implementations
{
    public class PersonalizationGroupService : IPersonalizationGroupService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public PersonalizationGroupService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PersonalizationGroupResponse>> GetAllAsync()
        {
            var groups = await _uow.PersonalizationGroups.GetAllAsync();
            return _mapper.Map<IEnumerable<PersonalizationGroupResponse>>(groups);
        }

        public async Task<PersonalizationGroupResponse?> GetByIdAsync(Guid groupId)
        {
            var group = await _uow.PersonalizationGroups.GetByIdAsync(groupId);
            if (group == null) throw new NotFoundException("PersonalizationGroup", groupId);
            return _mapper.Map<PersonalizationGroupResponse>(group);
        }

        public async Task<PersonalizationGroupResponse> CreateAsync(CreatePersonalizationGroupRequest request)
        {
            var group = _mapper.Map<PersonalizationGroup>(request);
            group.GroupId = Guid.NewGuid();
            
            await _uow.PersonalizationGroups.AddAsync(group);
            await _uow.SaveChangesAsync();
            
            return _mapper.Map<PersonalizationGroupResponse>(group);
        }

        public async Task UpdateAsync(Guid groupId, UpdatePersonalizationGroupRequest request)
        {
            var group = await _uow.PersonalizationGroups.GetByIdAsync(groupId);
            if (group == null) throw new NotFoundException("PersonalizationGroup", groupId);
            
            _mapper.Map(request, group);
            _uow.PersonalizationGroups.Update(group);
            await _uow.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid groupId)
        {
            var group = await _uow.PersonalizationGroups.GetByIdAsync(groupId);
            if (group is not null)
            {
                _uow.PersonalizationGroups.Remove(group);
                await _uow.SaveChangesAsync();
            }
        }
    }
}
