
namespace MiComunidadPro.Common.Contracts
{
    public interface IBusinessEngineFactory
    {
        T Get<T>() where T : IBusinessEngine;
    }
}
