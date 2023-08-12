using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MiComunidadPro.Business.Entities.LOCATION;
using MiComunidadPro.Common.Contracts;
using Microsoft.AspNetCore.Identity;

namespace MiComunidadPro.Business.Entities.IDENTITY
{
    [Table("Roles")]
    public class Role : IdentityRole<int>, IDeleteableEntity, IAuditableEntity
    {
        public int? LocationId { get; set; }
        public Location Location { get; set; }
        [MaxLength(64)]
        public string DisplayName { get; set; }
        public string Icon { get; set; }
        
        public bool Deleted { get; set; }
        public DateTimeOffset CreationDate { get; set; }
        public int CreateBy { get; set; }

        public virtual ICollection<UserRole>  UserRoles { get; set; }
    }
}