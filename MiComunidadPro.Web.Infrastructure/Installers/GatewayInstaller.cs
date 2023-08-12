using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using MiComunidadPro.Common.Contracts;
using MiComunidadPro.Business;
using System.Net.Http;
using System;

namespace MiComunidadPro.Web.Infrastructure.Installers
{
    public class GatewayInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IGatewayFactory, GatewayFactory>();
        }
    }
}
