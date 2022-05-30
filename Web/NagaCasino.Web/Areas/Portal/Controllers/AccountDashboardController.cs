using Invedia.Web.HttpUtils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using TechDrum.Contract.Repository.Models.Transaction;
using TechDrum.Contract.Service;
using TechDrum.Core.Attributes;
using TechDrum.Core.Constants;
using TechDrum.Core.Models.MessageResend;
using TechDrum.Core.Models.User;
using TechDrum.Core.Utils;
using TechDrum.Service;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Threading.Tasks;

namespace TechDrum.Web.Areas.Portal.Controllers
{
    public class AccountDashboardController : BaseController
    {
        private readonly ITransactionService _transactionService;
        private readonly ISDKPortalCommonService _iSDKPortalCommonService;

        public AccountDashboardController(IServiceProvider serviceProvider, ISDKPortalCommonService iSDKPortalCommonService,
            ISDKSsoCommonService iSDKSsoCommonService) : base(serviceProvider, iSDKSsoCommonService)
        {
            _transactionService = serviceProvider.GetRequiredService<ITransactionService>();
            _iSDKPortalCommonService = iSDKPortalCommonService;
        }

        [Route(PortalEndpoint.AccountDashboard.AccountDashboardEndpoint)]
        public IActionResult AccountDashboard()
        {
            return View();
        }

        [HttpPost]
        [SessionLifeTime]
        public async Task<string> WithdrawTransaction(string type, string username, string address, double amount, string code)
        {
            RefreshToken(HttpContext.Session.GetString("JWToken"), HttpContext.Session.GetString("refreshToken"));
            var entity = await UserManager.GetUserAsync(User);
            //if (entity.LastCheckTime != null)
            //{
            //    var checklasttime = DateTimeOffset.UtcNow - entity.LastCheckTime;
            //    //DateTimeOffset doffset = (DateTimeOffset)usernamedb.LastCheckTime;
            //    //TimeSpan span = doffset.Subtract(DateTimeOffset.UtcNow);
            //    var checksecond = checklasttime.GetValueOrDefault().TotalSeconds;
            //    if (checksecond < 60)
            //    {
            //        return JsonConvert.SerializeObject("Please wait make again transaction after " +
            //                                           ((int) checksecond) + "sec");
            //    }
            //}
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Formatting = Formatting.Indented
            };
            //var getId = await UserManager.FindByNameAsync(username);
            var ip = CoreHelper.CurrentHttpContext?.Request.GetIpAddress();
            if (ip == "127.0.0.1")
            {
                ip = "20.49.104.14";
            }

