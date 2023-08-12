using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MiComunidadPro.Business.Entities.Dtos;
using MiComunidadPro.Common.Contracts;

namespace MiComunidadPro.Business.Engines.Contracts
{
    public interface ICurrencyEngine : IBusinessEngine
    {
        ValueTask<IPagedList<CurrencySearchResultDto>> SearchAsync(CurrencySearchPayloadDto payload, int pageIndex = 1, int pageSize = 25);
        ValueTask<int> CreateAsync(CurrencyCreateEditPayload payload);
        ValueTask ActivateAsync(int id);
        ValueTask UpdateAsync(CurrencyCreateEditPayload payload);
    }
}