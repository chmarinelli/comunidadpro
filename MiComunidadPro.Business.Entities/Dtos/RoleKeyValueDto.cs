using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MiComunidadPro.Common.Dtos;

namespace MiComunidadPro.Business.Entities.Dtos
{
    public class RoleKeyValueDto : KeyValueDto
    {
        public string DisplayName { get; set; }
    }
}