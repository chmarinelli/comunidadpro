using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MiComunidadPro.Business.Entities.LOCATION;
using MiComunidadPro.Common.Base;

namespace MiComunidadPro.Business.Entities.IDENTITY
{
    public class UserLocation : AuditableEntityBase<UserLocation>
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public int LocationId { get; set; }
        public virtual Location Location { get; set; }
        public int RoleId { get; set; }
        public virtual Role Role { get; set; }
        public bool UseFundForUnitPayment { get; set; }
        /// <summary>
        /// State set from const values in <see cref="Enums.Status.UserLocationStatus"/>     
        /// </summary>
        [MaxLength(32)]
        public string Status { get; set; }
    }
}