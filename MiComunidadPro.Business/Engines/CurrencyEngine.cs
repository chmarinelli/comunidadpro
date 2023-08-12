using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinqKit;
using MiComunidadPro.Business.Engines.Contracts;
using MiComunidadPro.Business.Entities.DBO;
using MiComunidadPro.Business.Entities.Dtos;
using MiComunidadPro.Common.Contracts;

namespace MiComunidadPro.Business.Engines
{
    public class CurrencyEngine : ICurrencyEngine
    {
        private readonly IDataRepositoryFactory _DataRepositoryFactory;
        public CurrencyEngine(IDataRepositoryFactory dataRepositoryFactory) 
        {
            _DataRepositoryFactory = dataRepositoryFactory;
        }

        public async ValueTask<IPagedList<CurrencySearchResultDto>> SearchAsync(CurrencySearchPayloadDto payload, int pageIndex = 1, int pageSize = 25)
        {
            var repository = _DataRepositoryFactory.Get<Currency>();

            var predicate = PredicateBuilder.New<Currency>(x => !x.Deleted && x.LocationId == payload.LocationId);

            //Filters
            if(!string.IsNullOrEmpty(payload.Name))
                predicate = predicate.And(x => x.Name.Contains(payload.Name));

            var result = await repository.GetPagedAsync(currency => currency.Select(x => new CurrencySearchResultDto
            {
                Id = x.Id,
                Name = x.Name,
                Code = x.Code,
                Symbol = x.Symbol,
                NamePlural = x.NamePlural,
                Exchange = x.Exchange,
                IsDefault = x.IsDefault,
                IsActive = x.IsActive
            }).OrderByDescending(x => x.IsDefault).ThenBy(x => x.Name), predicate, pageIndex, pageSize);

            return result;
        }

        public async ValueTask<int> CreateAsync(CurrencyCreateEditPayload payload) 
        {
            var repository = _DataRepositoryFactory.Get<Currency>();

            var result = await repository.AddAsync(new Currency {
                LocationId = payload.LocationId,
                Name = payload.Name,
                NamePlural = payload.NamePlural,
                Symbol = payload.Symbol,
                Code = payload.Code,
                Exchange = payload.Exchange,
                IsDefault = payload.IsDefault,
                IsActive = true
            });

            return result.Id;
        }

        public async ValueTask ActivateAsync(int id) 
        {
            var repository = _DataRepositoryFactory.Get<Currency>();

            var currency = await repository.GetAsync(x => x, x => x.Id == id);
            currency.IsActive = !currency.IsActive;
            await repository.UpdateAsync(currency);
        }

        public async ValueTask UpdateAsync(CurrencyCreateEditPayload payload) 
        {
            var repository = _DataRepositoryFactory.Get<Currency>();

            var currency = await repository.GetAsync(x => x, x => x.Id == payload.Id);

            currency.Name = payload.Name;
            currency.NamePlural = payload.NamePlural;
            currency.Code = payload.Code;
            currency.Symbol = payload.Symbol;
            currency.Exchange = payload.Exchange;

            await repository.UpdateAsync(currency);
        }
    }
}