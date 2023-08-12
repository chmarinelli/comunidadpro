using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinqKit;
using MiComunidadPro.Business.Engines.Contracts;
using MiComunidadPro.Business.Entities.DBO;
using MiComunidadPro.Business.Entities.Dtos;
using MiComunidadPro.Business.Entities.Enums;
using MiComunidadPro.Business.Entities.Enums.Status;
using MiComunidadPro.Business.Entities.IDENTITY;
using MiComunidadPro.Business.Entities.LOCATION;
using MiComunidadPro.Common.Contracts;
using MiComunidadPro.Common.Dtos;

namespace MiComunidadPro.Business.Engines
{
    public class LocationEngine : ILocationEngine
    {
        private readonly IDataRepositoryFactory _DataRepositoryFactory;
        public LocationEngine(IDataRepositoryFactory dataRepositoryFactory)
        {
            _DataRepositoryFactory = dataRepositoryFactory;
        }

        public async ValueTask<IPagedList<LocationSearchResultDto>> SearchAsync(LocationSearchPayloadDto payload, int pageIndex = 1, int pageSize = 25)
        {
            var repository = _DataRepositoryFactory.Get<Location>();

            var predicate = PredicateBuilder.New<Location>(x => !x.Deleted);

            //Filters
            if (!string.IsNullOrEmpty(payload.Name))
                predicate = predicate.And(x => x.Name.Contains(payload.Name) || x.Company.Name.Contains(payload.Name));

            var result = await repository.GetPagedAsync(location => location.OrderBy(x => x.Name).Select(x => new LocationSearchResultDto
            {
                Id = x.Id,
                CompanyName = x.Company.Name,
                Name = x.Name,
                Address = x.Address,
                Status = x.Status
            }), predicate, pageIndex, pageSize);

            return result;
        }

        public async ValueTask<int> CreateAsync(LocationCreateEditPayloadDto payload)
        {
            var locationRepository = _DataRepositoryFactory.Get<Location>();
            var chargeRepository = _DataRepositoryFactory.Get<Charge>();
            var locationConfigurationRepository = _DataRepositoryFactory.Get<LocationConfiguration>();

            var result = await locationRepository.AddAsync(new Location
            {
                CompanyId = payload.CompanyId,
                Name = payload.Name,
                Note = payload.Note,
                Address = payload.Address,
                Status = LocationStatus.Active
            });

            await locationConfigurationRepository.AddAsync(new LocationConfiguration
            {
                LocationId = result.Id,
                Deleted = false
            });

            await chargeRepository.AddAsync(new Charge
            {
                LocationId = result.Id,
                OrderType = POFOrderType.UNIT_PAYMENT,
                Name = "Mora",
                Note = "Este cargo solo se aplica para los pagos de unidad tardios",
                Amount = null,
                IsReadOnly = false,
                IsActive = true
            });

            return result.Id;
        }

        public async ValueTask<LocationDto> GetAsync(int id)
        {
            var repository = _DataRepositoryFactory.Get<Location>();

            var result = await repository.GetAsync(x => x.Select(location => new LocationDto
            {
                Id = location.Id,
                CompanyId = location.CompanyId,
                CompanyName = location.Company.Name,
                Name = location.Name,
                Address = location.Address,
                Note = location.Note,
                Status = location.Status
            }), x => !x.Deleted && x.Id == id);

            return result;
        }
        public async ValueTask ActivateAsync(int id)
        {
            var repository = _DataRepositoryFactory.Get<Location>();

            var location = await repository.GetAsync(x => x, x => x.Id == id);
            location.Status = location.Status == LocationStatus.Active ? LocationStatus.Locked : LocationStatus.Active;
            await repository.UpdateAsync(location);
        }
        public async ValueTask UpdateAsync(LocationCreateEditPayloadDto payload)
        {
            var repository = _DataRepositoryFactory.Get<Location>();

            var location = await repository.GetAsync(x => x, x => x.Id == payload.Id);

            location.CompanyId = payload.CompanyId;
            location.Name = payload.Name;
            location.Note = payload.Note;
            location.Address = payload.Address;

            await repository.UpdateAsync(location);
        }

        public async ValueTask<MasterLocationConfigurationDto> GetMasterConfigurationsAsync(int locationId)
        {
            var repository = _DataRepositoryFactory.Get<LocationConfiguration>();

            var configuration = await repository.GetAsync(x => x.Select(lc => new MasterLocationConfigurationDto
            {
                LocationId = lc.LocationId,
                MaxUnits = lc.MaxUnits,
                MaxUsers = lc.MaxUsers,
                MaxPOF = lc.MaxPOF,
                MaxUnitUsers = lc.MaxUnitUsers,
                PaymentGenerationDay = lc.PaymentGenerationDay,
                PaymentInvoiceGenerationDay = lc.PaymentInvoiceGenerationDay,
                MaintenancePaymentDay = lc.MaintenancePaymentDay,
                MaintenanceLatePayment = lc.MaintenanceLatePayment,
                MaintenancePaymentDelayGenerationDay = lc.MaintenancePaymentDelayGenerationDay
            }), x => x.LocationId == locationId && !x.Deleted);

            return configuration;
        }

