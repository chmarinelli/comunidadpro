using MiComunidadPro.Common.Base;

namespace MiComunidadPro.Data
{
    public class Repository<TEntity> : RepositoryBase<TEntity, ApplicationContext> where TEntity : class, new()
    {
        public Repository(ApplicationContext context) : base(context) {  }
    }
}
