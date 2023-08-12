using System;

namespace MiComunidadPro.Common.Contracts
{
    public interface IAuditableEntity
    {
        DateTimeOffset CreationDate { get; set; }
        int CreateBy { get; set; }
    }
}
