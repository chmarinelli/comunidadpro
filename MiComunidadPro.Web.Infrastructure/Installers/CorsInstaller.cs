
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

using MiComunidadPro.Business.Engines.Contracts;
using MiComunidadPro.Business.Engines;
using MiComunidadPro.Common;

using System.Linq;
using MiComunidadPro.Web.Infrastructure.Settings;

namespace MiComunidadPro.Web.Infrastructure.Installers
{
    public class CorsInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            var corsSetting = new CorsSetting();
            configuration.GetSection(nameof(CorsSetting)).Bind(corsSetting);

            if(!services.Any(x => x.ServiceType == typeof(CorsSetting)))
                services.AddSingleton(corsSetting);

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                builder =>
                {
                    builder.WithOrigins(corsSetting.AllowedOrigins)
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });
        }
    }
}
