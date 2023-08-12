
namespace MiComunidadPro.Common.Contracts
{
    public interface IBusinessEngine
    {
    }

    public interface IBusinessEngine<T> : IBusinessEngine
        where T : class, new()
    {
    }
}
