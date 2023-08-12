using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiComunidadPro.Web.Api.Models
{
    public class LocationCreateEditViewModel
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public string Address { get; set; }
    }
}