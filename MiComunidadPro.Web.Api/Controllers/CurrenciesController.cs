using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MiComunidadPro.Business.Engines.Contracts;
using MiComunidadPro.Business.Entities.Dtos;
using MiComunidadPro.Common.Contracts;
using MiComunidadPro.Web.Api.Models;
using MiComunidadPro.Web.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MiComunidadPro.Web.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CurrenciesController : ApiControllerBase
    {
        private readonly IBusinessEngineFactory _BusinessEngineFactory;
         private readonly IMapper _Mapper;

        public CurrenciesController(IBusinessEngineFactory businessEngineFactory, IMapper mapper)
        {
            _BusinessEngineFactory = businessEngineFactory;
            _Mapper = mapper;
        }

        [HttpGet("search")]
        public async Task<IPagedList<CurrencySearchResultDto>> SearchAsync([FromQuery] CurrencySearchViewModel payload)
        {
            var engine = _BusinessEngineFactory.Get<ICurrencyEngine>();

            var result = await engine.SearchAsync(new CurrencySearchPayloadDto {
                Name = payload.Name,
                LocationId = payload.LocationId
            }, payload.PageIndex, payload.PageSize);

            return result;
        }

        [HttpPost("create")]
        public async Task<int> CreateAsync([FromBody] CurrencyCreateEditViewModel payload)
        {
            var engine = _BusinessEngineFactory.Get<ICurrencyEngine>();
            
            var model = _Mapper.Map<CurrencyCreateEditPayload>(payload);

            var result = await engine.CreateAsync(model);

            return result;
        }

        [HttpPut("{id}/activate")]
        public async Task ActivateAsync([FromRoute] int id)
        {
            var engine = _BusinessEngineFactory.Get<ICurrencyEngine>();

            await engine.ActivateAsync(id);
        }

        [HttpPut("{id}/update")]
        public async Task UpdateAsync([FromBody] CurrencyCreateEditViewModel payload)
        {
            var engine = _BusinessEngineFactory.Get<ICurrencyEngine>();

            var model = _Mapper.Map<CurrencyCreateEditPayload>(payload);

            await engine.UpdateAsync(model);
        }
    }
}