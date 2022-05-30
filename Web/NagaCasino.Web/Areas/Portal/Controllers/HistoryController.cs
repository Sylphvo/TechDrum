using Invedia.Core.EnvUtils;
using Invedia.Web.HttpUtils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using TechDrum.Contract.Repository.Models.User;
using TechDrum.Contract.Service;
using TechDrum.Core.Attributes;
using TechDrum.Core.Commons;
using TechDrum.Core.Constants;
using TechDrum.Core.Models.History;
using TechDrum.Core.Models.MessageResend;
using TechDrum.Core.Models.Portal;
using TechDrum.Core.Models.User;
using TechDrum.Core.Utils;
using TechDrum.Web.Utils.Notification;
using TechDrum.Web.Utils.Notification.Models.Constants;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace TechDrum.Web.Areas.Portal.Controllers
{
    public class HistoryController : BaseController
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly ISDKPortalCommonService _iSDKPortalCommonService;

        public HistoryController(IServiceProvider serviceProvider, ISDKPortalCommonService iSDKPortalCommonService,
            ISDKSsoCommonService iSDKSsoCommonService) :
            base(serviceProvider, iSDKSsoCommonService)
        {
            _userManager = serviceProvider.GetRequiredService<UserManager<UserEntity>>();
            _iSDKPortalCommonService = iSDKPortalCommonService;
        }
        [Authorize]
        [Route(PortalEndpoint.History.PolicyEndpoint)]
        public IActionResult Policy()
        {

            return View();
        }
        [Authorize]
        [SessionLifeTime]
        [Route(PortalEndpoint.History.CommissionEndpoint)]
        public IActionResult Commission(string sortOrder,
                                string currentFilter,
                                string searchString, int? pageNumber)
        {
            RefreshToken(HttpContext.Session.GetString("JWToken"), HttpContext.Session.GetString("refreshToken"));

            int pageIndex = 0;
            int pageSize = 500;
            string type = "commission";
            HistoryTransactionModel dataoption = new HistoryTransactionModel()
            {
                username = User.Identity.Name,
                searchString = "",
                pageIndex = pageIndex,
                pageSize = pageSize,
                type = type
            };
            var result = _iSDKPortalCommonService.SDKPortal_HistoryUser(type, dataoption, HttpContext.Session.GetString("JWToken"));
            if (result == null) return null;
            ViewBag.CookieValue = Request.Cookies["Roles"];
            if (result == null) return null;
            var myDeserializedClass = JsonConvert.DeserializeObject<Root>(result);
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            var students = from s in myDeserializedClass.data
                           select s;
            switch (sortOrder)
            {
                case "date_desc":
                    students = students.OrderBy(p => p.Time);
                    break;
                case "Date":
                    students = students.OrderByDescending(p => p.Time);
                    break;
                default:
                    students = students.OrderByDescending(p => p.Time);
                    break;
            }
            int pageRecord = 10;
            return View(PaginatedList<UserHistoryModel>.Create(students, pageNumber ?? 1, pageRecord));
        }

        [Authorize]
        [SessionLifeTime]
        [Route(PortalEndpoint.History.ETHEndpoint)]
        public IActionResult ETH(string sortOrder,
                                string currentFilter,
                                string searchString, int? pageNumber)
        {
            RefreshToken(HttpContext.Session.GetString("JWToken"), HttpContext.Session.GetString("refreshToken"));
            ViewBag.CookieValue = Request.Cookies["Roles"];

            int pageIndex = 0;
            int pageSize = 500;
            string type = "ETH";
            HistoryTransactionModel dataoption = new HistoryTransactionModel()
            {
                username = User.Identity.Name,
                searchString = "",
                pageIndex = pageIndex,
                pageSize = pageSize,
                type = type
            };
            var result = _iSDKPortalCommonService.SDKPortal_HistoryUser(type, dataoption, HttpContext.Session.GetString("JWToken"));
            if (result == null) return null;
            var myDeserializedClass = JsonConvert.DeserializeObject<Root>(result);
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            var students = from s in myDeserializedClass.data
                           select s;
            switch (sortOrder)
            {
                case "date_desc":
                    students = students.OrderBy(p => p.Time);
                    break;
                case "Date":
                    students = students.OrderByDescending(p => p.Time);
                    break;
                default:
                    students = students.OrderByDescending(p => p.Time);
                    break;
            }
            int pageRecord = 10;
            return View(PaginatedList<UserHistoryModel>.Create(students, pageNumber ?? 1, pageRecord));
        }

        [Authorize]
        [SessionLifeTime]
        [Route(PortalEndpoint.History.NAGAEndpoint)]
        public IActionResult? NAGA(string sortOrder,
                                string currentFilter,
                                string searchString, int? pageNumber)
        {
            RefreshToken(HttpContext.Session.GetString("JWToken"), HttpContext.Session.GetString("refreshToken"));
            ViewBag.CookieValue = Request.Cookies["Roles"];

            int pageIndex = 0;
            int pageSize = 500;
            string type = "NAGA";
            HistoryTransactionModel dataoption = new HistoryTransactionModel()
            {
                username = User.Identity.Name,
                searchString = "",
                pageIndex = pageIndex,
                pageSize = pageSize,
                type = type
            };
            var result = _iSDKPortalCommonService.SDKPortal_HistoryUser(type, dataoption, HttpContext.Session.GetString("JWToken"));
            if (result == null) return null;
            var myDeserializedClass = JsonConvert.DeserializeObject<Root>(result);
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            var students = from s in myDeserializedClass.data
                           select s;
            switch (sortOrder)
            {
                case "date_desc":
                    students = students.OrderBy(p=>p.Time);
                    break;
                case "Date":
                    students = students.OrderByDescending(p => p.Time);
                    break;
                default:
                    students = students.OrderByDescending(p => p.Time);
                    break;
            }
            int pageRecord = 10;
            return View(PaginatedList<UserHistoryModel>.Create(students, pageNumber ?? 1, pageRecord));
        }
        [Authorize]
        [SessionLifeTime]
        [Route(PortalEndpoint.History.NAGAInsEndpoint)]
        public IActionResult? NAGAIns(string sortOrder,
                                string currentFilter,
                                string searchString, int? pageNumber)
        {
            RefreshToken(HttpContext.Session.GetString("JWToken"), HttpContext.Session.GetString("refreshToken"));
            ViewBag.CookieValue = Request.Cookies["Roles"];

            int pageIndex = 0;
            int pageSize = 500;
            string type = "ins";
            HistoryTransactionModel dataoption = new HistoryTransactionModel()
            {
                username = User.Identity.Name,
                searchString = "",
                pageIndex = pageIndex,
                pageSize = pageSize,
                type = type
            };
            var result = _iSDKPortalCommonService.SDKPortal_HistoryUserIns(type, dataoption, HttpContext.Session.GetString("JWToken"));
            if (result == null) return null;
            var myDeserializedClass = JsonConvert.DeserializeObject<Root>(result);
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            var students = from s in myDeserializedClass.data
                           select s;
            switch (sortOrder)
            {
                case "date_desc":
                    students = students.OrderBy(p => p.Time);
                    break;
                case "Date":
                    students = students.OrderByDescending(p => p.Time);
                    break;
                default:
                    students = students.OrderByDescending(p => p.Time);
                    break;
            }
            int pageRecord = 10;
            return View(PaginatedList<UserHistoryModel>.Create(students, pageNumber ?? 1, pageRecord));
        }
        //[RequestRateLimit(Name = "re-send-email", Seconds = 60)]
        [HttpPost]
        public string ResendMailTransaction(string transactionId)
        {
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Formatting = Formatting.Indented
            };
            var model = new TransactionResendlModel
            {
                id = transactionId
            };
            var result = _iSDKPortalCommonService.SDKPortal_ResendTransaction(model, HttpContext.Session.GetString("JWToken"));
            MessageResendMailModel modelMessage = new MessageResendMailModel();
            if (result.IsSuccessful)
            {
                modelMessage.message = "Resend mail success. Please check mail!";
                return JsonConvert.SerializeObject(modelMessage, settings);
            }

            modelMessage.message = "Resend mail failed!";
            return JsonConvert.SerializeObject(modelMessage, settings);
        }
        [HttpPost]
        [SessionLifeTime]
        public string CancelHistory(string TransId)
        {
            RefreshToken(HttpContext.Session.GetString("JWToken"), HttpContext.Session.GetString("refreshToken"));

            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Formatting = Formatting.Indented
            };
            var model = new CancelModel
            {
                TransId = TransId
            };
            var result = _iSDKPortalCommonService.SDKPortal_CancelTransaction(model, HttpContext.Session.GetString("JWToken"));
            MessageResendMailModel modelMessage = new MessageResendMailModel();
            if (result.IsSuccessful)
            {
                modelMessage.message = "Cancel Success!";
                return JsonConvert.SerializeObject(modelMessage, settings);
            }
            modelMessage.message = "Cancel Failed!";
            return JsonConvert.SerializeObject(modelMessage, settings);
        }

        //[HttpGet]
        //[Route(PortalEndpoint.History.CancelEndpoint)]
        //public async Task<IActionResult> Cancel(string id)
        //{
        //    return PartialView(new CancelViewModel { Id = id, Amount = 10 });
        //    //var baseUrl = FundistHelper.ClientConfigs.BaseUrlPortal;
        //    //var client = new RestClient(baseUrl);
        //    //var request = new RestRequest($"api/transaction/get-by-id/{id}", Method.GET);
        //    //var response = await client.ExecuteAsync<CancelViewModel>(request);
        //    //if (response.IsSuccessful)
        //    //{
        //    //    return PartialView(response.Data);
        //    //}
        //    //return RedirectToAction("NAGA");
        //}

        //[HttpPost]
        //[Route(PortalEndpoint.History.CancelEndpoint)]
        //public async Task<IActionResult> Cancel(CancelViewModel model)
        //{
        //    var body = new CancelModel
        //    {
        //        TransId = model.Id
        //    };

        //    var baseUrl = FundistHelper.ClientConfigs.BaseUrlPortal;
        //    var client = new RestClient(baseUrl);

        //    var request = new RestRequest("api/transaction/cancel", Method.POST)
        //        .AddJsonBody(body);

        //    var response = await client.ExecuteAsync(request);

        //    switch (model.Type.ToLower())
        //    {
        //        case "naga+":
        //        case "naga-":
        //            return RedirectToAction("NAGA");
        //        case "eth":
        //        case "deposit":
        //            return RedirectToAction("ETH");
        //        case "commission":
        //            return RedirectToAction("Commission");
        //    }

        //    return RedirectToAction("AccountDashboard", "AccountDashboard");
        //}

        //[Authorize]
        //[Route(PortalEndpoint.History.CommissionEndpoint)]
        //public IActionResult VerifyHistory()
        //{
        //    return View();
        //}

        [HttpPost]
        [SessionLifeTime]
        public string SubmitVerifyHistory(string transId, string code, string type)
        {
            RefreshToken(HttpContext.Session.GetString("JWToken"), HttpContext.Session.GetString("refreshToken"));

            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Formatting = Formatting.Indented
            };
            var ip = CoreHelper.CurrentHttpContext?.Request.GetIpAddress();
            var model = new VerifylModel
            {
                ip = ip,
                transId = transId,
                token = code
            };
            var result = _iSDKPortalCommonService.SDKPortal_VerifyTransaction(model, type, HttpContext.Session.GetString("JWToken"));
            MessageResendMailModel modelMessage = new MessageResendMailModel();
            if (result.IsSuccessful)
            {
                modelMessage.data = 1;
                modelMessage.message = "Verify success!";
                return JsonConvert.SerializeObject(modelMessage, settings);
            }
            modelMessage.data = 0;
            modelMessage.message = "Verify failed!";
            return JsonConvert.SerializeObject(modelMessage, settings);
        }
    }
}