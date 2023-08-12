using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
    public class LocationsController : ApiControllerBase
    {
        private readonly IBusinessEngineFactory _BusinessEngineFactory;
        private readonly IMapper _Mapper;

        public LocationsController(IBusinessEngineFactory businessEngineFactory, IMapper mapper)
        {
            _BusinessEngineFactory = businessEngineFactory;
            _Mapper = mapper;
        }

        [HttpGet("search")]
        public async Task<IPagedList<LocationSearchResultDto>> SearchAsync([FromQuery] LocationSearchViewModel payload)
        {
            var engine = _BusinessEngineFactory.Get<ILocationEngine>();

            var result = await engine.SearchAsync(new LocationSearchPayloadDto {
                Name = payload.Name
            }, payload.PageIndex, payload.PageSize);

            return result;
        }

        [HttpPost("create")]
        public async Task<int> CreateAsync([FromBody] LocationCreateEditViewModel payload)
        {
            var engine = _BusinessEngineFactory.Get<ILocationEngine>();
            
            var model = _Mapper.Map<LocationCreateEditPayloadDto>(payload);

            var result = await engine.CreateAsync(model);

            return result;
        }

        [HttpGet("{id}")]
        public async Task<LocationDto> GetAsync([FromRoute] int id)
        {
            var engine = _BusinessEngineFactory.Get<ILocationEngine>();

            var result = await engine.GetAsync(id);

            return result;
        }

        
        [HttpPut("{id}/activate")]
        public async Task ActivateAsync([FromRoute] int id)
        {
            var engine = _BusinessEngineFactory.Get<ILocationEngine>();

            await engine.ActivateAsync(id);

        }

        [HttpPut("{id}/update")]
        public async Task UpdateAsync([FromBody] LocationCreateEditViewModel payload)
        {
            var engine = _BusinessEngineFactory.Get<ILocationEngine>();

            var model = _Mapper.Map<LocationCreateEditPayloadDto>(payload);

            await engine.UpdateAsync(model);

        }

        [HttpGet("{id}/master/configurations")]
        public async Task<MasterLocationConfigurationDto> GetMasterConfigurationsAsync([FromRoute] int id)
        {
            var engine = _BusinessEngineFactory.Get<ILocationEngine>();

            var result = await engine.GetMasterConfigurationsAsync(id);

            return result;
        }

        [HttpPut("{id}/master/configurations")]
        public async Task UpdateMasterConfigurationAsync([FromRoute] int id, [FromBody] MasterLocationConfigurationEditPayloadDto payload)
        {
            var engine = _BusinessEngineFactory.Get<ILocationEngine>();

            var model = _Mapper.Map<MasterLocationConfigurationEditPayloadDto>(payload);

            await engine.UpdateMasterConfigurationAsync(model);
        }

        [HttpGet("{id}/permissions")]
        public async Task<IEnumerable<KeyValueDto>> GetPermissionsAsync([FromRoute] int id)
        {
            var engine = _BusinessEngineFactory.Get<ILocationEngine>();

            var result = await engine.GetPermissionsAsync(id);

            return result;
        }

        [HttpPut("{id}/permissions")]
        public async Task UpdatePermissionsAsync([FromRoute] int id, [FromBody] List<KeyValueDto<int>> payload)
        {
            var engine = _BusinessEngineFactory.Get<ILocationEngine>();

            await engine.AddPermissionsAsync(id, payload);
        }

        [HttpGet("list")]
        public async Task<IEnumerable<KeyValueDto>> GetListAsync([FromQuery] string name, [FromQuery] int? id)
        {
            var engine = _BusinessEngineFactory.Get<ILocationEngine>();

            var result = await engine.GetListAsync(name, id);

            return result;
        }
    }
}