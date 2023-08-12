using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MiComunidadPro.Business.Entities.DBO;
using MiComunidadPro.Common.Base;

namespace MiComunidadPro.Business.Entities.IDENTITY
{
    [Table("RolePermissions")]
    public class RolePermission : AuditableEntityBase<RolePermission>
    {
        [Key]
        public int Id { get; set; }
        public int RoleId { get; set; }
        public virtual Role Role { get; set; }
        public int PermissionId { get; set; }
        public virtual Permission Permission { get; set; }
    }
}