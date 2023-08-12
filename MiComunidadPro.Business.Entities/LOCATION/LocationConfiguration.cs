using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using MiComunidadPro.Common.Base;
using MiComunidadPro.Common.Contracts;

namespace MiComunidadPro.Business.Entities.LOCATION
{
    [Table("LocationConfigurations")]
    public class LocationConfiguration : AuditableEntityBase<LocationConfiguration>, IDeleteableEntity
    {
        [Key]
        public int Id { get; set; }
        public int LocationId { get; set; }
        public Location Location { get; set; }
        public int MaxUsers { get; set; }
        public int MaxUnits { get; set; }
        public int MaxPOF { get; set; }
        public int MaxUnitUsers { get; set; }
        public int PaymentGenerationDay { get; set; }
        public int PaymentInvoiceGenerationDay { get; set; }
        public int MaintenancePaymentDay { get; set; }
        public bool MaintenanceLatePayment { get; set; }
        public int MaintenancePaymentDelayGenerationDay { get; set; }
        public bool Deleted { get; set; }
    }
}