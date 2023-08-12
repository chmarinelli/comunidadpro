using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Threading.Tasks;

namespace MiComunidadPro.Common.Contracts
{
    public interface IApplicationProfile
    {
        int? ApplicationId { get; set; }
    }
}
