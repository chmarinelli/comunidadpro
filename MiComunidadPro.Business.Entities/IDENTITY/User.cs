using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using MiComunidadPro.Business.Entities.DBO;
using MiComunidadPro.Common.Contracts;
using Microsoft.AspNetCore.Identity;

namespace MiComunidadPro.Business.Entities.IDENTITY
{
    public class User : IdentityUser<int>, IDeleteableEntity, IAuditableEntity
    {
        [PersonalData]
        public string Name { get; set; }
        [PersonalData]
        public string LastName { get; set; }
        [NotMapped]
        public string FullName { get => $"{Name} {LastName}"; }
        // public DocumentType DocumentType { get; set; }
        public DateTime? LastLogin { get; set; }

        /// <summary>
        /// State set from const values in <see cref="Enums.Status.UserStatus"/>     
        /// </summary>
        [MaxLength(32)]
        public string Status { get; set; }
        public bool Deleted { get; set; }
        public DateTimeOffset CreationDate { get; set; }
        public int CreateBy { get; set; }

        public virtual ICollection<UserRole> Roles { get; set; }
        public virtual ICollection<Phone> Phones { get; set; }
    }
}