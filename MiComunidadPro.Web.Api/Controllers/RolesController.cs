using System.Collections.Generic;
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
    public class RolesController : ApiControllerBase
    {
        private readonly IBusinessEngineFactory _BusinessEngineFactory;
        private readonly IMapper _Mapper;

        public RolesController(IBusinessEngineFactory businessEngineFactory, IMapper mapper)
        {
            _BusinessEngineFactory = businessEngineFactory;
            _Mapper = mapper;
        }

        [HttpGet("search")]
        public async Task<IPagedList<RoleSearchResultDto>> SearchAsync([FromQuery] RoleSearchViewModel payload)
        {
            var engine = _BusinessEngineFactory.Get<IRoleEngine>();

            var result = await engine.SearchAsync(new RoleSearchPayloadDto {
                Name = payload.Name,
                LocationId = payload.LocationId
            }, payload.PageIndex, payload.PageSize);

            return result;
        }

        [HttpPost("create")]
        public async Task<int> CreateAsync([FromBody] RoleCreateEditViewModel payload)
        {
            var engine = _BusinessEngineFactory.Get<IRoleEngine>();
            
            var model = _Mapper.Map<RoleCreateEditPayloadDto>(payload);

            var result = await engine.CreateAsync(model);

            return result;
        }

        [HttpGet("{id}")]
        public async Task<RoleDto> GetAsync([FromRoute] int id)
        {
            var engine = _BusinessEngineFactory.Get<IRoleEngine>();

            var result = await engine.GetAsync(id);

            return result;
        }

        [HttpPut("{id}/update")]
        public async Task UpdateAsync([FromBody] RoleCreateEditViewModel payload)
        {
            var engine = _BusinessEngineFactory.Get<IRoleEngine>();

            var model = _Mapper.Map<RoleCreateEditPayloadDto>(payload);

            await engine.UpdateAsync(model);

        }

        [HttpGet("{id}/permissions")]
        public async Task<IEnumerable<KeyValueDto>> GetPermissionsAsync([FromRoute] int id)
        {
            var engine = _BusinessEngineFactory.Get<IRoleEngine>();

            var result = await engine.GetPermissionsAsync(id);

            return result;
        }

        [HttpPut("{id}/permissions")]
        public async Task UpdatePermissionsAsync([FromRoute] int id, [FromBody] List<KeyValueDto<int>> payload)
        {
            var engine = _BusinessEngineFactory.Get<IRoleEngine>();

            await engine.AddPermissionsAsync(id, payload);
        }
    }
}