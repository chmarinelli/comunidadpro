using System;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using MiComunidadPro.Common.Base;
using MiComunidadPro.Data.Contracts;
using Microsoft.EntityFrameworkCore;

namespace MiComunidadPro.Data
{
    public class FunctionRepository : RepositoryBase<FunctionEntityBase, ApplicationContext>, IFunctionRepository
    {
        public FunctionRepository(ApplicationContext context) : base(context) { }

    }
}
