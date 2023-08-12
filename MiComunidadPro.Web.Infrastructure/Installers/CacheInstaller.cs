using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using MiComunidadPro.Common.Contracts;
using MiComunidadPro.Web.Infrastructure;
using MiComunidadPro.Web.Infrastructure.Services;
using MiComunidadPro.Common.Settings;
using MiComunidadPro.Web.Infrastructure.Contracts;

namespace MiComunidadPro.Web.Api.Infrastructure.Installers
{
    public class CacheInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            var redisCacheSettings = new CacheSettings();
            var redisSection = configuration.GetSection(nameof(CacheSettings));

            redisSection.Bind(redisCacheSettings);
            services.AddSingleton(redisCacheSettings);

            services.AddStackExchangeRedisCache((options) => {
                options.Configuration = redisCacheSettings.ConnectionString;
            });

            services.AddSingleton<ICacheService, CacheService>();
            services.AddSingleton<IResponseCacheService, ResponseCacheService>();
        }
    }
}
