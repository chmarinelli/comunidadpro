using MiComunidadPro.Common;
using MiComunidadPro.Common.Base;
using MiComunidadPro.Common.Contracts;
using MiComunidadPro.Data.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading.Tasks;

namespace MiComunidadPro.Data
{
    public class StoredProcedureRepository : RepositoryBase<StoredProcedureEntityBase, ApplicationContext>, IStoredProcedureRepository
    {
        public StoredProcedureRepository(ApplicationContext context) : base(context) { }

        //public IEnumerable<TSPEntity> SampleProcedureCall(bool sampleParameter)
        //{
        //    SqlParameter parameter = new SqlParameter("@sampleParameter", sampleParameter);
              
              //Can be any DBSet, whe use Persons in this case
        //    return _Context.Persons.FromSqlRaw("EXECUTE [dbo].[SampleProcedureCall] @sampleParameter", parameter).ToList();
        //}
    }
}