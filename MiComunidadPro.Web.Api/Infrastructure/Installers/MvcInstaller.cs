
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;



using System.Globalization;
using System;
using System.Linq;
using MiComunidadPro.Web.Infrastructure.Settings;
using MiComunidadPro.Web.Infrastructure;
using System.Security.Claims;
using System.Text.Json;
using MiComunidadPro.Common.Converters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using MiComunidadPro.Web.Infrastructure.Events;
using MiComunidadPro.Web.Infrastructure.Extensions;
using MiComunidadPro.Business.Entities.DBO;
using MiComunidadPro.Data;
using Microsoft.AspNetCore.Identity;
using MiComunidadPro.Business.Entities.IDENTITY;

namespace MiComunidadPro.Web.Api.Infrastructure.Installers
{
    public class MvcInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            var authorizerSetting = new AuthorizerSetting();
            configuration.GetSection("Authorizer").Bind(authorizerSetting);

            if(!services.Any(x => x.ServiceType == typeof(AuthorizerSetting)))
                services.AddSingleton(authorizerSetting);

            //Antiforgery Setup
            services.AddAntiforgery(options => options.HeaderName = "X-CSRF-TOKEN");

            //Add Culture
            var cultureInfo = new CultureInfo("es-DO");
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            services.AddControllers()
                    .AddJsonOptions(options =>
                    {
                        options.JsonSerializerOptions.WriteIndented = true;
                        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                        options.JsonSerializerOptions.Converters.Add(new DictionaryConverter());
                        options.JsonSerializerOptions.Converters.Add(new TimeSpanConverter());
                        options.JsonSerializerOptions.Converters.Add(new NullableTimeSpanConverter());
                        options.JsonSerializerOptions.Converters.Add(new NullableDateTimeConverter());
                    });

            services.AddOptions();

            services.AddMemoryCache();
            // Identity
            services.AddIdentity<User, Role>()
                    .AddEntityFrameworkStores<ApplicationContext>()
                    .AddDefaultTokenProviders();

            /*
               * Configure Identity to use the same JWT claims as OpenIddict instead of
               * the legacy WS-Federation claims it uses by default (ClaimTypes).
               * Which saves you from doing the mapping in your authorization controller.
           */

            //JWT Authentication
            var localJwtSettings = new LocalJwtSettings();

            configuration.GetSection(nameof(LocalJwtSettings)).Bind(localJwtSettings);

            services.AddSingleton<LocalJwtSettings>(localJwtSettings);

            services.AddAuthentication(options => 
                    {
                        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    })
                    .AddJwtBearer(cfg => {
                        cfg.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateIssuerSigningKey = true,
                            ValidateLifetime = true,
                            ValidIssuer = localJwtSettings.Issuer,
                            ClockSkew = TimeSpan.Zero, // remove delay of token when expire
                            ValidAudience = localJwtSettings.Audience,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(localJwtSettings.Key)),
                            NameClaimType = ClaimTypes.NameIdentifier,
                            RoleClaimType = ClaimTypes.Role,
                        };

                        cfg.SecurityTokenValidators.Clear();
                        cfg.SecurityTokenValidators.Add(new JwtSecurityTokenHandler
                        {
                            MapInboundClaims = false,
                            TokenLifetimeInMinutes = localJwtSettings.ExpiresOn
                        });

                        cfg.Events = new ApplicationJwtBearerEvents();
                    });
                    

            services.AddMvcCore()
                    .AddAuthorization(options =>
                    {
                        options.SetEndpointPolicies();
                    })
                    .AddViews();

            
        }
    }
}
