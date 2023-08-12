using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiComunidadPro.Web.Api.Models
{
    public class MasterLocationConfigurationEditViewModel
    {
        public int LocationId { get; set; }
        public int MaxUsers { get; set; }
        public int MaxUnits { get; set; }
        public int MaxPOF { get; set; }
        public int MaxUnitUsers { get; set; }
        public int PaymentGenerationDay { get; set; }
        public int PaymentInvoiceGenerationDay { get; set; }
        public int MaintenancePaymentDay { get; set; }
        public bool MaintenanceLatePayment { get; set; }
        public int MaintenancePaymentDelayGenerationDay { get; set; }
    }
}