using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinqKit;
using MiComunidadPro.Business.Engines.Contracts;
using MiComunidadPro.Business.Entities.DBO;
using MiComunidadPro.Business.Entities.Dtos;
using MiComunidadPro.Business.Entities.IDENTITY;
using MiComunidadPro.Business.Entities.LOCATION;
using MiComunidadPro.Common.Contracts;
using MiComunidadPro.Common.Dtos;
using Microsoft.AspNetCore.Identity;

namespace MiComunidadPro.Business.Engines
{
    public class UserEngine : IUserEngine
    {
        private readonly ICacheService _CacheService;
        private readonly IDataRepositoryFactory _DataRepositoryFactory;
        private readonly UserManager<User> _UserManager;  
        private readonly RoleManager<Role> _RoleManager;  
        public UserEngine(IDataRepositoryFactory dataRepositoryFactory, ICacheService cacheService, UserManager<User> userManager)
        {
            _CacheService = cacheService;
            _DataRepositoryFactory = dataRepositoryFactory;
            _UserManager = userManager;
        }

        public async ValueTask<IEnumerable<AuthLocationDto>> GetLocationsAsync(int userId)
        {
            var result = await _CacheService.GetCachedAsync<IEnumerable<AuthLocationDto>>($"user_${userId}_locations", async () => {
                var repository = _DataRepositoryFactory.Get<UserLocation>();
                var locations = await repository.GetAllAsync(x => x.Select(ul => new AuthLocationDto {
                    Id = ul.LocationId,
                    Name = ul.Location.Name,
                    LocationStatus = ul.Location.Status,
                    UserLocationStatus = ul.Status
                }), x => x.UserId == userId);
                return locations;
            });

            return result;
        }
        public async ValueTask<IEnumerable<string>> GetAllPermissionsAsync(int userId, int roleId)
        {
            var result = await _CacheService.GetCachedAsync<IEnumerable<string>>($"user_${userId}_${roleId}_permissions", async () => {
                var permissionRepository = _DataRepositoryFactory.Get<Permission>();
                var roleRepository = _DataRepositoryFactory.Get<Role>();
                int? roleLocationId = await roleRepository.GetAsync(x => x.Select(r => r.LocationId), x => x.Id == roleId);

                var permissions = await permissionRepository.GetAllAsync(x => x.Select(p => p.Name),
                                     x => (x.UserPermissions.Any(up => up.UserId == userId && up.LocationId == roleLocationId) || 
                                            x.RolePermissions.Any(rp => rp.RoleId == roleId)) && x.IsForLocation == roleLocationId.HasValue);
               return permissions;
            });

            return result;
        }

        public async ValueTask<KeyValueDto> GetRoleAsync(int userId, int? locationId = null) 
        {
            var repository = _DataRepositoryFactory.Get<Role>();

            return await repository.GetAsync(x => x.Select(r => new KeyValueDto {
                Id = r.Id,
                Name = r.Name
            }), x => x.UserRoles.Any(ur => ur.UserId == userId) && x.LocationId == locationId);
        }

        public async ValueTask<IPagedList<UserSearchResultDto>> SearchAsync(LocationSearchPayloadDto payload, int pageIndex = 1, int pageSize = 25)
        {
            var repository = _DataRepositoryFactory.Get<User>();

            var predicate = PredicateBuilder.New<User>(x => !x.Deleted);

            //Filters
            if (!string.IsNullOrEmpty(payload.Name))
                predicate = predicate.And(x => ($"{x.Name} {x.LastName}" ).Contains(payload.Name) || x.UserName.Contains(payload.Name));

            var result = await repository.GetPagedAsync(location => location.OrderBy(x => x.Name).Select(x => new UserSearchResultDto
            {
                Id = x.Id,
                LastName = x.LastName,
                Name = x.Name,
                UserName = x.UserName,
                lastLogin = x.LastLogin,
                Status = x.Status
            }), predicate, pageIndex, pageSize);

            return result;
        }

        public async ValueTask AddToRoleAsync(User user, int roleId) 
        {
            var roleRepository = _DataRepositoryFactory.Get<Role>();
            var userRoleRepository = _DataRepositoryFactory.Get<UserRole>();

            var role = await roleRepository.GetAsync(x => x, x => x.Id == roleId && !x.Deleted);

            var currentRole = await userRoleRepository.GetAsync(x => x.Select(ur => new {
                Id = ur.RoleId,
                Name = ur.Role.Name
            }), x => x.UserId == user.Id && x.Role.LocationId == role.LocationId);

            if (currentRole != null && currentRole.Id == roleId) return;

            if (currentRole != null)
                await _UserManager.RemoveFromRoleAsync(user, currentRole.Name);

            await _UserManager.AddToRoleAsync(user, role.Name);
        }

        public async ValueTask AddToRoleAsync(int id, int roleId) 
        {
            var userRepository = _DataRepositoryFactory.Get<User>();
            var user = await userRepository.GetAsync(x => x, x => x.Id == id && !x.Deleted);

            await AddToRoleAsync(user, roleId);
        }

        public async ValueTask<int> CreateAsync(UserCreatePayloadDto payload)
        {
            List<Phone> phones = null;

            if (payload.Phones != null) {
                phones = payload.Phones.Select(x => new Phone {
                    Number = x.Number,
                    Extension = x.Extension,
                    IsWhatsAppeable = x.IsWhatsAppeable,
                    Deleted = false
                }).ToList();
            }
            var user = new User {
                LastName = payload.LastName,
                Name = payload.Name,
                Email = payload.Email,
                UserName = payload.UserName,
                Deleted = false,
                Phones = phones
            };

            IdentityResult result = await _UserManager.CreateAsync(user, payload.Password);

            if (!result.Succeeded)
                throw new ArgumentException(result.Errors.ToString());

            await AddToRoleAsync(user, payload.RoleId);

            return user.Id;
        }
    }
}