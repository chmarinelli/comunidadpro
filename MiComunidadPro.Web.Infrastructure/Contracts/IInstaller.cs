using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MiComunidadPro.Web.Infrastructure
{
    public interface IInstaller
    {
        void InstallServices(IServiceCollection services, IConfiguration configuration);
    }
}
