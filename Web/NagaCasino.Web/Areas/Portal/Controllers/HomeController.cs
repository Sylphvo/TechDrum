using System;
using Invedia.Core.EnvUtils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechDrum.Contract.Service;
using TechDrum.Core.Constants;
using TechDrum.Web.Utils.Notification;
using TechDrum.Web.Utils.Notification.Models.Constants;

namespace TechDrum.Web.Areas.Portal.Controllers
{
    public class HomeController : BaseController
    {

        public HomeController(IServiceProvider serviceProvider, ISDKSsoCommonService iSDKSsoCommonService) : base(serviceProvider, iSDKSsoCommonService)
        {
        }

        public IActionResult Index(string status, string messages, NotificationStatus type)
        {
            if (string.IsNullOrEmpty(status) || string.IsNullOrEmpty(messages))
            {
                return View();
            }
            this.SetNotification(status, messages, type);
            return View();
        }
        [AllowAnonymous]
        [Route(PortalEndpoint.Home.OopsWithParamEndpoint)]
        public IActionResult Oops(int statusCode)
        {
            if (EnvHelper.IsDevelopment())
            {
                var message = $"Welcome TechDrum";

                this.SetNotification(Messages.Common.SomethingWentWrong, message, NotificationStatus.Success);
            }

            if (statusCode == StatusCodes.Status401Unauthorized) return RedirectToAction("SignIn", "Auth");

            if (statusCode == StatusCodes.Status403Forbidden) return View("UnAuthorization", "Home");

            if (statusCode == StatusCodes.Status404NotFound) return View("NotFound", "Home");
            if (statusCode == StatusCodes.Status429TooManyRequests) {
                return RedirectToAction("Index", "Home",
                        new { status = "Fail", messages = "Please try in 60 seconds !", type = NotificationStatus.Error });
            } 

            return View("Oops", "Home");
        }

    }
}