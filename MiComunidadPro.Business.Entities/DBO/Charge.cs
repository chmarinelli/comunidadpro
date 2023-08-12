using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using MiComunidadPro.Business.Entities.Enums;
using MiComunidadPro.Business.Entities.LOCATION;
using MiComunidadPro.Common.Base;
using MiComunidadPro.Common.Contracts;

namespace MiComunidadPro.Business.Entities.DBO
{
    [Table("Charges")]
    public class Charge : AuditableEntityBase<Charge>, IDeleteableEntity
    {
        public int Id { get; set; }
        public int LocationId { get; set; }
        public Location Location { get; set; }
        public POFOrderType OrderType { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public bool IsTax { get; set; }
        public bool IsPercent { get; set; }
        public double? Amount { get; set; }
        public bool Shortlisted { get; set; }
        public bool Mandatory { get; set; }
        public bool IsReadOnly { get; set; }
        public bool IsActive { get; set; }
        public bool Deleted { get; set; }
    }
}