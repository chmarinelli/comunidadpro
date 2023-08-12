using MiComunidadPro.Common;
using MiComunidadPro.Common.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MiComunidadPro.Business.Contracts;
using MiComunidadPro.Business.Handlers;
using MiComunidadPro.Resources;
using MiComunidadPro.Web.Infrastructure.Settings;
using MiComunidadPro.Web.Services;
using System.Linq;
using Microsoft.AspNetCore.Http.Json;
using MiComunidadPro.Common.Converters;

namespace MiComunidadPro.Web.Infrastructure.Installers
{
    public class DefaultInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            //Set the AppSettings information
            var appSettingValues = new AppSettings();
            var emailSettingValues = new EmailSetting();

            configuration.GetSection(nameof(AppSettings)).Bind(appSettingValues);
            configuration.GetSection(nameof(EmailSetting)).Bind(emailSettingValues);

            services.AddSingleton<AppSettings>(appSettingValues);
            services.AddSingleton<EmailSetting>(emailSettingValues);

            var authorizerSetting = new AuthorizerSetting();
            configuration.GetSection("Authorizer").Bind(authorizerSetting);

            if(!services.Any(x => x.ServiceType == typeof(AuthorizerSetting)))
                services.AddSingleton(authorizerSetting);

            //Handlers and Resources
            services.AddSingleton<IMessagesResourceHandler, MessagesResourceHandler>();
            services.AddSingleton<IMessageHandler, MessageHandler>();

            // Add application services.
            services.AddTransient<ISMTPEmailSender, SMTPEmailSender>();
            services.AddTransient<IRequestProvider, RequestProvider>();

            //User Profile
            services.AddScoped<IUserProfile, UserProfile>();

             //Json Options
            services.Configure<JsonOptions>((s) =>
            {
                s.SerializerOptions.Converters.Add(new DictionaryConverter());
                s.SerializerOptions.Converters.Add(new TimeSpanConverter());
                s.SerializerOptions.Converters.Add(new NullableTimeSpanConverter());
            });

            services.AddSingleton<JsonOptions>((s) =>
            {
                var opt = new JsonOptions();

                opt.SerializerOptions.Converters.Add(new DictionaryConverter());
                opt.SerializerOptions.Converters.Add(new TimeSpanConverter());
                opt.SerializerOptions.Converters.Add(new NullableTimeSpanConverter());

                return opt;
            });
        }
    }
}
