using Invedia.Web.HttpUtils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using TechDrum.Contract.Repository.Models.Transaction;
using TechDrum.Contract.Repository.Models.TransactionMail;
using TechDrum.Contract.Service;
using TechDrum.Core.Attributes;
using TechDrum.Core.Commons;
using TechDrum.Core.Constants;
using TechDrum.Core.Models.AuthenSso;
using TechDrum.Core.Models.Portal;
using TechDrum.Core.Utils;
using TechDrum.Web.Utils.Notification;
using TechDrum.Web.Utils.Notification.Models.Constants;
using Newtonsoft.Json;
using System;

namespace TechDrum.Web.Areas.Portal.Controllers
{
    public class MailReceiveController : BaseController
    {
        private readonly ITransactionMailService _transactionMailService;
        private readonly ITransactionService _transactionService;
        private readonly ISDKPortalCommonService _iSDKPortalCommonService;
        private readonly ISDKSsoCommonService _iSDKSsoCommonService;
        public MailReceiveController(IServiceProvider serviceProvider, ISDKPortalCommonService iSDKPortalCommonService, ISDKSsoCommonService iSDKSsoCommonService) : base(serviceProvider, iSDKSsoCommonService)
        {
            _transactionMailService = serviceProvider.GetRequiredService<ITransactionMailService>();
            _transactionService = serviceProvider.GetRequiredService<ITransactionService>();
            _iSDKPortalCommonService = iSDKPortalCommonService;
            _iSDKSsoCommonService = iSDKSsoCommonService;
        }

        [Route(PortalEndpoint.MailReceive.MailConfirm)]
        public async System.Threading.Tasks.Task<IActionResult> Index(string token)
        {
            string[] arrStr = token.Split('_'); 
            var user = await UserManager.FindByNameAsync(arrStr[0]);
            if (user == null)
            {
                this.SetNotification(Messages.Status.Fail, "Account not exist. Please register here", NotificationStatus.Error);
                return Redirect("signin");
            }
            else
            {
                if (user.EmailConfirmed == false)
                {


                    var affiliate = await UserManager.FindByIdAsync(user.ParentId);
                    VerifyEmailModel model = new VerifyEmailModel()
                    {
                        username = arrStr[0],
                        affiliate = affiliate.UserName,
                        clientId = user.Id,
                        token = arrStr[1]
                    };
                    var result = _iSDKSsoCommonService.SDKSSO_VerifyMail(model);
                    var check = JsonConvert.DeserializeObject<Notification>(result.Content);
                    //if (check.message == "Verification successful, you can now login" || check.message == null)
                    if (!result.IsSuccessful)
                    {
                        this.SetNotification(Messages.Status.Fail, "Sorry, verification failed", NotificationStatus.Error);
                        return Redirect("signin");
                    }
                    else
                    {
                        user.EmailConfirmed = true;
                        await UserManager.UpdateAsync(user);
                        this.SetNotification(Messages.Status.Successful, "Verification successful, you can now login", NotificationStatus.Success);
                        return Redirect("signin");
                    }
                }
                else
                {
                    this.SetNotification(Messages.Status.Fail, "Sorry, verification failed", NotificationStatus.Error);
                    return Redirect("signin");
                }
            }
            
        }
        #region confirm withdraw eth
        [HttpGet]
        [Route(PortalEndpoint.MailReceive.MailConfirmETH)]
        public IActionResult ConfirmWithdrawETH(string token)
        {
            string[] arrStr = token.Split('_'); //tách chuỗi token thành timestamp và userId
            if (arrStr.Length < 3)
                return RedirectToAction("Index", "Home",
                    new { status = "Fail", messages = "Validate token failed can't transaction !", type = NotificationStatus.Error });
            var result = _iSDKPortalCommonService.SDKPortal_GetByToken(arrStr[1], arrStr[0]);
            if (result.IsSuccessful)
            {
                var tokenModel = new Tokens()
                {
                    token = arrStr[2],
                    transId = arrStr[1],
                    timeStamp = arrStr[0]
                };
                result.Data.token = tokenModel;
                result.Data.strToken = token;
                return View(result.Data);
            }
            return RedirectToAction("Index", "Home",
                        new { status = Messages.Status.Fail, messages = "Transaction invalid !", type = NotificationStatus.Error });
        }
        [HttpPost]
        [Route(PortalEndpoint.MailReceive.MailConfirmETH)]
        //[SessionLifeTime]
        public IActionResult SubmitConfirmWithdrawETH([FromForm] ConfirmTranModel modelForm)
        {
            //RefreshToken(HttpContext.Session.GetString("JWToken"), HttpContext.Session.GetString("refreshToken"));
            var transactionMail = _transactionMailService.getTransactionById(modelForm.strToken);
            if (transactionMail == null)
            {
                var Ip = CoreHelper.CurrentHttpContext?.Request.GetIpAddress();
                if (Ip == "127.0.0.1")
                {
                    Ip = "115.78.236.20";
                }

                WithdrawModel model = new WithdrawModel()
                {
                    transId = modelForm.token.transId,
                    timestamp = modelForm.token.timeStamp,
                    ip = Ip,
                    token = modelForm.token.token
                };
                TransactionEnity transactionEntity;
                var result = _iSDKPortalCommonService.SDKPortal_RollBackTransaction(model, TransactionType.WITHDRAWETH/*, HttpContext.Session.GetString("JWToken")*/);
                if (result.IsSuccessful)
                {
                    TransactionMailEntity entity = new TransactionMailEntity()
                    {
                        TypeMail = "Withdraw eth",
                        Token = modelForm.strToken,
                        Temp = CoreHelper.CurrentHttpContext?.Request.GetIpAddress()
                    };
                    _transactionMailService.Create(entity);
                    transactionEntity = _transactionService.GetTransactionByToken(model.timestamp);
                    transactionEntity.Token = model.token;
                    transactionEntity.Status = "Done";
                    transactionEntity.IpConfirm = CoreHelper.CurrentHttpContext?.Request.GetIpAddress();
                    transactionEntity.UserConfirmTime = DateTimeOffset.UtcNow;
                    _transactionService.Update(transactionEntity);
                    return RedirectToAction("Index", "Home",
                        new { status = Messages.Status.Successful, messages = "Confirm withdraw transaction is successful !", type = NotificationStatus.Success });
                }
                else
                {
                    return RedirectToAction("Index", "Home",
                        new { status = Messages.Status.Fail, messages = "Withdraw transaction has been confirm failed !", type = NotificationStatus.Error });
                }
            }
            return RedirectToAction("Index", "Home",
                        new { status = Messages.Status.Fail, messages = "Withdraw transaction has been confirmed or canceled !", type = NotificationStatus.Error });
        }

