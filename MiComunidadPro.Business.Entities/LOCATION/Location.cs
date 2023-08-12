using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using MiComunidadPro.Business.Entities.DBO;
using MiComunidadPro.Business.Entities.IDENTITY;
using MiComunidadPro.Common.Base;
using MiComunidadPro.Common.Contracts;

namespace MiComunidadPro.Business.Entities.LOCATION
{
    [Table("Locations")]
    public class Location : AuditableEntityBase<Location>, IDeleteableEntity
    {
        [Key]
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
        [MaxLength(128)]
        public string Name { get; set; }
        [MaxLength(256)]
        public string Note { get; set; }
        [MaxLength(256)]
        public string Address { get; set; }
        public string LogoPath { get; set; }
        public bool Deleted { get; set; }
        /// <summary>
        /// State set from const values in <see cref="Enums.Status.LocationStatus"/>     
        /// </summary>
        public string Status { get; set; }

        public virtual ICollection<UserLocation> UserLocations { get; set; }
    }
}