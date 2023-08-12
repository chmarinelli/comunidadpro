using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiComunidadPro.Business.Entities.Dtos
{
    public class CurrencySearchResultDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Symbol { get; set; }
        public string NamePlural { get; set; }
        public bool IsDefault { get; set; }
        public double Exchange { get; set; }
        public bool IsActive { get; set; }
    }
}