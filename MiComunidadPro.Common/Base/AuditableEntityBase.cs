using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using MiComunidadPro.Common.Contracts;

namespace MiComunidadPro.Common.Base
{
    [DataContract]
    public abstract class AuditableEntityBase<T> : EntityBase<T>, IAuditableEntity
    {
        public DateTimeOffset CreationDate { get; set; }
        public int CreateBy { get; set; }
    }
}