        public async ValueTask UpdateMasterConfigurationAsync(MasterLocationConfigurationEditPayloadDto payload)
        {
            var repository = _DataRepositoryFactory.Get<LocationConfiguration>();

            var configuration = await repository.GetAsync(x => x, x => x.LocationId == payload.LocationId);

            configuration.MaxUnits = payload.MaxUnits;
            configuration.MaxUsers = payload.MaxUsers;
            configuration.MaxPOF = payload.MaxPOF;
            configuration.MaxUnitUsers = payload.MaxUnitUsers;
            configuration.PaymentGenerationDay = payload.PaymentGenerationDay;
            configuration.PaymentInvoiceGenerationDay = payload.PaymentInvoiceGenerationDay;
            configuration.MaintenancePaymentDay = payload.MaintenancePaymentDay;
            configuration.MaintenanceLatePayment = payload.MaintenanceLatePayment;
            configuration.MaintenancePaymentDelayGenerationDay = payload.MaintenancePaymentDelayGenerationDay;

            await repository.UpdateAsync(configuration);
        }

        public async ValueTask<IEnumerable<KeyValueDto>> GetPermissionsAsync(int locationId) 
        {
            var repository = _DataRepositoryFactory.Get<Permission>();

            var result = await repository.GetAllAsync(x => x.Select(p => new KeyValueDto {
                Id = p.Id,
                Name = p.Note,
                Selected = p.LocationPermissions.Any(pl => pl.LocationId == locationId)
            }).OrderBy(x => x.Name), x => x.IsForLocation);

            return result;
        }

        public async ValueTask AddPermissionsAsync(int locationId, List<KeyValueDto<int>> payload)
        {
            var locationPermissionRepository = _DataRepositoryFactory.Get<LocationPermission>();
            var rolePermissionRepository = _DataRepositoryFactory.Get<RolePermission>();
            var userPermissionRepository = _DataRepositoryFactory.Get<UserPermission>();
            var roleRepository = _DataRepositoryFactory.Get<Role>();

            var permissions = payload.Select(x => x.Id);
            var currentPermissions = await locationPermissionRepository.GetAllAsync(x => x, x => x.LocationId == locationId);
            var toRemovePermissions = currentPermissions.Where(x => !permissions.Contains(x.PermissionId));
            var toAddPermissions = permissions.Where(x => !currentPermissions.Any(c => c.PermissionId == x));

            await locationPermissionRepository.RemoveAllAsync(toRemovePermissions);
            
            foreach (var permissionId in toAddPermissions)
            {
                await locationPermissionRepository.AddAsync(new LocationPermission {
                    LocationId = locationId,
                    PermissionId = permissionId
                });
            }

            var locationPermissions = await locationPermissionRepository.GetAllAsync(x => x.Select(lp => lp.PermissionId), x => x.LocationId == locationId);

            var roleIds = await roleRepository.GetAllAsync(x => x.Select(r => r.Id), x => x.LocationId == locationId);

            foreach (var roleId in roleIds)
            {
                var rolePermissions = await rolePermissionRepository.GetAllAsync(x => x, x => x.RoleId == roleId);
                var toRemoveRolePermissions = rolePermissions.Where(x => !locationPermissions.Contains(x.PermissionId)); 
                await rolePermissionRepository.RemoveAllAsync(toRemoveRolePermissions);
            }

            var toRemoveUserPermissions = await userPermissionRepository.GetAllAsync(x => x, x => x.LocationId == locationId && !locationPermissions.Contains(x.PermissionId));

            await userPermissionRepository.RemoveAllAsync(toRemoveUserPermissions);
        }

        public async ValueTask<IEnumerable<RoleKeyValueDto>> GetRolesAsync(int locationId) 
        {
            var repository = _DataRepositoryFactory.Get<Role>();

            var result = await repository.GetAllAsync(x => x.Select(r => new RoleKeyValueDto {
                Id = r.Id,
                Name = r.Name,
                DisplayName = r.DisplayName
            }).OrderBy(x => x.Name), x => x.LocationId == locationId);

            return result;
        }

        public async ValueTask<IEnumerable<KeyValueDto>> GetListAsync(string name, int? id)
        {
            var repository = _DataRepositoryFactory.Get<Location>();

            var predicate = PredicateBuilder.New<Location>(x => !x.Deleted);

            //Filters
            if(!string.IsNullOrEmpty(name))
                predicate = predicate.And(x => x.Name.Contains(name));
            
            if (id.HasValue)
                predicate = predicate.And(x => x.Id == id);

            var result = await repository.GetAllAsync(x => x.OrderBy(x => x.Name).Select(l => new KeyValueDto
            {
                Id = l.Id,
                Name = l.Name
            }), predicate);

            return result;
        }
    }
}