        #endregion
        #region confirm withdraw naga
        [HttpGet]
        [Route(PortalEndpoint.MailReceive.MailConfirmNAGA)]
        public IActionResult ConfirmWithdrawNAGA(string token)
        {
            string[] arrStr = token.Split('_'); //tách chuỗi token thành timestamp và userId
            if (arrStr.Length < 3)
                return RedirectToAction("Index", "Home",
                    new { status = "Fail", messages = "Validate token failed can't transaction !", type = NotificationStatus.Error });
            var result = _iSDKPortalCommonService.SDKPortal_GetByToken(arrStr[1], arrStr[0]);
            if (result.IsSuccessful)
            {
                var tokenModel = new Tokens()
                {
                    token = arrStr[2],
                    transId = arrStr[1],
                    timeStamp = arrStr[0]
                };
                result.Data.token = tokenModel;
                result.Data.strToken = token;
                return View(result.Data);
            }
            return RedirectToAction("Index", "Home",
                        new { status = Messages.Status.Fail, messages = "Transaction invalid !", type = NotificationStatus.Error });
        }

        [Route(PortalEndpoint.MailReceive.MailConfirmNAGA)]
        public IActionResult SubmitConfirmWithdrawNAGA([FromForm] ConfirmTranModel modelForm)
        {
            var transactionMail = _transactionMailService.getTransactionById(modelForm.strToken);
            if (transactionMail == null)
            {
                var Ip = CoreHelper.CurrentHttpContext?.Request.GetIpAddress();
                if (Ip == "127.0.0.1")
                {
                    Ip = "115.78.236.20";
                }

                WithdrawModel model = new WithdrawModel()
                {
                    transId = modelForm.token.transId,
                    timestamp = modelForm.token.timeStamp,
                    ip = Ip,
                    token = modelForm.token.token
                };
                var result = _iSDKPortalCommonService.SDKPortal_RollBackTransaction(model, TransactionType.TRANSFER/*, HttpContext.Session.GetString("JWToken")*/);
                TransactionEnity transactionEntity;
                if (result.IsSuccessful)
                {
                    TransactionMailEntity entity = new TransactionMailEntity()
                    {
                        TypeMail = "Transfer NAGA",
                        Token = modelForm.strToken,
                        Temp = CoreHelper.CurrentHttpContext?.Request.GetIpAddress()
                    };
                    _transactionMailService.Create(entity);
                    transactionEntity = _transactionService.GetTransactionByToken(model.timestamp);
                    transactionEntity.Token = model.token;
                    transactionEntity.Status = "Done";
                    transactionEntity.IpConfirm = CoreHelper.CurrentHttpContext?.Request.GetIpAddress();
                    transactionEntity.UserConfirmTime = DateTimeOffset.UtcNow;
                    _transactionService.Update(transactionEntity);
                    return RedirectToAction("Index", "Home",
                        new { status = Messages.Status.Successful, messages = "Confirm transfer transaction is successful !", type = NotificationStatus.Success });
                }
                else
                {
                    return RedirectToAction("Index", "Home",
                            new { status = Messages.Status.Fail, messages = "Transfer transaction has been confirm failed !", type = NotificationStatus.Error });
                }
            }
            return RedirectToAction("Index", "Home",
                            new { status = Messages.Status.Fail, messages = "Transfer transaction has been confirmed or canceled!", type = NotificationStatus.Error });
        }
        #endregion


