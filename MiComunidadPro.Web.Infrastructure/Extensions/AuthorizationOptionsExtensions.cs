using MiComunidadPro.Web.Infrastructure.Constants;
using MiComunidadPro.Web.Infrastructure.Policy;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MiComunidadPro.Web.Infrastructure.Extensions
{
    public static class AuthorizationOptionsExtensions
    {
        public static void SetEndpointPolicies(this AuthorizationOptions options)
        {
            options.AddPolicy(PolicyName.ClientApplicationAccess, policy =>
            {
                policy.Requirements.Add(new ClientApplicationAccessRequirement());
            });

            options.AddPolicy(PolicyName.WebApi, policy =>
            {
                policy.RequireAssertion(x =>
                {
                    return x.User.Claims.Any(x => x.Type == "scope" && x.Value == ScopeName.WebApi);
                });
            });
        }
    }
}
