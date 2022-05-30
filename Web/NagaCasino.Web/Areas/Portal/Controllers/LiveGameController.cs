using Invedia.Web.HttpUtils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using TechDrum.Contract.Service;
using TechDrum.Core.Attributes;
using TechDrum.Core.Constants;
using TechDrum.Core.Models.Game;
using TechDrum.Core.Utils;
using TechDrum.Service;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace TechDrum.Web.Areas.Portal.Controllers
{
    public class LiveGameController : BaseController
    {
        private readonly ISDKFundistCommonService _iSDKFundistCommonService;

        public LiveGameController(IServiceProvider serviceProvider, ISDKFundistCommonService iSDKFundistCommonService,
            ISDKSsoCommonService iSDKSsoCommonService) : base(serviceProvider, iSDKSsoCommonService)
        {
            _iSDKFundistCommonService = iSDKFundistCommonService;
        }

        [HttpGet]
        [SessionLifeTime]
        [Route(PortalEndpoint.LiveGame.SelectGameEndpoint)]
        public IActionResult SelectGame()
        {
            ViewBag.CookieValue = Request.Cookies["Roles"];
            var obj = _iSDKFundistCommonService.SDKFund_FullList(TimeStampHelper.GetTimeStamp());
            //JObject jsObject = JObject.Parse(obj);
            //var game = jsObject["games"];
            //var res = JsonConvert.SerializeObject(game);
            var myDeserializedClass = JsonConvert.DeserializeObject<Root>(obj);
            return View(myDeserializedClass);
        }

        [Authorize]
        [HttpGet]
        [SessionLifeTime]
        [Route(PortalEndpoint.LiveGame.LiveGameEndpoint)]
        public async Task<IActionResult> LiveGame(string ID, string System)
        {
            ViewBag.CookieValue = Request.Cookies["Roles"];
            var user = await UserManager.FindByNameAsync(User.Identity?.Name);
            var ip = CoreHelper.CurrentHttpContext?.Request.GetIpAddress();
            if (ip == "127.0.0.1")
            {
                ip = "115.78.236.20";
            }

            var result = _iSDKFundistCommonService.SDKFund_Authorization(User.Identity.Name, TimeStampHelper.GetTimeStamp(),
                user.PasswordNotHash, ip, ID, System);
            ViewBag.DirectUrl = result.Split(',')[1];
            var result2 = _iSDKFundistCommonService.SDKFund_Authorization_Html(User.Identity.Name,
                TimeStampHelper.GetTimeStamp(), user.PasswordNotHash, ip, ID, System);
            if (result.Split(',')[0].Equals("1"))
            {
                ViewBag.DirectUrl = result.Split(',')[1];
            }
            else if (result2.Split(',')[0].Equals("1"))
            {
                ViewBag.HTMLUrl = result2.Split(',')[1];
            }

            return View();
        }
    }
}