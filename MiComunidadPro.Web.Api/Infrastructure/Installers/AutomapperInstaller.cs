using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using AutoMapper;
using MiComunidadPro.Web.Api.Infrastructure.AutoMapper;
using MiComunidadPro.Web.Infrastructure;
using MiComunidadPro.Web.Infrastructure.AutoMapper;

namespace MiComunidadPro.Web.Api.Infrastructure.Installers
{
    public class AutomapperInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfileConfiguration());
                cfg.AddProfile(new AutoMapperCommonProfileConfiguration());
            });

            var mapper = config.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}
