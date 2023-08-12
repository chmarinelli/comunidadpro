using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using MiComunidadPro.Business.Engines.Contracts;
using MiComunidadPro.Business.Engines;
using MiComunidadPro.Common;
using MiComunidadPro.Data.Contracts;
using MiComunidadPro.Data;
using MiComunidadPro.Common.Contracts;
using System.Linq;

namespace MiComunidadPro.Web.Infrastructure.Installers
{
    public class RepositoryInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IDataRepositoryFactory, DataRepositoryFactory>();

            //New instance for injection
            services.AddTransient(typeof(IDataRepository<>), typeof(Repository<>));

            //Custom Repositories injection            
            var repositoryTypes =
                typeof(Repository<>).Assembly
                                    .ExportedTypes
                                    .Where(x => typeof(IDataRepository).IsAssignableFrom(x) &&
                                                !x.IsInterface &&
                                                !x.IsAbstract).ToList();

            repositoryTypes.ForEach(repositoryType =>
            {
                var contract = repositoryType.GetInterface($"I{repositoryType.Name}");

                if (contract != null)
                    services.AddScoped(contract, repositoryType);
            });
        }
    }
}
