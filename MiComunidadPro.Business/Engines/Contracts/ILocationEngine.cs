using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MiComunidadPro.Business.Entities.Dtos;
using MiComunidadPro.Common.Contracts;
using MiComunidadPro.Common.Dtos;

namespace MiComunidadPro.Business.Engines.Contracts
{
    public interface ILocationEngine : IBusinessEngine
    {
        ValueTask<IPagedList<LocationSearchResultDto>> SearchAsync(LocationSearchPayloadDto payload, int pageIndex = 1, int pageSize = 25);
        ValueTask<int> CreateAsync(LocationCreateEditPayloadDto payload);
        ValueTask<LocationDto> GetAsync(int id);
        ValueTask ActivateAsync(int id);
        ValueTask UpdateAsync(LocationCreateEditPayloadDto payload);
        ValueTask<MasterLocationConfigurationDto> GetMasterConfigurationsAsync(int locationId);
        ValueTask UpdateMasterConfigurationAsync(MasterLocationConfigurationEditPayloadDto payload);
        ValueTask<IEnumerable<KeyValueDto>> GetPermissionsAsync(int locationId);
        ValueTask AddPermissionsAsync(int locationId, List<KeyValueDto<int>> payload);
        ValueTask<IEnumerable<KeyValueDto>> GetListAsync(string name, int? id);
    }
}