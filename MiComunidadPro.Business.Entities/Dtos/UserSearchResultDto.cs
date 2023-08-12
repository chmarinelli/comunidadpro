using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiComunidadPro.Business.Entities.Dtos
{
    public class UserSearchResultDto
    {
        public int Id { get; set; }
        public string LastName { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public DateTime? lastLogin { get; set; }
        public string Status { get; set; }
    }
}