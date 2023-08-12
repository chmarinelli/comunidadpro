using MiComunidadPro.Common;
using MiComunidadPro.Common.Base;
using MiComunidadPro.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiComunidadPro.Data.Contracts
{
    public interface IStoredProcedureRepository : IDataRepository<StoredProcedureEntityBase>
    {
        //IEnumerable<TSPEntity> SampleProcedureCall(bool sampleParameter);
    }
}
