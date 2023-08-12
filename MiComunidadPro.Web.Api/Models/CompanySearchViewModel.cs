using System.ComponentModel.DataAnnotations;

namespace MiComunidadPro.Web.Api.Models
{
    public class CompanySearchViewModel
    {
        public string Name { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}