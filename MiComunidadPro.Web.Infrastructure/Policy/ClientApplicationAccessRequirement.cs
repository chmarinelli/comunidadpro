using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiComunidadPro.Web.Infrastructure.Policy
{
    public class ClientApplicationAccessRequirement : IAuthorizationRequirement
    {
        public ClientApplicationAccessRequirement() { }
    }
}
