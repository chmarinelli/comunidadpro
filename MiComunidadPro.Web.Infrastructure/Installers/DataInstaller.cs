using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.Linq;
using MiComunidadPro.Data;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MiComunidadPro.Web.Infrastructure.Settings;

namespace MiComunidadPro.Web.Infrastructure.Installers
{
    public class DataInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            var connectionStrings = new ConnectionStrings();
            configuration.GetSection(nameof(ConnectionStrings)).Bind(connectionStrings);

            if(!services.Any(x => x.ServiceType == typeof(ConnectionStrings)))
                services.AddSingleton(connectionStrings);

            var mainConnectionString = connectionStrings.MainConnection;

            //One instance per request
            services.AddDbContext<ApplicationContext>(options =>
            {
                //NOTE: There is an AppSettings.json for TEST Projects picks the one of their project

                var connection = new SqlConnection(mainConnectionString);

                options.UseSqlServer(connection);
            });
        }
    }
}
