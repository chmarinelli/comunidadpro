using Microsoft.EntityFrameworkCore.Storage;

namespace MiComunidadPro.Common.Contracts
{
    public interface IUnitOfWork
    {
        IDbContextTransaction CreateTransaction();

        int SaveChanges();
    }
}
