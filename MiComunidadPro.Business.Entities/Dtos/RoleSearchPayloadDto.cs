using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiComunidadPro.Business.Entities.Dtos
{
    public class RoleSearchPayloadDto
    {
        public string Name { get; set; }
        public int? LocationId { get; set; }
    }
}