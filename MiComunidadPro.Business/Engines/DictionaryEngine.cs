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
    public class DictionaryEngine : IDictionaryEngine
    {
        private readonly IDataRepositoryFactory _DataRepositoryFactory;
        public DictionaryEngine(IDataRepositoryFactory dataRepositoryFactory) 
        {
            _DataRepositoryFactory = dataRepositoryFactory;
        }

        public async ValueTask<IPagedList<DictionarySearchResultDto>> SearchAsync(DictionarySearchPayloadDto payload, int pageIndex = 1, int pageSize = 25)
        {
            var repository = _DataRepositoryFactory.Get<Dictionary>();

            var predicate = PredicateBuilder.New<Dictionary>(x => !x.Deleted);

            //Filters
            if (!string.IsNullOrEmpty(payload.ParentCode)) {
                predicate = predicate.And(x => x.Parent.Name == payload.ParentCode);
            }
            
            if (payload.LocationId.HasValue)
                predicate = predicate.And(x => x.LocationId == payload.LocationId.Value || (payload.IncludeCommon ? !x.LocationId.HasValue : false));


            if (!string.IsNullOrEmpty(payload.Name))
                predicate = predicate.And(x => x.Name.Contains(payload.Name) || (!x.LocationId.HasValue ? x.Location.Name.Contains(payload.Name) : false));

            var result = await repository.GetPagedAsync(location => location.OrderBy(x => x.Name).Select(x => new DictionarySearchResultDto
            {
                Id = x.Id,
                Name = x.Name,
                LocationName = x.Location.Name,
                Note = x.Note,
                IsReadOnly = x.IsReadOnly
            }), predicate, pageIndex, pageSize);

            return result;
        }
    }
}