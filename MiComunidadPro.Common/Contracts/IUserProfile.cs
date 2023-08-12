
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MiComunidadPro.Common.Contracts
{
    public interface IUserProfile
    {
        int UserId { get; }
        int RoleId {get; }

        string UserName { get; }

        string FullName { get; }

        string RoleName { get; }

        string Email { get; }

        Task<bool> HasPermissionAsync(string permissionName);
    }
}
