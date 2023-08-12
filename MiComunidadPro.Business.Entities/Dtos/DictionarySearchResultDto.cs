using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiComunidadPro.Business.Entities.Dtos
{
    public class DictionarySearchResultDto
    {
        public int Id { get; set; }
        public string LocationName { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public bool IsReadOnly { get; set; }
    }
}