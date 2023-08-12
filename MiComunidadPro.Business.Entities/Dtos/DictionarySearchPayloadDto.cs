using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiComunidadPro.Business.Entities.Dtos
{
    public class DictionarySearchPayloadDto
    {
        public string Name { get; set; }
        public string ParentCode { get; set; }
        public int? LocationId { get; set; }
        public bool IncludeCommon { get; set; }
    }
}