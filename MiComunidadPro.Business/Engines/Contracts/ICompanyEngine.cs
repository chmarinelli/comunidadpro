using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MiComunidadPro.Business.Entities.Dtos;
using MiComunidadPro.Common.Contracts;
using MiComunidadPro.Common.Dtos;

namespace MiComunidadPro.Business.Engines.Contracts
{
    public interface ICompanyEngine : IBusinessEngine
    {
        ValueTask<IPagedList<CompanySearchResultDto>> SearchAsync(CompanySearchPayloadDto payload, int pageIndex = 1, int pageSize = 25);
        ValueTask<IEnumerable<KeyValueDto>> GetListAsync(string name, int? id);
    }
}