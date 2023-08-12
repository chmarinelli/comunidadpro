using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MiComunidadPro.Common.Dtos;

namespace MiComunidadPro.Business.Entities.Dtos
{
    public class AuthenticatedUserDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }
}