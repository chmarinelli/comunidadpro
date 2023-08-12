using MiComunidadPro.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MiComunidadPro.Business.Engines.Contracts
{
    public interface IAuthorizationEngine : IBusinessEngine
    {
        Task<string[]> GetUserPermissionsAsync(Guid? userId = null);

        Task<string[]> GetRolePermissionsAsync(Guid? roleId = null);

        Task<string[]> GetUserAndRolePermissionsAsync(Guid? userId = null);
    }
}