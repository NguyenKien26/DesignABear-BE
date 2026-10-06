using AutoMapper;
using DesignABear.Repositories.Models;
using DesignABear.Repositories.UnitOfWork;
using DesignABear.Services.DTOs.Requests;
using DesignABear.Services.DTOs.Responses;
using DesignABear.Services.Exceptions;
using DesignABear.Services.Services.Interfaces;

namespace DesignABear.Services.Services.Implementations
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public RoleService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<RoleResponse?> GetByIdAsync(int roleId)
        {
            var role = await _uow.Roles.GetByIdAsync(roleId);
            if (role == null) throw new NotFoundException("Role", roleId);
            return _mapper.Map<RoleResponse>(role);
        }

        public async Task<IEnumerable<RoleResponse>> GetAllAsync()
        {
            var roles = await _uow.Roles.GetAllAsync();
            return _mapper.Map<IEnumerable<RoleResponse>>(roles);
        }

        public async Task<RoleResponse> CreateAsync(CreateRoleRequest request)
        {
            var existing = await _uow.Roles.FindAsync(r => r.RoleName == request.RoleName);
            if (existing.Any()) throw new BadRequestException("Role name already exists.");

            var role = _mapper.Map<Role>(request);
            await _uow.Roles.AddAsync(role);
            await _uow.SaveChangesAsync();

            return _mapper.Map<RoleResponse>(role);
        }

        public async Task UpdateAsync(int roleId, UpdateRoleRequest request)
        {
            var role = await _uow.Roles.GetByIdAsync(roleId);
            if (role == null) throw new NotFoundException("Role", roleId);

            var existing = await _uow.Roles.FindAsync(r => r.RoleName == request.RoleName && r.RoleId != roleId);
            if (existing.Any()) throw new BadRequestException("Role name already exists.");

            _mapper.Map(request, role);
            _uow.Roles.Update(role);
            await _uow.SaveChangesAsync();
        }

        public async Task DeleteAsync(int roleId)
        {
            var role = await _uow.Roles.GetByIdAsync(roleId);
            if (role != null)
            {
                _uow.Roles.Remove(role);
                await _uow.SaveChangesAsync();
            }
        }
    }
}
