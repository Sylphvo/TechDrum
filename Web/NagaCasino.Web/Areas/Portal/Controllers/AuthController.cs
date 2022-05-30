using Invedia.Web.HttpUtils;
using Invedia.Web.ITempDataDictionaryUtils;
using Invedia.Web.IUrlHelperUtils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using TechDrum.Contract.Repository.Models.User;
using TechDrum.Contract.Service;
using TechDrum.Core.Commons;
using TechDrum.Core.Constants;
using TechDrum.Core.Exceptions;
using TechDrum.Core.Models.Authentication;
using TechDrum.Core.Utils;
using TechDrum.Core.ViewModels;
using TechDrum.Web.Utils.Notification;
using TechDrum.Web.Utils.Notification.Models.Constants;
using Newtonsoft.Json.Linq;
using Serilog;
using System;
using System.Threading.Tasks;
using TechDrum.Core.ViewModels.AuthViewModels;
using TechDrum.Web.Extensions;
using TechDrum.Core.Attributes;
using TechDrum.Core.Models.AuthenSso;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using RestSharp;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TechDrum.Web.Areas.Portal.Controllers
{
    public class AuthController : BaseController
    {
        private readonly SignInManager<UserEntity> _signInManager;
        private readonly IActionLogService _actionLogService;
        private readonly ISDKPortalCommonService _iSDKPortalCommonService;
        private readonly ISDKSsoCommonService _iSDKSsoCommonService;
        private readonly ILogger _logger;

        public AuthController(IServiceProvider serviceProvider, ISDKPortalCommonService iSDKProtalCommonService, ISDKSsoCommonService iSDKSsoCommonService
            ) : base(serviceProvider, iSDKSsoCommonService)
        {
            _logger = Log.Logger;
            _signInManager = serviceProvider.GetRequiredService<SignInManager<UserEntity>>();
            _actionLogService = serviceProvider.GetRequiredService<IActionLogService>();
            _iSDKPortalCommonService = iSDKProtalCommonService;
            _iSDKSsoCommonService = iSDKSsoCommonService;
        }

        /// <summary>
        ///     Sign Out
        /// </summary>
        /// <returns>  </returns>
        [Route(PortalEndpoint.Auth.SignOutEndpoint)]
        public async Task<IActionResult> SignOutAsync(string @continue)
        {
            // Redirect to Un-Authorize to set a cookie to current site
            if (string.IsNullOrWhiteSpace(@continue)) @continue = Url.AbsoluteAction("SignIn", "Auth");
            await _signInManager.SignOutAsync();
            //clear token
            HttpContext.Session.Clear();
            Response.Cookies.Delete("JWToken");
            Response.Cookies.Delete("2fa");
            Response.Cookies.Delete("Roles");
            ViewBag.RedirectUri = @continue;
            return Redirect(@continue);
        }

        #region SignIn

        /// <summary>
        ///     Sign In
        /// </summary>
        /// <param name="model">  </param>
        /// <param name="returnUrl"></param>
        /// <returns>  </returns>
        /// <remarks>
        ///     Support GrantType: Implicit, Authorization Code (PKCE), Resource Owner Password.
        /// </remarks>
        [Route(PortalEndpoint.Auth.SignInEndpoint)]
        public async Task<IActionResult> SignIn(AuthorizeModel model, string returnUrl)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            var urlName = Request.RouteValues;
            model.Url = returnUrl;
            return View(model);
        }

        /// <summary>
        ///     Submit Sign In
        /// </summary>
        /// <param name="model">  </param>
        /// <returns>  </returns>
        /// <remarks>
        ///     Support GrantType: Implicit, Authorization Code (PKCE), Resource Owner Password.
        /// </remarks>
        [HttpPost]
        [Route(PortalEndpoint.Auth.SignInEndpoint)]
        public async Task<IActionResult> SubmitSignIn([FromForm] AuthorizeModel model)
        {
            var @continue = model.Continue;
            //var ip = CoreHelper.CurrentHttpContext.Request.HttpContext.Items.Values;
            if (string.IsNullOrWhiteSpace(@continue)) @continue = Url.AbsoluteAction("Index", "Home");
            TempData.Set(TempDataKey.RedirectUri, @continue);
            if (!ModelState.IsValid) return View(nameof(SignIn), model);

            var result = await _signInManager.PasswordSignInAsync(model.Login, model.Password, model.Remember, false);



            // check sso 
            var jsonObject = new { username = model.Login, password = model.Password };
            var ssoResult = _iSDKSsoCommonService.SDKSSO_Authenticate(jsonObject);
            if (string.IsNullOrEmpty(ssoResult.Content))
            {
                this.SetNotification(Messages.Status.Fail, "Sign In Failed !", NotificationStatus.Error);
                return View("SignIn", model);
            }
            if (ssoResult.IsSuccessful)
            {
                AuthenticateModel authenModel = JsonConvert.DeserializeObject<AuthenticateModel>(ssoResult.Content);
                HttpContext.Session.SetString("JWToken", authenModel.jwtToken);
                HttpContext.Session.SetString("refreshToken", authenModel.refreshToken);
                SetCookie("JWToken", authenModel.jwtToken);
                SetCookie("Roles", authenModel.role);
                SetCookie("Email", authenModel.email);
                SetCookie("UserName", model.Login);
                SetCookie("PassWord", model.Password);
                if (authenModel.amr == "mfa")
                {
                    return RedirectToAction(nameof(LoginWith2Fa),
                    new { rememberMe = model.Remember, returnUrl = model.Continue, routerurl = model.Url });
                }
                if (authenModel.isVerified == false)
                {
                    var user = await UserManager.FindByNameAsync(model.Login);
                    this.SetNotification(Messages.Status.Fail, Messages.Auth._007, NotificationStatus.Error);
                    return RedirectToAction("VerifyCode", new { username = model.Login, clientId = user.Id, type = VerifyCodeType.Login });
                }
                return RedirectToAction("AccountDashboard", "AccountDashboard");

            }
            this.SetNotification(Messages.Status.Fail, "User name or Password incorrect", NotificationStatus.Error);
            return View("SignIn", model);
        }

        #endregion

        #region SignIn2FaEndpoint

        [HttpGet]
        [Route(PortalEndpoint.Auth.SignIn2FaEndpoint)]
        [AllowAnonymous]
        public async Task<IActionResult> LoginWith2Fa(bool rememberMe, string? returnUrl = null,
            string? routerurl = null)
        {
            // Ensure the user has gone through the username & password screen first
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();


            if (user == null)
            {
                //throw new ApplicationException($"Unable to load two-factor authentication user.");
                return RedirectToAction("SignIn");
            }


            var model = new LoginWith2FaViewModel { RememberMe = rememberMe };
            ViewData["ReturnUrl"] = returnUrl;
            ViewData["Routerurl"] = routerurl;


            return View(model);
        }


        [HttpPost]
        [AllowAnonymous]
        [Route(PortalEndpoint.Auth.SignIn2FaEndpoint)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginWith2Fa(LoginWith2FaViewModel model, bool rememberMe,
            string? returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            string PassWord = Request.Cookies["PassWord"];
            string Username = Request.Cookies["UserName"];
            var optcodeoo = model.TwoFactorCode.Replace(" ", string.Empty).Replace("-", string.Empty);
            var loginWith2Fa = new LoginWith2FaViewModel()
            {
                password = PassWord,
                userName = Username,
                otpCode = optcodeoo
            };
            var ssoResult = _iSDKSsoCommonService.SDKSSO_Authenticate(loginWith2Fa);
            AuthenticateModel authenModel = JsonConvert.DeserializeObject<AuthenticateModel>(ssoResult.Content);
            if (ssoResult.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                this.SetNotification(Messages.Status.Fail, "Invalid authenticator code.",
                      NotificationStatus.Error);
                return View("SignIn", model);
            }
            if (ssoResult.IsSuccessful)
            {
                HttpContext.Session.SetString("JWToken", authenModel.jwtToken);
                HttpContext.Session.SetString("refreshToken", authenModel.refreshToken);
                SetCookie("JWToken", authenModel.jwtToken);
                SetCookie("refreshToken", authenModel.refreshToken);
                var result = await _signInManager.TwoFactorAuthenticatorSignInAsync(optcodeoo, rememberMe,
                     loginWith2Fa.RememberMachine);
                if (result.Succeeded)
                {
                    if (model.Url == "/select-game")
                    {
                        return RedirectToAction("SelectGame", "LiveGame");
                    }
                    else
                    {
                        return RedirectToAction("AccountDashboard", "AccountDashboard");
                    }
                }
                this.SetNotification(Messages.Status.Fail, "Invalid authenticator code.",
                       NotificationStatus.Error);
                return View("SignIn", model);
            }
            this.SetNotification(Messages.Status.Fail, "Invalid authenticator code.",
                       NotificationStatus.Error);
            return View("SignIn", model);
        }

        #endregion
        #region SignInRecoveryCode

        [HttpGet]
        [Route(PortalEndpoint.Auth.SignInRecoveryCode)]
        [AllowAnonymous]
        public async Task<IActionResult> LoginWithRecoveryCode(string? returnUrl = null)
        {
            // Ensure the user has gone through the username & password screen first
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                //throw new ApplicationException($"Unable to load two-factor authentication user.");
                return RedirectToAction("SignIn");
            }


            ViewData["ReturnUrl"] = returnUrl;


            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        [Route(PortalEndpoint.Auth.SignInRecoveryCode)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginWithRecoveryCode(LoginWithRecoveryCodeViewModel model,
            string? returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }


            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                //throw new ApplicationException($"Unable to load two-factor authentication user.");
                this.SetNotification("", "Unable to load two-factor authentication user.", NotificationStatus.Error);
                return RedirectToAction("SignIn");
            }


            var recoveryCode = model.RecoveryCode.Replace(" ", string.Empty);


            var result = await _signInManager.TwoFactorRecoveryCodeSignInAsync(recoveryCode);


            if (result.Succeeded)
            {
                //_logger.LogInformation("User with ID {UserId} logged in with a recovery code.", user.Id);
                if (model.Url == "/select-game")
                {
                    return RedirectToAction("SelectGame", "LiveGame");
                }
                else
                {
                    return RedirectToAction("AccountDashboard", "AccountDashboard");
                }
            }


            if (result.IsLockedOut)
            {
                // _logger.LogWarning("User with ID {UserId} account locked out.", user.Id);
                return RedirectToAction(nameof(Lockout));
            }


            // _logger.LogWarning("Invalid recovery code entered for user with ID {UserId}", user.Id);
            ModelState.AddModelError(string.Empty, "Invalid recovery code entered.");
            return View();
        }

        #endregion
        #region Lockout

        [HttpGet]
        [Route(PortalEndpoint.Auth.LockOutEndpoint)]
        [AllowAnonymous]
        public IActionResult Lockout()
        {
            return RedirectToAction("Index", "Home", new { status = Messages.Status.Fail, messages = "Account blocked !", type = NotificationStatus.Error });
        }

        #endregion
        #region Authorized
        [Route(PortalEndpoint.Auth.AuthorizedEndpoint)]
        public IActionResult Authorized()
        {
            var redirectUri = TempData.Get<string>(TempDataKey.RedirectUri);
            if (string.IsNullOrWhiteSpace(redirectUri))
                redirectUri = Url.AbsoluteAction("AccountDashboard", "AccountDashboard");
            ViewBag.RedirectUri = redirectUri;
            return View();
        }
        #endregion

        [HttpGet]
        [Route(PortalEndpoint.Auth.ResendmailRegisterEndpoint)]
        public async Task<IActionResult> ResendmailRegister()
        {
            var getusername = Request.Cookies["UsernameResend"];
            var baseUrl = FundistHelper.ClientConfigs.BaseUrlSso;
            var client = new RestClient(baseUrl);
            var request = new RestRequest("accounts/resend-email", Method.POST)
                .AddJsonBody(new { Username = getusername });
            var response = await client.ExecuteAsync<IRestResponse>(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                this.SetNotification(Messages.Status.Successful, "Resend mail success",
                                   NotificationStatus.Success);
                return RedirectToAction("VerifyCode", new { username = getusername, type = VerifyCodeType.Register });
            };
            this.SetNotification(Messages.Status.Fail, "Resend mail failed, email has been confirmed",
                                   NotificationStatus.Error);
            return RedirectToAction("VerifyCode", new { username = getusername, type = VerifyCodeType.Register });
        }



        #region  ForgetPassword
        [RequestRateLimit(Name = "forget-password", Seconds = 60)]
        [Route(PortalEndpoint.Auth.ForgetPasswordEndpoint)]
        public IActionResult ForgetPassword(ForgetPasswordModel model)
        {
            return View(model);
        }


        [HttpPost]
        [Route(PortalEndpoint.Auth.ForgetPasswordEndpoint)]
        public async Task<IActionResult> SubmitForgetPassword([FromForm] ForgetPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.UserName);
                if (user == null)
                {
                    this.SetNotification(Messages.Status.Fail, "User Name incorrect or not exist system !",
                        NotificationStatus.Error);
                    return View("ForgetPassword");
                }

                if (!user.EmailConfirmed)
                {
                    this.SetNotification(Messages.Status.Fail, "Please confirm account. !",
                       NotificationStatus.Error);
                    return View("ForgetPassword");
                }
                HttpContext.Session.SetString("UsernameResend", user.UserName);
                ForgetPassModel forget = new ForgetPassModel()
                {
                    username = user.UserName,
                    timestamp = TimeStampHelper.GetTimeStamp()
                };

                var result = _iSDKPortalCommonService.SDKPortal_ForgetPassword(forget);
                if (result.IsSuccessful)
                {
                    this.SetNotification(Messages.Status.Successful,
                        "Please check mail to reset your password", NotificationStatus.Success);
                    return RedirectToAction("VerifyCode", new { username = model.UserName, type = VerifyCodeType.ForgotPassword });
                    //return RedirectToAction("ForgetPassword", new { strNotify = notify });
                }

                this.SetNotification(Messages.Status.Fail, "Email not exist !", NotificationStatus.Error);
                return View("ForgetPassword", model);
            }
            return View("ForgetPassword", model);
        }
        [Route(PortalEndpoint.Auth.NotificationForgetPasswordEndpoint)]
        public IActionResult NotificationFogetPass(string strNotify)
        {
            return View(model: strNotify);
        }
        #endregion
        #region SignUp
        [Route(PortalEndpoint.Auth.SignUpEndpoint)]
        [Route(PortalEndpoint.Auth.SignUpAffiliateEndpoint)]
        public IActionResult SignUp(string affiliate)
        {
            try
            {
                //TODO implement your code HERE
                var model = new SignUpModel();
                if (!string.IsNullOrWhiteSpace(affiliate))
                {
                    model.AffiliatesId = affiliate;
                    return View(model);
                }
                //var user = await UserManager.FindByNameAsync(affiliate);
                //if (user == null) return RedirectToAction("Oops", "Home", new {statusCode = 404});
                //model.AffiliatesId = affiliate;

                return View(model);
            }
            catch (CoreException e)
            {
                var errorModel = new ErrorModel(e);

                // Return bad request due to external client request
                return BadRequest(errorModel);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route(PortalEndpoint.Auth.SignUpEndpoint)]
        public async Task<IActionResult> SubmitSignUp([FromForm] SignUpModel model)
        {
            var @continue = model.Continue;
            if (string.IsNullOrWhiteSpace(@continue)) @continue = Url.AbsoluteAction("Index", "Home");
            //format user name
            var formatUser = model.Login.ToLower().Trim();
            var formatEmail = model.Email;


            //TODO temp to fix email duplicate
            //var checkemail = await UserManager.FindByEmailAsync(formatEmail);
            //if (checkemail != null)
            //{
            //    this.SetNotification(Messages.Status.Fail, "Email Exist, Please register again",
            //        NotificationStatus.Error);
            //    return View("SignUp", model);
            //}
            try
            {
                var resutl = await UserManager.FindByEmailAsync(formatEmail);
                if (resutl != null)
                {
                    this.SetNotification(Messages.Status.Fail, Messages.Auth._010, NotificationStatus.Warning);
                    return View("SignUp", model);
                }
            }
            catch (Exception)
            {
                this.SetNotification(Messages.Status.Fail, Messages.Auth._010, NotificationStatus.Warning);
                return View("SignUp", model);
            }

            TempData.Set(TempDataKey.RedirectUri, @continue);
            if (ModelState.IsValid)
            {
                //check không cho đăng ký affiliate là master
                if (model.AffiliatesId.ToLower() == "master")
                {
                    //master
                    this.SetNotification(Messages.Status.Fail, Messages.Auth._009, NotificationStatus.Warning);
                    return View("SignUp", model);
                }
                else
                {

                    ///check user trùng khi đăng ký
                    var checkUser = await UserManager.FindByNameAsync(formatUser);
                    if (checkUser != null)
                    {
                        this.SetNotification(Messages.Status.Successful, Messages.Auth._010,
                            NotificationStatus.Warning);
                        model.AffiliatesId = null;
                        return View("SignUp", model);
                    }

                    else
                    {
                        //kiểm tra affliliate tồn tại khi đăng ký
                        var checkAffiliate = await UserManager.FindByNameAsync(model.AffiliatesId);
                        if (checkAffiliate != null)
                        {
                            //case confirm mail
                            if (checkAffiliate.EmailConfirmed)
                            {
                                var ip = CoreHelper.CurrentHttpContext?.Request.GetIpAddress();
                                if (ip == "::1")
                                {
                                    ip = FundistHelper.GetIp();
                                }

                                var user = new UserEntity
                                {
                                    UserName = formatUser,
                                    Email = model.Email,
                                    ParentId = checkAffiliate.Id,
                                    Temp3 = ip,
                                    LastCheckTime = DateTimeOffset.UtcNow,
                                    PasswordNotHash = model.Password,
                                };

                                var result = await UserManager.CreateAsync(user, model.Password);
                                if (result.Succeeded)
                                {
                                    //TODO feature use this link generated send to email-api 1st.
                                    // Contact PIC of email-api to prepare api for this.
                                    // create service IEmailSender
                                    // method Task SendEmailAsync(string email, string subject, string message);
                                    // then implement this method to call
                                    //var code = await UserManager.GenerateEmailConfirmationTokenAsync(user);
                                    //var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);

                                    // TODO this code will be move to confirm email. just call to portal when user confirm email
                                    var obj = new
                                    {
                                        clientId = user.Id,
                                        username = user.UserName,
                                        password = user.PasswordNotHash,
                                        confirmPassword = user.PasswordNotHash,
                                        parent = model.AffiliatesId,
                                        email = user.Email,
                                        ip = ip
                                    };
                                    var callPortal = _iSDKSsoCommonService.SDKSSO_RegisterUser(obj);
                                    if (callPortal.IsSuccessful)
                                    {
                                        this.SetNotification(Messages.Status.Successful, Messages.Auth._003,
                                            NotificationStatus.Success);
                                        return RedirectToAction("VerifyCode", new { username = user.UserName, clientId = user.Id, type = VerifyCodeType.Register });
                                        //return RedirectToAction("SignIn");
                                    }

                                    this.SetNotification(Messages.Status.Fail, Messages.Auth._005,
                                        NotificationStatus.Error);
                                    return RedirectToAction("SignUp");
                                }

                                var array = result.ToString().Split(',');
                                switch (array.Length)
                                {
                                    case 3:
                                        this.SetNotification(Messages.Status.Fail, array[0],
                                            NotificationStatus.Warning);
                                        this.SetNotification(Messages.Status.Fail, array[1],
                                            NotificationStatus.Warning);
                                        this.SetNotification(Messages.Status.Fail, array[2],
                                            NotificationStatus.Warning);
                                        break;
                                    case 2:
                                        this.SetNotification(Messages.Status.Fail, array[0],
                                            NotificationStatus.Warning);
                                        this.SetNotification(Messages.Status.Fail, array[1],
                                            NotificationStatus.Warning);
                                        break;
                                    case 1:
                                        this.SetNotification(Messages.Status.Fail, array[0],
                                            NotificationStatus.Warning);
                                        break;
                                }

                                return View("SignUp", model);
                                //return RedirectToAction("SignUp");
                            }

                            this.SetNotification(Messages.Status.Fail, Messages.Auth._011,
                                NotificationStatus.Warning);
                            model.AffiliatesId = null;
                            return View("SignUp", model);
                        }
                        model.AffiliatesId = null;
                        this.SetNotification(Messages.Status.Fail, Messages.Auth._008,
                            NotificationStatus.Warning);

                        return View("SignUp", model);
                    }
                }
            }
            return View("SignUp", model);
        }
        #endregion

        [Route(PortalEndpoint.Auth.VerifyCodeEndpoint)]
        public IActionResult VerifyCode(string username, VerifyCodeType type)
        {
            SetCookie("UsernameResend", username);
            ViewBag.Check = type.ToString();
            var model = new VerifyCodeModel()
            {
                Username = username,
                Type = type
            };
            return View(model);
        }

        [HttpPost]
        [Route(PortalEndpoint.Auth.VerifyCodeEndpoint)]
        public async Task<IActionResult> SubmitVerify([FromForm] VerifyCodeModel model)
        {
            try
            {
                switch (model.Type)
                {
                    case VerifyCodeType.Register:
                        await VerifyRegisterByCode(model);
                        this.SetNotification(Messages.Status.Successful, "Verification successful, you can now login", NotificationStatus.Success);
                        return RedirectToAction("SignIn");

                    case VerifyCodeType.ForgotPassword:
                        var obj = new { username = model.Username, token = model.Code };
                        var resul = _iSDKSsoCommonService.SDKSSO_Validate(obj);
                        if (resul.IsSuccessful)
                        {
                            var setPasswordModel = new SetPasswordModel
                            {
                                Token = model.Code,
                                Username = model.Username
                            };
                            this.SetNotification(Messages.Status.Successful, "validate successful, you can now input new password", NotificationStatus.Success);
                            return View("SetPassword", setPasswordModel);
                        }
                        else
                        {
                            this.SetNotification(Messages.Status.Fail, "validate Failed, please input code", NotificationStatus.Error);
                            return View("VerifyCode", model);
                        }
                    case VerifyCodeType.Login:
                        await VerifyRegisterByCode(model);
                        this.SetNotification(Messages.Status.Successful, "Verification successful, you can now login", NotificationStatus.Success);
                        return RedirectToAction("SignIn");
                    default:
                        throw new ArgumentOutOfRangeException();
                }

            }
            catch (Exception e)
            {
                this.SetNotification(Messages.Status.Fail, e.Message, NotificationStatus.Error);
                return RedirectToAction("VerifyCode", new { username = model.Username, clientId = model.ClientId, type = model.Type });
            }
        }

        private async Task VerifyRegisterByCode(VerifyCodeModel codeModel)
        {
            var user = await UserManager.FindByIdAsync(codeModel.ClientId);
            if (user == null)
            {
                throw new CoreException(ErrorCode.BadRequest, "Account not exist. Please register here");
            }
            else
            {
                var affiliate = await UserManager.FindByIdAsync(user.ParentId);

                var model = new
                {
                    ClientId = codeModel.ClientId,
                    Affiliate = affiliate.UserName,
                    Token = codeModel.Code,
                    Username = codeModel.Username,
                };
                var result = _iSDKSsoCommonService.SDKSSO_VerifyMail(model);
                var check = JsonConvert.DeserializeObject<Notification>(result.Content);
                //if (check.message == "Verification successful, you can now login" || check.message == null)
                if (!result.IsSuccessful)
                {
                    throw new CoreException(ErrorCode.BadRequest, "Sorry, verification failed");
                }
                else
                {
                    user.EmailConfirmed = true;
                    await UserManager.UpdateAsync(user);
                    return;
                }
            }
        }
        #region ConfirmEmail

        [Route(PortalEndpoint.Auth.ConfirmEmailEndpoint)]
        public IActionResult ConfirmEmail(string token)
        {
            var confirmEmailModel = new ConfirmEmailModel
            {
                Token = token
            };

            return View(confirmEmailModel);
        }

        [HttpPost]
        [Route(PortalEndpoint.Auth.ConfirmEmailEndpoint)]
        public IActionResult SubmitConfirmEmail([FromForm] ConfirmEmailModel model)
        {
            this.SetNotification(Messages.Status.Successful, "Now you can sign-in to the system",
                NotificationStatus.Success);

            return RedirectToAction("SignIn");
        }
        #endregion
        #region SetPassword


        [Route(PortalEndpoint.Auth.SetPasswordEndpoint)]
        public IActionResult SetPassword(string token)
        {
            string[] arrStr = token.Split('_');
            var parseTimeStamp = TimeStampHelper.ConvertTimeStamp(arrStr[1]);
            var duration = DateTimeOffset.UtcNow - parseTimeStamp.UtcDateTime;
            if (duration.TotalMinutes >= 30)
                return RedirectToAction("NotificationFogetPass", new { strNotify = "Email verification time out !" });
            var setPasswordModel = new SetPasswordModel
            {
                Username = arrStr[0],
                Token = token
            };
            return View(setPasswordModel);
        }

        [HttpPost]
        [Route(PortalEndpoint.Auth.SetPasswordEndpoint)]
        public async Task<IActionResult> SubmitSetPassword([FromForm] SetPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                this.SetNotification(Messages.Common.InValidFormDataTitle, Messages.Common.InValidFormDataMessage,
                    NotificationStatus.Error);
                return View("SetPassword", model);
            }

            try
            {
                if (model.Password != model.ConfirmPassword)
                {
                    this.SetNotification(Messages.Status.Fail, "Password or Password Confirm incorrect",
                        NotificationStatus.Error);
                    return View("SetPassword", model);
                }

                var user = await UserManager.FindByNameAsync(model.Username);
                if (user == null)
                    return RedirectToAction("NotificationFogetPass", new { strNotify = "Reset password failed. Please try again!" });

                var remove = await UserManager.RemovePasswordAsync(user);
                if (remove.Succeeded)
                {
                    string[] arrStr = model.Token.Split('_');
                    var obj = new { Token = arrStr[2], Password = model.Password, ConfirmPassword = model.ConfirmPassword, Username = model.Username };
                    _iSDKSsoCommonService.SDKSSO_ResetPass(obj);
                    var add = await UserManager.AddPasswordAsync(user, model.Password);
                    if (!add.Succeeded)
                    {
                        this.SetNotification(Messages.Status.Fail, "Reset password failed (require number and text)", NotificationStatus.Error);
                        return View("SetPassword", model);
                    }
                    user.Temp2 = model.Password;
                    await UserManager.UpdateAsync(user);
                    this.SetNotification(Messages.Status.Successful, "Reset password successful !", NotificationStatus.Success);
                    return RedirectToAction("SignIn");
                }
                else
                {
                    this.SetNotification(ErrorCode.BadRequest, "Reset password failed. Please try again!", NotificationStatus.Error);
                    return View("SetPassword", model);
                }
            }
            catch (CoreException ex)
            {
                this.SetNotification(Messages.Status.Fail, ex.Message, NotificationStatus.Error);

                return RedirectToAction("ForgetPassword");
            }
        }
        #endregion
        #region ChangePassword
        [Authorize]
        [Route(PortalEndpoint.Auth.ChangePasswordEndpoint)]
        public IActionResult ChangePassword(string @continue)
        {
            var changePasswordModel = new ChangePasswordModel
            {
                Continue = @continue
            };

            return View(changePasswordModel);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        [Route(PortalEndpoint.Auth.ChangePasswordEndpoint)]
        public async Task<IActionResult> SubmitChangePassword(ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                this.SetNotification(Messages.Common.InValidFormDataTitle, Messages.Common.InValidFormDataMessage,
                    NotificationStatus.Error);
                return View("ChangePassword", model);
            }

            if (string.IsNullOrWhiteSpace(model.Continue)) Url.AbsoluteAction("ChangePassword", "Auth");

            var user = await UserManager.GetUserAsync(User);
            var result = await UserManager.ChangePasswordAsync(user, model.OldPass, model.CreatePasswordNew);
            if (result.Succeeded)
            {
                user.Temp2 = model.CreatePasswordNew;
                await UserManager.UpdateAsync(user);
                this.SetNotification(Messages.Status.Successful, "Change password successful",
                    NotificationStatus.Success);
                return View("ChangePassword");
            }

            this.SetNotification(Messages.Status.Fail, "Password incorrect!", NotificationStatus.Error);
            return View("ChangePassword");
        }
        #endregion
        [HttpPost]
        [AllowAnonymous]
        [Route("~/auth/verify-token")]
        public async Task<IActionResult> Validate2Fa(string username, string pinCode)
        {
            var user = await UserManager.FindByNameAsync(username);
            if (user == null)
            {
                return Ok(false);
            }

            var check = await VerifyTwoFactorTokenAsync(user, pinCode);
            return Ok(check);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("~/auth/confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            var user = await UserManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{userId}'.");
            }

            var result = await UserManager.ConfirmEmailAsync(user, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }
        #region ResetPassword
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null)
        {
            if (code == null)
            {
                throw new ApplicationException("A code must be supplied for password reset.");
            }

            var model = new ResetPasswordViewModel { Code = code };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await UserManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }

            var result = await UserManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }

            AddErrors(result);
            return View();
        }
        #endregion
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }
        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        #endregion
    }
}