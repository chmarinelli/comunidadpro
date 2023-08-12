using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using MiComunidadPro.Business.Contracts;
using MiComunidadPro.Business.Engines.Contracts;
using MiComunidadPro.Business.Entities.DBO;
using MiComunidadPro.Business.Entities.Dtos;
using MiComunidadPro.Business.Entities.Enums.Status;
using MiComunidadPro.Business.Entities.IDENTITY;
using MiComunidadPro.Common.Contracts;
using MiComunidadPro.Web.Api.Models;
using MiComunidadPro.Web.Infrastructure;
using MiComunidadPro.Web.Infrastructure.Constants;
using MiComunidadPro.Web.Infrastructure.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace MiComunidadPro.Web.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ApiControllerBase
    {
        private readonly IDataRepositoryFactory _DataRepositoryFactory;
        private readonly IMessageHandler _MessageHandler;
        private readonly SignInManager<User> _SignInManager;
        private readonly LockoutOptions _lockoutOptions;
        private readonly LocalJwtSettings _LocalJwtSettings;
        protected readonly UserManager<User> _UserManager;
        private readonly IBusinessEngineFactory _BusinessEngineFactory;
        private readonly IUserProfile _UserProfile;
        public AuthController(IDataRepositoryFactory dataRepositoryFactory, IMessageHandler messageHandler, 
                                SignInManager<User> signInManager, IOptions<LockoutOptions> options, 
                                LocalJwtSettings localJwtSettings, UserManager<User> userManager, IBusinessEngineFactory businessEngineFactory, 
                                IUserProfile userProfile) 
        {
            _DataRepositoryFactory = dataRepositoryFactory;
            _MessageHandler = messageHandler;
            _SignInManager = signInManager;
            _lockoutOptions = options.Value;
            _LocalJwtSettings = localJwtSettings;
            _UserManager = userManager;
            _BusinessEngineFactory = businessEngineFactory;
            _UserProfile = userProfile;
        }

        private string GenerateUserToken(AuthenticatedUserDto user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_LocalJwtSettings.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Sid, user.Id.ToString()),
                    new Claim("Rid", user.RoleId.ToString()),
                    new Claim(ClaimTypes.NameIdentifier, user.UserName),
                    new Claim(ClaimTypes.GivenName, user.FullName),
                    new Claim(ClaimTypes.Role, user.RoleName),
                    new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                    new Claim("scope", ScopeName.WebApi),
                }),
                IssuedAt = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddMinutes(_LocalJwtSettings.ExpiresOn),
                SigningCredentials = credentials,
                Issuer = _LocalJwtSettings.Issuer,
                Audience = _LocalJwtSettings.Audience
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> LoginAsync([FromBody] LoginViewModel model)
        {
            var userRepository = _DataRepositoryFactory.Get<User>();
            
            var appUser = await _UserManager.FindByNameAsync(model.UserName);

            if (appUser is null)
                throw new ArgumentException(_MessageHandler.GetMessage("Error00001").Name);
            
            if (appUser.Status == UserStatus.Locked) 
                throw new ArgumentException(_MessageHandler.GetMessage("Error00002").Name);

            var signInResult = await _SignInManager.PasswordSignInAsync(model.UserName, model.Password, isPersistent: false, lockoutOnFailure: true);
            if (signInResult.IsLockedOut)
                throw new ArgumentException(string.Format(_MessageHandler.GetMessage("Error00003").Name,  _lockoutOptions.DefaultLockoutTimeSpan.Minutes));
            else if (!signInResult.Succeeded) throw new ArgumentException(_MessageHandler.GetMessage("Error00001").Name);

            var userEngine = _BusinessEngineFactory.Get<IUserEngine>();
            var role = await userEngine.GetRoleAsync(appUser.Id);

            var user = new AuthenticatedUserDto 
            {
                Id = appUser.Id,
                UserName = appUser.UserName,
                FullName = appUser.FullName,
                Email = appUser.Email,
                RoleId = (int)role.Id,
                RoleName = role.Name    
            };

            var token = GenerateUserToken(user);
            
            var locations = await userEngine.GetLocationsAsync(appUser.Id);

            var permissions = await userEngine.GetAllPermissionsAsync(appUser.Id, (int)role.Id);

            var payload = new
            {
                Access_token = token,
                Expires_in = _LocalJwtSettings.ExpiresOn,
                user,
                locations,
                permissions
            };

            if (appUser.LastLogin.HasValue)
            {
                appUser.LastLogin = DateTime.Now;
                await userRepository.UpdateAsync(appUser);
            }

            return Ok(payload);
        }

        [HttpGet]
        [Route("me")]
        public async Task<ActionResult> MeAsync()
        {
            var userId = _UserProfile.UserId;

            var userEngine = _BusinessEngineFactory.Get<IUserEngine>();

            var locations = await userEngine.GetLocationsAsync(userId);

            var permissions = await userEngine.GetAllPermissionsAsync(userId, _UserProfile.RoleId);

            var payload = new
            {
                user = new AuthenticatedUserDto 
                {
                    Id = _UserProfile.UserId,
                    UserName = _UserProfile.UserName,
                    FullName = _UserProfile.FullName,
                    Email = _UserProfile.Email,
                    RoleId = _UserProfile.RoleId,
                    RoleName = _UserProfile.RoleName    
                },
                locations,
                permissions
            };

            return Ok(payload);
        }
    }
}