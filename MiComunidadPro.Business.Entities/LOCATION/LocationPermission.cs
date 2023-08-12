using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using MiComunidadPro.Business.Entities.DBO;
using MiComunidadPro.Common.Base;

namespace MiComunidadPro.Business.Entities.LOCATION
{
    [Table("LocationPermissions")]
    public class LocationPermission : AuditableEntityBase<LocationPermission>
    {
        [Key]
        public int Id { get; set; }
        public int LocationId { get; set; }
        public virtual Location Location { get; set; }
        public int PermissionId { get; set; }
        public virtual Permission Permission { get; set; }
    }
}