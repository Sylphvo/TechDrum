using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechDrum.Contract.Service;
using TechDrum.Core.Attributes;
using TechDrum.Core.Commons;
using TechDrum.Core.Constants;
using TechDrum.Core.Models.User;
using TechDrum.Core.Utils;
using Newtonsoft.Json.Linq;

namespace TechDrum.Web.Areas.Portal.Controllers
{
    [Authorize]
    public class AffiliatesController : BaseController
    {
        private readonly ISDKPortalCommonService _iSDKPortalCommonService;

        public AffiliatesController(IServiceProvider serviceProvider, ISDKPortalCommonService iSDKPortalCommonService,
            ISDKSsoCommonService iSDKSsoCommonService) : base(serviceProvider, iSDKSsoCommonService)
        {
            _iSDKPortalCommonService = iSDKPortalCommonService;
        }

        [HttpGet]
        [SessionLifeTime]
        [Route(PortalEndpoint.Affiliates.AffiliatesEndpoint)]
        public IActionResult Affiliates()
        {
            RefreshToken(HttpContext.Session.GetString("JWToken"), HttpContext.Session.GetString("refreshToken"));
            var result = _iSDKPortalCommonService.SDKPortal_AffiliateTree(new UserPortalModel { username = User.Identity?.Name }, HttpContext.Session.GetString("JWToken"));
            if (result == null) return View(model: "[]");
            JObject JsonResult = JObject.Parse(result);
            var data = JsonResult["data"];
            ViewBag.LinkAffiliate = FundistHelper.ClientConfigs.BaseUrlRegist + "/signup/" + User.Identity.Name;
            ViewBag.CookieValue = Request.Cookies["Roles"];

            if (data == null) return View(model: JsonResult["[]"].ToString());
            return View(model: JsonResult["data"].ToString());
        }


  
    }
}
