using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinqKit;
using MiComunidadPro.Business.Contracts;
using MiComunidadPro.Business.Engines.Contracts;
using MiComunidadPro.Business.Entities.DBO;
using MiComunidadPro.Business.Entities.Dtos;
using MiComunidadPro.Business.Entities.IDENTITY;
using MiComunidadPro.Common.Contracts;
using MiComunidadPro.Common.Dtos;
using Microsoft.AspNetCore.Identity;

namespace MiComunidadPro.Business.Engines
{
    public class RoleEngine : IRoleEngine
    {
        private readonly IDataRepositoryFactory _DataRepositoryFactory;
        private readonly RoleManager<Role> _RoleManager;  
        private readonly IMessageHandler _MessageHandler;
        public RoleEngine(IDataRepositoryFactory dataRepositoryFactory, RoleManager<Role> roleManager, IMessageHandler messageHandler) 
        {
            _DataRepositoryFactory = dataRepositoryFactory;
            _RoleManager = roleManager;
            _MessageHandler = messageHandler;
        }

        public async ValueTask<IPagedList<RoleSearchResultDto>> SearchAsync(RoleSearchPayloadDto payload, int pageIndex = 1, int pageSize = 25)
        {
            var repository = _DataRepositoryFactory.Get<Role>();

            var predicate = PredicateBuilder.New<Role>(x => !x.Deleted);

            //Filters
            if (payload.LocationId.HasValue)
                predicate = predicate.And(x => x.LocationId == payload.LocationId.Value);

            if (!string.IsNullOrEmpty(payload.Name))
                predicate = predicate.And(x => x.Name.Contains(payload.Name) || x.Location.Name.Contains(payload.Name));

            var result = await repository.GetPagedAsync(location => location.OrderBy(x => x.Name).Select(x => new RoleSearchResultDto
            {
                Id = x.Id,
                Name = x.Name,
                DisplayName = x.DisplayName,
                LocationName = x.Location.Name
            }), predicate, pageIndex, pageSize);

            return result;
        }

        public async ValueTask<int> CreateAsync(RoleCreateEditPayloadDto payload)
        {
            var role = new Role {
                Name = payload.Name,
                DisplayName = payload.DisplayName,
                LocationId = payload.LocationId,
                Deleted = false
            };

            IdentityResult result = await _RoleManager.CreateAsync(role);

            if (!result.Succeeded)
                throw new ArgumentException(result.Errors.ToString());

            return role.Id;
        }

        public async ValueTask<RoleDto> GetAsync(int id)
        {
            var repository = _DataRepositoryFactory.Get<Role>();

            var result = await repository.GetAsync(x => x.Select(role => new RoleDto
            {
                Id = role.Id,
                LocationId = role.LocationId,
                LocationName = role.Location.Name,
                Name = role.Name,
                DisplayName = role.DisplayName
            }), x => !x.Deleted && x.Id == id);

            return result;
        }

        public async ValueTask UpdateAsync(RoleCreateEditPayloadDto payload)
        {
            var repository = _DataRepositoryFactory.Get<Role>();
            var role = await _RoleManager.FindByIdAsync(payload.Id.ToString());
            
            role.Name = payload.Name;
            role.DisplayName = payload.DisplayName;
            role.LocationId = payload.LocationId;
            
            IdentityResult result = await _RoleManager.UpdateAsync(role);

            if (!result.Succeeded)
                throw new ArgumentException(result.Errors.ToString());

        }

        public async ValueTask<IEnumerable<KeyValueDto>> GetPermissionsAsync(int id) 
        {
            var roleRepository = _DataRepositoryFactory.Get<Role>();
            var permissionRepository = _DataRepositoryFactory.Get<Permission>();

            var locationId = await roleRepository.GetAsync(x => x.Select(r => r.LocationId), x => x.Id == id);

            var result = await permissionRepository.GetAllAsync(x => x.Select(p => new KeyValueDto {
                Id = p.Id,
                Name = p.Note,
                Selected = p.RolePermissions.Any(rp => rp.RoleId == id),
                Disabled = locationId.HasValue ? !p.LocationPermissions.Any(lp => lp.LocationId == locationId.Value) : false
            }).OrderBy(x => x.Name), x => x.IsForLocation == locationId.HasValue);

            return result;
        }

        public async ValueTask AddPermissionsAsync(int id, List<KeyValueDto<int>> payload)
        {
            var rolePermissionRepository = _DataRepositoryFactory.Get<RolePermission>();
            var userPermissionRepository = _DataRepositoryFactory.Get<UserPermission>();
            var userRepository = _DataRepositoryFactory.Get<User>();

            var permissions = payload.Select(x => x.Id);

            var currentPermissions = await rolePermissionRepository.GetAllAsync(x => x, x => x.RoleId == id);

            var toRemovePermissions = currentPermissions.Where(x => !permissions.Contains(x.PermissionId));
            
            var toAddPermissions = permissions.Where(x => !currentPermissions.Any(c => c.PermissionId == x));

            await rolePermissionRepository.RemoveAllAsync(toRemovePermissions);

            foreach (var permissionId in toAddPermissions)
            {
                await rolePermissionRepository.AddAsync(new RolePermission {
                    RoleId = id,
                    PermissionId = permissionId
                });
            }

            var userIds = await userRepository.GetAllAsync(x => x.Select(u => u.Id), x => x.Roles.Any(ur => ur.RoleId == id));

            var rolePermissions = await rolePermissionRepository.GetAllAsync(x => x.Select(rp => rp.PermissionId), x => x.RoleId == id);

            var userPermissions = await userPermissionRepository.GetAllAsync(x => x, x => userIds.Contains(x.UserId) && rolePermissions.Contains(x.PermissionId));

            await userPermissionRepository.RemoveAllAsync(userPermissions);
        }
    }
}