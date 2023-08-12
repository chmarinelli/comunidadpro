using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MiComunidadPro.Common.Base;
using MiComunidadPro.Common.Contracts;

namespace MiComunidadPro.Business.Entities.DBO
{
    [Table("Companies")]
    public class Company : AuditableEntityBase<Company>, IDeleteableEntity
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(128)]
        public string Name { get; set; }
        [MaxLength(256)]
        public string Note { get; set; }
        [MaxLength(256)]
        public string Address { get; set; }
        public string LogoPath { get; set; }
        public bool Deleted { get; set; }
    }
}