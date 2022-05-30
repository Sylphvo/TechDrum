using Microsoft.AspNetCore.Mvc;
using TechDrum.Contract.Service;
using TechDrum.Core.Constants;
using System;

namespace TechDrum.Web.Areas.Portal.Controllers
{
    public class ShopController : BaseController
    {
        public ShopController(IServiceProvider serviceProvider, ISDKSsoCommonService iSDKSsoCommonSerivce) : base(serviceProvider, iSDKSsoCommonSerivce)
        {
        }

        [Route(PortalEndpoint.Shop.ShopEndpoint)]
        public IActionResult Shop()
        {
            return View();
        }
    }
}
