using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MiComunidadPro.Business.Entities.Dtos;
using MiComunidadPro.Common.Contracts;
using MiComunidadPro.Common.Dtos;

namespace MiComunidadPro.Business.Engines.Contracts
{
    public interface IRoleEngine : IBusinessEngine
    {
        ValueTask<IPagedList<RoleSearchResultDto>> SearchAsync(RoleSearchPayloadDto payload, int pageIndex = 1, int pageSize = 25);
        ValueTask<int> CreateAsync(RoleCreateEditPayloadDto payload);
        ValueTask<RoleDto> GetAsync(int id);
        ValueTask UpdateAsync(RoleCreateEditPayloadDto payload);
        ValueTask<IEnumerable<KeyValueDto>> GetPermissionsAsync(int id);
        ValueTask AddPermissionsAsync(int id, List<KeyValueDto<int>> payload);
    }
}