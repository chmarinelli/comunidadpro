using System.Collections.Generic;
using System.Threading.Tasks;
using MiComunidadPro.Business.Engines.Contracts;
using MiComunidadPro.Business.Entities.Dtos;
using MiComunidadPro.Common.Contracts;
using MiComunidadPro.Common.Dtos;
using MiComunidadPro.Web.Api.Models;
using MiComunidadPro.Web.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MiComunidadPro.Web.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CompaniesController : ApiControllerBase
    {
        private readonly IBusinessEngineFactory _BusinessEngineFactory;

        public CompaniesController(IBusinessEngineFactory businessEngineFactory)
        {
            _BusinessEngineFactory = businessEngineFactory;
        }

        [HttpGet("search")]
        public async Task<IPagedList<CompanySearchResultDto>> SearchAsync([FromQuery] CompanySearchViewModel payload)
        {
            var companyEngine = _BusinessEngineFactory.Get<ICompanyEngine>();

            var result = await companyEngine.SearchAsync(new CompanySearchPayloadDto {
                Name = payload.Name
            }, payload.PageIndex, payload.PageSize);

            return result;
        }
        
        [HttpGet("list")]
        public async Task<IEnumerable<KeyValueDto>> GetListAsync([FromQuery] string name, [FromQuery] int? id)
        {
            var companyEngine = _BusinessEngineFactory.Get<ICompanyEngine>();

            var result = await companyEngine.GetListAsync(name, id);

            return result;
        }
    }
}