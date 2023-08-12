using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiComunidadPro.Web.Api.Models
{
    public class RoleCreateEditViewModel
    {
        public int Id { get; set; }
        public int? LocationId { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
    }
}