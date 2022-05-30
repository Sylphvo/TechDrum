using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using TechDrum.Contract.Repository.Models.User;
using TechDrum.Contract.Service;
using TechDrum.Core.Constants;
using TechDrum.Core.Models.AuthenSso;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace TechDrum.Web.Areas.Portal.Controllers
{
    [Area(PortalEndpoint.AreaName)]
    public class BaseController : Controller
    {
        protected readonly UserManager<UserEntity> UserManager;
        private readonly ISDKSsoCommonService _iSDKSsoCommonService;
        public BaseController(IServiceProvider serviceProvider, ISDKSsoCommonService iSDKSsoCommonSerivce)
        {
            UserManager = serviceProvider.GetRequiredService<UserManager<UserEntity>>();
            _iSDKSsoCommonService = iSDKSsoCommonSerivce;
        }

        public void SetCookie(string key, string value)
        {
            CookieOptions option = new CookieOptions { Expires = DateTimeOffset.UtcNow.AddHours(24) };
            Response.Cookies.Append(key, value, option);
        }
        protected async Task<bool> VerifyTwoFactorTokenAsync(UserEntity user, string twoFactorCode)
        {
            // Strip spaces and hypens
            var verificationCode = twoFactorCode.Replace(" ", string.Empty).Replace("-", string.Empty);

            var is2FaTokenValid = await _iSDKSsoCommonService.SDKSSO_VerifycodeAsync(user.UserName, verificationCode );

            return is2FaTokenValid;
        }
        protected void RefreshToken(string jwtToken, string refreshToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwtToken);
            var jti = token.Payload;
            //handler.TokenLifetimeInMinutes = (int)TimeSpan.FromMinutes(Convert.ToDouble(jti.Exp)).Minutes;
            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds((int)jti.Exp);
            var checkLifeTime = DateTimeOffset.UtcNow - dateTimeOffset;

            if (checkLifeTime.Minutes >= 0)
            {
                var result = _iSDKSsoCommonService.SDKSSO_RefreshToken(new { token = refreshToken }, jwtToken);
                if (!string.IsNullOrEmpty(result))
                {
                    HttpContext.Session.Clear();
                    AuthenticateModel authenModel = JsonConvert.DeserializeObject<AuthenticateModel>(result);
                    HttpContext.Session.SetString("JWToken", authenModel.jwtToken);
                    HttpContext.Session.SetString("refreshToken", authenModel.refreshToken);
                    Response.Cookies.Delete("JWToken");
                    SetCookie("JWToken", authenModel.jwtToken);
                }
            }
            Response.Cookies.Delete("2fa");
            SetCookie("2fa", jti["2fa"].ToString());
        }
    }
}