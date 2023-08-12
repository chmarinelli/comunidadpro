
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

using MiComunidadPro.Business.Engines.Contracts;
using MiComunidadPro.Business.Engines;
using MiComunidadPro.Common;

using System.Globalization;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Builder;
using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using MiComunidadPro.Business.Contracts;
using MiComunidadPro.Web.Services;
using MiComunidadPro.Business.Handlers;

namespace MiComunidadPro.Web.Infrastructure.Installers
{
    public class HttpContextInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            //Globally used objects
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        }
    }
}
