using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using MiComunidadPro.Business.Entities.IDENTITY;
using MiComunidadPro.Business.Entities.LOCATION;
using MiComunidadPro.Common.Base;

namespace MiComunidadPro.Business.Entities.DBO
{
    [Table("Permissions")]
    public class Permission : AuditableEntityBase<Permission>
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public bool IsForLocation { get; set; }
        public ICollection<UserPermission> UserPermissions { get; set; }
        public ICollection<RolePermission> RolePermissions { get; set; }
        public ICollection<LocationPermission> LocationPermissions { get; set; }
    }
}