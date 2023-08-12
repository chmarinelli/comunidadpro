using AutoMapper;
using MiComunidadPro.Business.Entities.Dtos;
using MiComunidadPro.Common;
using MiComunidadPro.Web.Api.Models;

namespace MiComunidadPro.Web.Api.Infrastructure.AutoMapper
{
    public class AutoMapperProfileConfiguration : Profile
    {
        public AutoMapperProfileConfiguration()
        : this("MiComunidadProProfile")
        {
        }
        protected AutoMapperProfileConfiguration(string profileName)
        : base(profileName)
        {
            this.RecognizePrefixes("Config");

            CreateMap<LocationCreateEditViewModel, LocationCreateEditPayloadDto>();
            CreateMap<RoleCreateEditViewModel, RoleCreateEditPayloadDto>();

            CreateMap<CurrencyCreateEditViewModel, CurrencyCreateEditPayload>();
        }
    }
}
