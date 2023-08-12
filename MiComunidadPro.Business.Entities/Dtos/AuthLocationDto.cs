using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiComunidadPro.Business.Entities.Dtos
{
    public class AuthLocationDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LocationStatus { get; set; }
        public string UserLocationStatus { get; set; }
    }
}