            var withdraw = new WithdrawModel
            {
                username = username,
                timeStamp = TimeStampHelper.GetTimeStamp(),
                amount = Convert.ToDecimal(amount),
                addressTo = address,
                ip = ip, 
                code = code
            };
            MessageResendMailModel resend = new MessageResendMailModel();
            IRestResponse result;
            var transactionEntity = new TransactionEnity();
            if (type == "ETH") //Withdraw
            {
                transactionEntity.Token = withdraw.token;
                transactionEntity.TimeStamp = withdraw.timeStamp;
                transactionEntity.TypeTransaction = "Withdraw ETH";
                transactionEntity.Temp = $"{withdraw.amount}";
                transactionEntity.Temp1 = withdraw.addressTo;
                transactionEntity.IpRequest = withdraw.ip;
                transactionEntity.UserId = withdraw.userId;
                transactionEntity.ExpirTime = DateTimeOffset.UtcNow.AddMinutes(30);
                transactionEntity.Status = "New";
                _transactionService.Create(transactionEntity);
                result = _iSDKPortalCommonService.SDKPortal_WithdrawTransaction(type, withdraw, HttpContext.Session.GetString("JWToken"));
                if (result.IsSuccessful)
                {
                    transactionEntity = _transactionService.GetTransactionByToken(withdraw.timeStamp);
                    transactionEntity.Token = withdraw.token;
                    transactionEntity.Status = result.StatusCode.ToString();
                    transactionEntity.Request = result.Request.Body.Value.ToString();
                    transactionEntity.UrlSent = result.Request.Resource;
                    transactionEntity.ResponseData = result.Content;
                    _transactionService.Update(transactionEntity);

                    entity.LastCheckTime = DateTimeOffset.UtcNow;
                    await UserManager.UpdateAsync(entity);
                    resend.data = 1;
                    resend.message = "Withdraw accepted !";
                    return JsonConvert.SerializeObject(resend, settings);
                }
                if ((int)result.StatusCode == 400)
                {
                    var messageFailed = JsonConvert.DeserializeObject<MessageResendTxhashModel>(result.Content);
                    resend.data = 2;
                    resend.message = messageFailed.message;
                    return JsonConvert.SerializeObject(resend, settings);
                }
                resend.data = 1;
                resend.message = (int)result.StatusCode == 429
                        ? "Please wait make again transaction after 60 sec"
                        : "Withdraw failed. Please contact supporter !";
                return JsonConvert.SerializeObject(resend
                    , settings);
            }
            else
            {
                withdraw.userId = entity.Id;
                withdraw.blockchain = type;
                var checkUser = await UserManager.FindByNameAsync(address);
                if (checkUser != null)
                {
                    transactionEntity.Token = withdraw.token;
                    transactionEntity.TimeStamp = withdraw.timeStamp;
                    transactionEntity.TypeTransaction = "Transfer";
                    transactionEntity.Temp = $"{withdraw.amount}";
                    transactionEntity.Temp1 = withdraw.addressTo;
                    transactionEntity.IpRequest = withdraw.ip;
                    transactionEntity.UserId = withdraw.userId;
                    transactionEntity.ExpirTime = DateTimeOffset.UtcNow.AddHours(1);
                    transactionEntity.Status = "New";
                    _transactionService.Create(transactionEntity);
                    result = _iSDKPortalCommonService.SDKPortal_WithdrawTransaction(type, withdraw, HttpContext.Session.GetString("JWToken"));
                }
                else
                {
                    resend.data = 2;
                    resend.message = "User name not exist system !";
                    return JsonConvert.SerializeObject(resend, settings);
                }

                if (result.IsSuccessful)
                {
                    transactionEntity = _transactionService.GetTransactionByToken(withdraw.timeStamp);
                    transactionEntity.Token = withdraw.token;
                    transactionEntity.Status = result.StatusCode.ToString();
                    transactionEntity.Request = result.Request.Body.Value.ToString();
                    transactionEntity.UrlSent = result.Request.Resource;
                    transactionEntity.ResponseData = result.Content;
                    _transactionService.Update(transactionEntity);

                    entity.LastCheckTime = DateTimeOffset.UtcNow;
                    await UserManager.UpdateAsync(entity);
                    resend.data = 1;
                    resend.message = "Transfer accepted !";
                    return JsonConvert.SerializeObject(resend, settings);
                }
                if((int)result.StatusCode == 400)
                {
                    var messageFailedTransfer = JsonConvert.DeserializeObject<MessageResendTxhashModel>(result.Content);
                    resend.data = 2;
                    resend.message = messageFailedTransfer.message;
                    return JsonConvert.SerializeObject(resend, settings);
                }
                if ((int) result.StatusCode == 429)
                {
                    resend.data = 2;
                    resend.message = "Please wait make again transaction after 60 sec";
                    return JsonConvert.SerializeObject(resend, settings);
                }
                resend.data = 2;
                resend.message = "Transfer failed. Please contact supporter !";
                return JsonConvert.SerializeObject(resend, settings);
            }
        }

        [HttpPost]
        [SessionLifeTime]
        public bool CheckPlayerLimited(string username)
        {
            UserPortalModel user = new UserPortalModel()
            {
                username = username
            };
            RefreshToken(HttpContext.Session.GetString("JWToken"), HttpContext.Session.GetString("refreshToken"));
            var result = _iSDKPortalCommonService.SDKPortal_PLayerCheckLimit(user, HttpContext.Session.GetString("JWToken"));
            JObject js = JObject.Parse(result);
            bool.TryParse(js["data"]?.ToString(), out bool parseBool);
            return parseBool;
        }

        public string SearchEmail(string userNameTo)
        {
            RefreshToken(HttpContext.Session.GetString("JWToken"), HttpContext.Session.GetString("refreshToken"));

            var jsonObject = new {username = userNameTo};
            var client = new RestClient(FundistHelper.ClientConfigs.BaseUrlPortal);
            var bearer = $"Bearer {HttpContext.Session.GetString("JWToken")}";
            var request = new RestRequest("api/user/get-email", Method.POST)
                .AddHeader("Authorization", bearer)
                .AddHeader("Accept", "*/*")
                .AddHeader("Content-Type", "application/json")
                .AddJsonBody(jsonObject);
            var response = client.Execute(request);
            return response.Content;
        }
    }
}