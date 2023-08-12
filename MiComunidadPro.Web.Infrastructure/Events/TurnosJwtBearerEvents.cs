using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MiComunidadPro.Web.Infrastructure.Events
{
    public class ApplicationJwtBearerEvents : JwtBearerEvents
    {
        public async override Task TokenValidated(TokenValidatedContext context)
        {
            var identity = context.Principal.Identity as ClaimsIdentity;

            var userNameClaim = identity.Claims.FirstOrDefault(z => z.Type == "name");

            if (userNameClaim == null)
                return;

            // IAccountEngine accountEngine = null;


            // if(accountEngine == null)
            //     accountEngine = context.HttpContext.RequestServices.GetService<IAccountEngine>();

            // var cacheService = context.HttpContext.RequestServices.GetService<ICacheService>();

            // var userData = await cacheService.GetCachedAsync($"{userNameClaim.Value}_Profile", async () =>
            // {
            //     var user = await accountEngine.FindUserDataAsync(userNameClaim.Value);

            //     return user;
            // }, TimeSpan.FromMinutes(60));

            var claims = new List<Claim>();

            // claims.Add(new Claim("givenname", $"{userData.FirstName} {userData.LastPaternalName ?? string.Empty} {userData.LastMaternalName ?? string.Empty}".Trim()));

            // claims.Add(new Claim("firstname", userData.FirstName));

            // if (!string.IsNullOrEmpty(userData.LastPaternalName))
            //     claims.Add(new Claim("lastpaternalname", userData.LastPaternalName));

            // if (!string.IsNullOrEmpty(userData.LastMaternalName))
            //     claims.Add(new Claim("lastmaternalname", userData.LastMaternalName));

            // if(userData.CarrierId.HasValue && userData.CarrierId.Value > 0)
            // {
            //     claims.Add(new Claim("phone", userData.Phone));
            //     claims.Add(new Claim("carrierid", userData.CarrierId.ToString()));
            // }

            // if(userData.Gender.HasValue)
            // {
            //     claims.Add(new Claim("gender", userData.Gender.ToString()));
            // }

            // if(userData.Ethnicity.HasValue)
            // {
            //     claims.Add(new Claim("ethnicity", userData.Ethnicity.ToString()));
            // }

            // if(userData.BirthDate.HasValue)
            //     claims.Add(new Claim("birthdate", userData.BirthDate.ToString()));

            // if(!string.IsNullOrEmpty(userData.ZipCode))
            //     claims.Add(new Claim("zipcode", userData.ZipCode));

            // claims.Add(new Claim("isapproved", userData.IsApproved ? "1" : "0"));
            // claims.Add(new Claim("islockedout", userData.IsLockedOut ? "1" : "0"));

            // if(userData.LastLoginDate.HasValue)
            //     claims.Add(new Claim("lastlogindate", userData.LastLoginDate.Value.ToString("dd/MMM/yyyy hh:mmtt")));

            // foreach (var item in userData.Locations)
            // {
            //     claims.Add(new Claim("location", item.Key.ToString()));
            //     claims.Add(new Claim("locationdesc", item.Value));
            // }

            // foreach(var item in userData.LocationConfigurations)
            // {
            //     claims.Add(new Claim("locationconfiguration", item.Key.ToString()));
            // }

            // foreach (var item in userData.Organizations)
            // {
            //     claims.Add(new Claim("organization", item.Key.ToString()));
            //     claims.Add(new Claim("organizationdesc", item.Value));
            // }

            // foreach (var item in userData.Companies)
            // {
            //     claims.Add(new Claim("company", item.Key.ToString()));
            //     claims.Add(new Claim("companydesc", item.Value));
            // }

            // foreach(var item in userData.UserLocationsAlerts)
            // {
            //     claims.Add(new Claim("userlocationsalert", item.Key.ToString()));
            //     claims.Add(new Claim("userlocationsalertdesc", item.Value));
            // }

            // add claims to the identity
            identity.AddClaims(claims);
        }
    }
}
