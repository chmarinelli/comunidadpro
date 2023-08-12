using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiComunidadPro.Business.Entities.Dtos
{
    public class PhoneDto
    {
        public int Id { get; set; }
        public int? CompanyId { get; set; }
        public int? LocationId { get; set; }
        public int? UserId { get; set; }
        // public int? ContactId { get; set; }
        public int CategoryId { get; set; }
        public string Number { get; set; }
        public string Extension { get; set; }
        public bool IsWhatsAppeable { get; set; }
    }
}