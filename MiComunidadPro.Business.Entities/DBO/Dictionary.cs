using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MiComunidadPro.Business.Entities.LOCATION;
using MiComunidadPro.Common.Base;
using MiComunidadPro.Common.Contracts;

namespace MiComunidadPro.Business.Entities.DBO
{
    [Table("Dictionaries")]
    public class Dictionary : AuditableEntityBase<Dictionary>, IDeleteableEntity
    {
        [Key]
        public int Id { get; set; } 
        public int? LocationId { get; set; }
        public virtual Location Location { get; set; }
        public int? ParentId { get; set; }
        public virtual Dictionary Parent { get; set; }   
        [MaxLength(128)]
        [Required]   
        public string Code { get; set; } 
        [MaxLength(64)]
        [Required]      
        public string Name { get; set; }
        [MaxLength(256)]
        public string Note { get; set; }
        public bool IsReadOnly { get; set; }
        public bool Deleted { get; set; }
          
        public virtual ICollection<Dictionary> Children { get; set; }
    }
}