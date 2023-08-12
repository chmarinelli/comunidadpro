using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MiComunidadPro.Business.Entities.LOCATION;
using MiComunidadPro.Common.Base;
using MiComunidadPro.Common.Contracts;

namespace MiComunidadPro.Business.Entities.DBO
{
    [Table("Currencies")]
    public class Currency : AuditableEntityBase<Currency>, IDeleteableEntity
    {
        [Key]
        public int Id { get; set; }
        public int LocationId { get; set; }
        public virtual Location Location { get; set; }
        [MaxLength(24)]
        [Required]
        public string Name { get; set; }
        [MaxLength(3)]
        public string Code { get; set; }
        [MaxLength(5)]
        public string Symbol { get; set; }
        [MaxLength(24)]
        public string NamePlural { get; set; }
        public bool IsDefault { get; set; }
        public double Exchange { get; set; }
        public bool IsActive { get; set; }  
        public bool Deleted { get; set; }
    }
}