        [Route(PortalEndpoint.MailReceive.MailConfirmRollbackETH)]
        //[SessionLifeTime]
        public IActionResult RollbackWithdrawETH([FromForm] ConfirmTranModel modelForm)
        {
            //RefreshToken(HttpContext.Session.GetString("JWToken"), HttpContext.Session.GetString("refreshToken"));
            var transactionMail = _transactionMailService.getTransactionById(modelForm.strToken);
            if (transactionMail == null)
            {
                var Ip = CoreHelper.CurrentHttpContext?.Request.GetIpAddress();
                if (Ip == "127.0.0.1")
                {
                    Ip = "115.78.236.20";
                }

                WithdrawModel model = new WithdrawModel()
                {
                    transId = modelForm.token.transId,
                    timestamp = modelForm.token.timeStamp,
                    ip = Ip,
                    token = modelForm.token.token
                };
                var result = _iSDKPortalCommonService.SDKPortal_RollBackTransaction(model, TransactionType.CANCLEETH/*, HttpContext.Session.GetString("JWToken")*/);
                TransactionEnity transactionEntity;
                if (result.IsSuccessful)
                {
                    TransactionMailEntity entity = new TransactionMailEntity()
                    {
                        TypeMail = "Cancel withdraw eth",
                        Token = modelForm.strToken,
                        Temp = CoreHelper.CurrentHttpContext?.Request.GetIpAddress()
                    };
                    _transactionMailService.Create(entity);
                    transactionEntity = _transactionService.GetTransactionByToken(model.timestamp);
                    transactionEntity.Token = model.token;
                    transactionEntity.Status = "Rollback";
                    transactionEntity.IpConfirm = CoreHelper.CurrentHttpContext?.Request.GetIpAddress();
                    transactionEntity.UserConfirmTime = DateTimeOffset.UtcNow;
                    _transactionService.Update(transactionEntity);
                    return RedirectToAction("Index", "Home",
                        new { status = Messages.Status.Successful, messages = "Cancel withdraw transaction is successful !", type = NotificationStatus.Success });
                }
                else
                {
                    return RedirectToAction("Index", "Home",
                                new { status = Messages.Status.Fail, messages = "Withdraw transaction has been cancel failed !", type = NotificationStatus.Error });

                }
            }
            return RedirectToAction("Index", "Home",
                                new { status = Messages.Status.Fail, messages = "Withdraw transaction has been confirmed or canceled!", type = NotificationStatus.Error });
        }

        [Route(PortalEndpoint.MailReceive.MailConfirmRollbackONE)]
        //[SessionLifeTime]
        public IActionResult RollbackWithdrawONE([FromForm] ConfirmTranModel modelForm)
        {
            //RefreshToken(HttpContext.Session.GetString("JWToken"), HttpContext.Session.GetString("refreshToken"));
            var transactionMail = _transactionMailService.getTransactionById(modelForm.strToken);
            if (transactionMail == null)
            {
                var Ip = CoreHelper.CurrentHttpContext?.Request.GetIpAddress();
                if (Ip == "127.0.0.1")
                {
                    Ip = "115.78.236.20";
                }

                WithdrawModel model = new WithdrawModel()
                {
                    transId = modelForm.token.transId,
                    timestamp = modelForm.token.timeStamp,
                    ip = Ip,
                    token = modelForm.token.token
                };
                var result = _iSDKPortalCommonService.SDKPortal_RollBackTransaction(model, TransactionType.CANCLEONE/*, HttpContext.Session.GetString("JWToken")*/);
                TransactionEnity transactionEntity;
                if (result.IsSuccessful)
                {
                    TransactionMailEntity entity = new TransactionMailEntity()
                    {
                        TypeMail = "Cancel Transfer NAGA",
                        Token = modelForm.strToken,
                        Temp = CoreHelper.CurrentHttpContext?.Request.GetIpAddress()
                    };
                    _transactionMailService.Create(entity);
                    transactionEntity = _transactionService.GetTransactionByToken(model.timestamp);
                    transactionEntity.Token = model.token;
                    transactionEntity.Status = "Rollback";
                    transactionEntity.IpConfirm = CoreHelper.CurrentHttpContext?.Request.GetIpAddress();
                    transactionEntity.UserConfirmTime = DateTimeOffset.UtcNow;
                    _transactionService.Update(transactionEntity);
                    return RedirectToAction("Index", "Home",
                        new { status = Messages.Status.Successful, messages = "Cancel transfer transaction is successful !", type = NotificationStatus.Success });
                }
                else
                {
                    return RedirectToAction("Index", "Home",
                                new { status = Messages.Status.Fail, messages = "Transfer transaction has been cancel failed !", type = NotificationStatus.Error });

                }
            }
            return RedirectToAction("Index", "Home",
                                new { status = Messages.Status.Fail, messages = "Transfer transaction has been cofirmed or canceled!", type = NotificationStatus.Error });
        }

        [Route(PortalEndpoint.MailReceive.MailConfirmRollback)]
        public IActionResult Notification(string message)
        {
            return View(model: message);
        }
    }
}