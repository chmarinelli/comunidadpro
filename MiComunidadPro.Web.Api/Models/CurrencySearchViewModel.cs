using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiComunidadPro.Web.Api.Models
{
    public class CurrencySearchViewModel
    {
        public string Name { get; set; }
        public int LocationId { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}