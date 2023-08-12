using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiComunidadPro.Business.Entities.Dtos
{
    public class UserCreatePayloadDto : UserEditPayloadDto
    {
        public string Password { get; set; }
    }
}