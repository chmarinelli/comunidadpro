using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MiComunidadPro.Business.Entities.Dtos;
using MiComunidadPro.Common.Contracts;
using MiComunidadPro.Common.Dtos;

namespace MiComunidadPro.Business.Engines.Contracts
{
    public interface IUserEngine : IBusinessEngine
    {
        ValueTask<IEnumerable<AuthLocationDto>> GetLocationsAsync(int userId);
        ValueTask<IEnumerable<string>> GetAllPermissionsAsync(int userId, int roleId);
        ValueTask<KeyValueDto> GetRoleAsync(int userId, int? locationId = null);
    }
}