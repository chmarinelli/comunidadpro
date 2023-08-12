using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MiComunidadPro.Web.Api.Models
{
    public class LocationSearchViewModel
    {
        public string Name { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}