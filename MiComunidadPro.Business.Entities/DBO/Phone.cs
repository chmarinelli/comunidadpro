using System.ComponentModel.DataAnnotations;
using MiComunidadPro.Business.Entities.IDENTITY;
using MiComunidadPro.Business.Entities.LOCATION;
using MiComunidadPro.Common.Base;
using MiComunidadPro.Common.Contracts;

namespace MiComunidadPro.Business.Entities.DBO
{
    public class Phone : AuditableEntityBase<Dictionary>, IDeleteableEntity
    {
        [Key]
        public int Id { get; set; }
        public int? CompanyId { get; set; }
        public virtual Company Company { get; set; }
        public int? LocationId { get; set; }
        public virtual Location Location { get; set; }
        public int? UserId { get; set; }
        public virtual User User { get; set; }
        // public int? ContactId { get; set; }
        // public  virtual Contact Contact { get; set; }
        public int CategoryId { get; set; }
        public virtual Dictionary Category { get; set; }
        [Required]
        [MaxLength(20)]
        public string Number { get; set; }
        [MaxLength(5)]
        public string Extension { get; set; }
        public bool IsWhatsAppeable { get; set; }
        public bool Deleted { get; set; }
    }
}