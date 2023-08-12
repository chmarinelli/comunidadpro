using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using MiComunidadPro.Business.Engines.Contracts;
using MiComunidadPro.Business.Engines;
using MiComunidadPro.Common;
using MiComunidadPro.Resources;
using MiComunidadPro.Common.Contracts;
using MiComunidadPro.Business;
using System.Linq;
using MiComunidadPro.Business.Settings;
using MiComunidadPro.Web.Infrastructure;

namespace MiComunidadPro.Web.Api.Infrastructure.Installers
{
    public class EngineInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            var azureSettingValues = new AzureSetting();
            configuration.GetSection(nameof(AzureSetting)).Bind(azureSettingValues);

            if(!services.Any(x => x.ServiceType == typeof(AzureSetting)))
                services.AddSingleton(azureSettingValues);

            services.AddScoped<IBusinessEngineFactory, BusinessEngineFactory>();

            //Engines injection            
            var engineTypes =
                typeof(BusinessEngineFactory).Assembly
                                             .ExportedTypes
                                             .Where(x => typeof(IBusinessEngine).IsAssignableFrom(x) &&
                                                         !x.IsInterface &&
                                                         !x.IsAbstract).ToList();

            engineTypes.ForEach(engineType =>
            {
                services.AddScoped(engineType.GetInterface($"I{engineType.Name}"),
                                   engineType);
            });
        }
    }
}
