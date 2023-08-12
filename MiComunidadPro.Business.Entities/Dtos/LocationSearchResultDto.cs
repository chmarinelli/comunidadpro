using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiComunidadPro.Business.Entities.Dtos
{
    public class LocationSearchResultDto
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Status { get; set; }
    }
}