using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechDrum.Contract.Service;
using TechDrum.Core.Constants;

namespace TechDrum.Web.Areas.Portal.Controllers
{
    public class MarketingController : BaseController
    {
        public MarketingController(IServiceProvider serviceProvider, ISDKSsoCommonService iSDKSsoCommonService) : base(serviceProvider, iSDKSsoCommonService)
        {
        }

        [Route(PortalEndpoint.Marketing.AffiliateMarketingEndpoint)]
        public IActionResult Index()
        {
            return View();
        }
        [Route(PortalEndpoint.Marketing.BuyInsuranceEndpoint)]
        [Authorize]
        public IActionResult BuyInsurance()
        {
            return View();
        }
        [Route(PortalEndpoint.Marketing.InsuranceHistoryEndpoint)]
        [Authorize]
        public IActionResult InsuranceHistory()
        {
            return View();
        }
        [Route(PortalEndpoint.Marketing.ClaimInsuranceEndpoint)]
        [Authorize]
        public IActionResult ClaimInsurance()
        {
            return View();
        }
        [Route(PortalEndpoint.Marketing.ClaimHistoryEndpoint)]
        [Authorize]
        public IActionResult ClaimHistory()
        {
            return View();
        }

    }
}
