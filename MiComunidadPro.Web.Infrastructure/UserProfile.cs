using MiComunidadPro.Common.Contracts;
using MiComunidadPro.Common.Exceptions;
using MiComunidadPro.Business.Engines.Contracts;
using MiComunidadPro.Web.Infrastructure.Settings;
using MiComunidadPro.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MiComunidadPro.Web.Infrastructure
{
    public class UserProfile : IUserProfile
    {
        private readonly IEnumerable<Claim> _Claims;
        private readonly IServiceProvider _ServiceLocator;
        private IAuthorizationEngine _AuthorizationEngine;

        public int UserId
        {
            get
            {
                if(_Claims.Any(x => x.Type == ClaimTypes.Sid))
                {
                    return int.Parse(_Claims.First(x => x.Type == ClaimTypes.Sid).Value);
                }

                return 0;
            }
        }

        public int RoleId
        {
            get
            {
                if(_Claims.Any(x => x.Type == "Rid"))
                {
                    return int.Parse(_Claims.First(x => x.Type == "Rid").Value);
                }

                return 0;
            }
        }

        public string UserName
        {
            get
            {
                if(_Claims.Any(x => x.Type == "nameid"))
                {
                    return _Claims.First(x => x.Type == "nameid").Value;
                }

                return null;
            }
        }

        public string FullName
        {
            get
            {
                if(_Claims.Any(x => x.Type == "given_name"))
                {
                    return _Claims.First(x => x.Type == "given_name").Value;
                }

                return null;
            }
        }

        public string RoleName
        {
            get
            {
                if(_Claims.Any(x => x.Type == "role"))
                {
                    return _Claims.First(x => x.Type == "role").Value;
                }

                return null;
            }
        }

        public string Email
        {
            get
            {
                if(_Claims.Any(x => x.Type == "email"))
                {
                    return _Claims.First(x => x.Type == "email").Value;
                }

                return null;
            }
        }

        public UserProfile(IHttpContextAccessor httpContextAccessor, IServiceProvider serviceLocator)
        {
            IHttpContextAccessor context;
            if (httpContextAccessor.HttpContext.RequestServices != null)
            {
                context = (IHttpContextAccessor)httpContextAccessor.HttpContext.RequestServices.GetService(typeof(IHttpContextAccessor));
            }
            else  //Test Mode
            {
                context = new HttpContextAccessor()
                {
                    HttpContext = httpContextAccessor.HttpContext
                };
            }

            this._Claims = context.HttpContext.User.Claims;
            this._ServiceLocator = serviceLocator;
        }

        public async Task<bool> HasPermissionAsync(string permissionName)
        {
            //TODO: Call the engine that validate the permission here

            return true;
        }

        private IAuthorizationEngine GetAuthorizationEngine()
        {
            if (_AuthorizationEngine == null)
                _AuthorizationEngine = (IAuthorizationEngine)this._ServiceLocator.GetService(typeof(IAuthorizationEngine));

            return _AuthorizationEngine;
        }
    }
}
