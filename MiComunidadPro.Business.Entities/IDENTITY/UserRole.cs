using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MiComunidadPro.Common.Contracts;
using Microsoft.AspNetCore.Identity;

namespace MiComunidadPro.Business.Entities.IDENTITY
{
    public class UserRole : IdentityUserRole<int>, IAuditableEntity
    {
        public virtual User User { get; set; }
        public virtual Role Role { get; set; }

        public DateTimeOffset CreationDate { get; set; }
        public int CreateBy { get; set; }
    }
}