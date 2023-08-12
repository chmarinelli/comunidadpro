using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinqKit;
using MiComunidadPro.Business.Engines.Contracts;
using MiComunidadPro.Business.Entities.DBO;
using MiComunidadPro.Business.Entities.Dtos;
using MiComunidadPro.Common.Contracts;
using MiComunidadPro.Common.Dtos;

namespace MiComunidadPro.Business.Engines
{
    public class CompanyEngine : ICompanyEngine
    {
        private readonly IDataRepositoryFactory _DataRepositoryFactory;
        public CompanyEngine(IDataRepositoryFactory dataRepositoryFactory) 
        {
            _DataRepositoryFactory = dataRepositoryFactory;
        }

        public async ValueTask<IPagedList<CompanySearchResultDto>> SearchAsync(CompanySearchPayloadDto payload, int pageIndex = 1, int pageSize = 25)
        {
            var repository = _DataRepositoryFactory.Get<Company>();

            var predicate = PredicateBuilder.New<Company>(x => !x.Deleted);

            //Filters
            if(!string.IsNullOrEmpty(payload.Name))
                predicate = predicate.And(x => x.Name.Contains(payload.Name));

            var result = await repository.GetPagedAsync(company => company.OrderBy(x => x.Name).Select(x => new CompanySearchResultDto
            {
                Id = x.Id,
                Name = x.Name,
                Note = x.Note
            }), predicate, pageIndex, pageSize);

            return result;
        }

        public async ValueTask<IEnumerable<KeyValueDto>> GetListAsync(string name, int? id)
        {
            var repository = _DataRepositoryFactory.Get<Company>();

            var predicate = PredicateBuilder.New<Company>(x => !x.Deleted);

            //Filters
            if(!string.IsNullOrEmpty(name))
                predicate = predicate.And(x => x.Name.Contains(name));
            
            if (id.HasValue)
                predicate = predicate.And(x => x.Id == id);

            var result = await repository.GetAllAsync(company => company.OrderBy(x => x.Name).Select(x => new KeyValueDto
            {
                Id = x.Id,
                Name = x.Name
            }), predicate);

            return result;
        }

    }
}