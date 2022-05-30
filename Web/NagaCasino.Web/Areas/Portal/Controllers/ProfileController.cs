using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using TechDrum.Contract.Repository.Models.User;
using TechDrum.Contract.Service;
using TechDrum.Core.Attributes;
using TechDrum.Core.Constants;
using TechDrum.Core.Utils;
using TechDrum.Core.ViewModels;
using TechDrum.Core.ViewModels.ProfileViewModels;
using TechDrum.Web.Utils.Notification;
using TechDrum.Web.Utils.Notification.Models.Constants;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace TechDrum.Web.Areas.Portal.Controllers
{
    [Authorize]
    public class ProfileController : BaseController
    {
        private const string AuthenticatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";

        private readonly SignInManager<UserEntity> _signInManager;
        private readonly UrlEncoder _urlEncoder;

        public ProfileController(IServiceProvider serviceProvider, UrlEncoder urlEncoder, ISDKSsoCommonService iSDKSsoCommonSerice) : base(serviceProvider, iSDKSsoCommonSerice)
        {
            _urlEncoder = urlEncoder;
            _signInManager = serviceProvider.GetRequiredService<SignInManager<UserEntity>>();
        }

        [TempData] public string StatusMessage { get; set; }

        #region Index
        [HttpGet]
        [SessionLifeTime]
        [Route(PortalEndpoint.Profile.ProfileEndpoint)]
        public async Task<IActionResult> Index()
        {
            RefreshToken(HttpContext.Session.GetString("JWToken"), HttpContext.Session.GetString("refreshToken"));
            ViewBag.CookieValue = Request.Cookies["Roles"];
            var user = await UserManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{UserManager.GetUserId(User)}'.");
            }

            var model = new IndexViewModel
            {
                Username = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                IsEmailConfirmed = user.EmailConfirmed,
                StatusMessage = StatusMessage
            };

            return View(model);
        }

        [HttpPost]
        [Route(PortalEndpoint.Profile.ProfileEndpoint)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(IndexViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await UserManager.GetUserAsync(User);
            if (user == null)
            {
                //throw new ApplicationException($"Unable to load user with ID '{UserManager.GetUserId(User)}'.");
                this.SetNotification("", $"Unable to load user with ID '{UserManager.GetUserId(User)}'.",
                    NotificationStatus.Error);
                return RedirectToAction("SignIn", "Auth");
            }

            //var email = user.Email;
            //if (model.Email != email)
            //{
            //    var setEmailResult = await UserManager.SetEmailAsync(user, model.Email);
            //    if (!setEmailResult.Succeeded)
            //    {
            //        throw new ApplicationException(
            //            $"Unexpected error occurred setting email for user with ID '{user.Id}'.");
            //    }
            //}

            var phoneNumber = user.PhoneNumber;
            if (model.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await UserManager.SetPhoneNumberAsync(user, model.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    throw new ApplicationException(
                        $"Unexpected error occurred setting phone number for user with ID '{user.Id}'.");
                }
            }

            StatusMessage = "Your profile has been updated";
            return RedirectToAction(nameof(Index));
        }

        #endregion Index

        #region ChangePassword

        [HttpGet]
        [SessionLifeTime]
        [Route(PortalEndpoint.Profile.ChangePasswordEndpoint)]
        public async Task<IActionResult> ChangePassword()
        {
            RefreshToken(HttpContext.Session.GetString("JWToken"), HttpContext.Session.GetString("refreshToken"));
            ViewBag.CookieValue = Request.Cookies["Roles"];
            var user = await UserManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{UserManager.GetUserId(User)}'.");
            }

            var hasPassword = await UserManager.HasPasswordAsync(user);
            if (!hasPassword)
            {
                //return RedirectToAction(nameof(SetPassword));
            }

            var model = new ChangePasswordViewModel { StatusMessage = StatusMessage };
            return View(model);
        }

        [HttpPost]
        [Route(PortalEndpoint.Profile.ChangePasswordEndpoint)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await UserManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{UserManager.GetUserId(User)}'.");
            }
            var changePasswordResult =
                    await UserManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                this.SetNotification(Messages.Status.Fail, "Password incorect!", NotificationStatus.Error);
                AddErrors(changePasswordResult);
                return View(model);
            }
            var baseUrl = FundistHelper.ClientConfigs.BaseUrlSso;

            var client = new RestClient(baseUrl);

            var request = new RestRequest("accounts/reset-password", Method.POST)
                .AddJsonBody(new { token = model.OldPassword, password = model.NewPassword, confirmPassword = model.ConfirmPassword, username = user.UserName });
            var response = await client.ExecuteAsync(request);
            if (response.IsSuccessful)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                //_logger.LogInformation("User changed their password successfully.");
                StatusMessage = "Change password successful.";

                this.SetNotification("", StatusMessage, NotificationStatus.Success);

                return RedirectToAction(nameof(ChangePassword));
            }

            this.SetNotification(Messages.Status.Fail, "Password current incorect!", NotificationStatus.Error);
            return View(model);

        }

        #endregion ChangePassword

        #region SetPassword

        [HttpGet]
        [SessionLifeTime]
        [Route(PortalEndpoint.Profile.SetPasswordEndpoint)]
        public async Task<IActionResult> SetPassword()
        {
            RefreshToken(HttpContext.Session.GetString("JWToken"), HttpContext.Session.GetString("refreshToken"));
            ViewBag.CookieValue = Request.Cookies["Roles"];
            var user = await UserManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{UserManager.GetUserId(User)}'.");
            }

            var hasPassword = await UserManager.HasPasswordAsync(user);

            if (hasPassword)
            {
                return RedirectToAction(nameof(ChangePassword));
            }

            var model = new SetPasswordViewModel { StatusMessage = StatusMessage };
            return View(model);
        }

        [HttpPost]
        [Route(PortalEndpoint.Profile.SetPasswordEndpoint)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await UserManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{UserManager.GetUserId(User)}'.");
            }

            var addPasswordResult = await UserManager.AddPasswordAsync(user, model.NewPassword);
            if (!addPasswordResult.Succeeded)
            {
                AddErrors(addPasswordResult);
                return View(model);
            }

            await _signInManager.SignInAsync(user, isPersistent: false);
            StatusMessage = "Your password has been set.";

            return RedirectToAction(nameof(SetPassword));
        }

        #endregion SetPassword

        #region ResetAuthenticatorWarning

        [HttpGet]
        [Route(PortalEndpoint.Profile.ResetAuthenticator)]
        public async Task<IActionResult> ResetAuthenticatorWarning()
        {
            var user = await UserManager.GetUserAsync(User);
            ViewBag.CookieValue = Request.Cookies["Roles"];
            var model = new LoginWith2FaViewModel();
            if (user.TwoFactorEnabled)
            {
                model.RememberMe = true;
            }
            else
            {
                model.TwoFactorCode = "000000";
            }
            return View(nameof(ResetAuthenticator), model);
        }

        [HttpPost]
        [Route(PortalEndpoint.Profile.ResetAuthenticator)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetAuthenticator(LoginWith2FaViewModel model)
        {
            RefreshToken(HttpContext.Session.GetString("JWToken"), HttpContext.Session.GetString("refreshToken"));
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await UserManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{UserManager.GetUserId(User)}'.");
            }

            if (user.TwoFactorEnabled)
            {
                var is2FaTokenValid = await VerifyTwoFactorTokenAsync(user, model.TwoFactorCode);
                if (is2FaTokenValid)
                {
                    await UserManager.SetTwoFactorEnabledAsync(user, false);
                    var accessToken = HttpContext.Session.GetString("JWToken");
                    var baseUrl = FundistHelper.ClientConfigs.BaseUrlSso;

                    var client = new RestClient(baseUrl)
                    {
                        Authenticator = new JwtAuthenticator(accessToken)
                    };
                    var request = new RestRequest("two-authen/reset", Method.POST)
                        .AddJsonBody(new { twoFactorCode = model.TwoFactorCode });
                    var response = await client.ExecuteAsync<EnableAuthenticatorViewModel>(request);
                    if (response.IsSuccessful)
                    {
                        var unformattedKey = response.Data.sharedKey.Replace(" ", "").ToUpper();
                        await UserManager.SetAuthenticationTokenAsync(user, tokenName: "AuthenticatorKey", tokenValue: unformattedKey, loginProvider: "[AspNetUserStore]");
                        //await UserManager.ResetAuthenticatorKeyAsync(user);
                        //_logger.LogInformation("User with id '{UserId}' has reset their authentication app key.", user.Id);
                        return RedirectToAction(nameof(EnableAuthenticator));
                    }
                    ModelState.AddModelError(string.Empty, response.Content.ToString());
                    return View(model);
                }
                ModelState.AddModelError(string.Empty, "Invalid authenticator code.");
                return View(model);
            }

            await UserManager.SetTwoFactorEnabledAsync(user, false);
            //await UserManager.ResetAuthenticatorKeyAsync(user);
            //_logger.LogInformation("User with id '{UserId}' has reset their authentication app key.", user.Id);

            return RedirectToAction(nameof(EnableAuthenticator));
        }

        #endregion ResetAuthenticatorWarning

        #region EnableAuthenticator

        [Route(PortalEndpoint.Profile.EnableAuthenticator)]
        [HttpGet]
        [SessionLifeTime]
        public async Task<IActionResult> EnableAuthenticator()
        {
            RefreshToken(HttpContext.Session.GetString("JWToken"), HttpContext.Session.GetString("refreshToken"));
            ViewBag.CookieValue = Request.Cookies["Roles"];
            var user = await UserManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{UserManager.GetUserId(User)}'.");
            }

            var model = new EnableAuthenticatorViewModel();
            await LoadSharedKeyAndQrCodeUriAsync(user, model);

            return View(model);
        }

        [HttpPost]
        [Route(PortalEndpoint.Profile.EnableAuthenticator)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EnableAuthenticator(EnableAuthenticatorViewModel model)
        {
            var user = await UserManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{UserManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadSharedKeyAndQrCodeUriAsync(user, model);
                return View(model);
            }

            // Strip spaces and hypens
            var verificationCode = model.Code.Replace(" ", string.Empty).Replace("-", string.Empty);

            //var is2FaTokenValid = await UserManager.VerifyTwoFactorTokenAsync(
            //    user, UserManager.Options.Tokens.AuthenticatorTokenProvider, verificationCode);

            var accessToken = HttpContext.Session.GetString("JWToken");
            var baseUrl = FundistHelper.ClientConfigs.BaseUrlSso;

            var client = new RestClient(baseUrl)
            {
                Authenticator = new JwtAuthenticator(accessToken)
            };
            var request = new RestRequest("two-authen/enable", Method.POST)
                .AddJsonBody(new { Code = model.Code });
            var response = await client.ExecuteAsync<bool>(request);
            bool is2FaTokenValid = false;
            if (response.IsSuccessful)
            {
                is2FaTokenValid = response.Data;
            };
            if (!is2FaTokenValid)
            {
                ModelState.AddModelError("Code", "Verification code is invalid.");
                await LoadSharedKeyAndQrCodeUriAsync(user, model);
                return View(model);
            }

            await UserManager.SetTwoFactorEnabledAsync(user, true);
            // _logger.LogInformation("User with ID {UserId} has enabled 2FA with an authenticator app.", user.Id);
            var recoveryCodes = await UserManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);
            TempData[TempDataKey.RecoveryCodesKey] = recoveryCodes.ToArray();
            //return RedirectToAction("AccountDashboard", "AccountDashboard");
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home",
                        new { status = Messages.Status.Successful, messages = "Enable success !", type = NotificationStatus.Success });
        }

        #endregion

        #region RecoveryCodes

        [HttpGet]
        [Route(PortalEndpoint.Profile.RecoveryCodes)]
        public IActionResult ShowRecoveryCodes()
        {
            ViewBag.CookieValue = Request.Cookies["Roles"];
            var recoveryCodes = (string[])TempData[TempDataKey.RecoveryCodesKey];
            if (recoveryCodes.Length == 0)
            {
                return RedirectToAction(nameof(TwoFactorAuthentication));
            }

            var model = new ShowRecoveryCodesViewModel { RecoveryCodes = recoveryCodes };
            return View(model);
        }

        [HttpGet]
        [Route(PortalEndpoint.Profile.GenerateRecoveryCodes)]
        public async Task<IActionResult> GenerateRecoveryCodesWarning()
        {
            ViewBag.CookieValue = Request.Cookies["Roles"];
            var user = await UserManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{UserManager.GetUserId(User)}'.");
            }

            if (!user.TwoFactorEnabled)
            {
                throw new ApplicationException(
                    $"Cannot generate recovery codes for user with ID '{user.Id}' because they do not have 2FA enabled.");
            }

            return View(nameof(GenerateRecoveryCodes), new LoginWith2FaViewModel());
        }

        [HttpPost]
        [Route(PortalEndpoint.Profile.GenerateRecoveryCodes)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GenerateRecoveryCodes(LoginWith2FaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var user = await UserManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{UserManager.GetUserId(User)}'.");
            }

            if (!user.TwoFactorEnabled)
            {
                throw new ApplicationException(
                    $"Cannot generate recovery codes for user with ID '{user.Id}' as they do not have 2FA enabled.");
            }

            var is2FaTokenValid = await VerifyTwoFactorTokenAsync(user, model.TwoFactorCode);
            if (!is2FaTokenValid)
            {
                ModelState.AddModelError(string.Empty, "Invalid authenticator code.");
                return View();
            }

            var recoveryCodes = await UserManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);
            // _logger.LogInformation("User with ID {UserId} has generated new 2FA recovery codes.", user.Id);

            var viewModel = new ShowRecoveryCodesViewModel { RecoveryCodes = recoveryCodes.ToArray() };

            return View(nameof(ShowRecoveryCodes), viewModel);
        }

        #endregion

        #region TwoFactorAuthentication

        [HttpGet]
        [SessionLifeTime]
        [Route(PortalEndpoint.Profile.TwoFactorAuthentication)]
        public async Task<IActionResult> TwoFactorAuthentication()
        {
            RefreshToken(HttpContext.Session.GetString("JWToken"), HttpContext.Session.GetString("refreshToken"));
            ViewBag.CookieValue = Request.Cookies["Roles"];
            var user = await UserManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{UserManager.GetUserId(User)}'.");
            }

            var model = new TwoFactorAuthenticationViewModel
            {
                HasAuthenticator = await UserManager.GetAuthenticatorKeyAsync(user) != null,
                Is2FaEnabled = user.TwoFactorEnabled,
                RecoveryCodesLeft = await UserManager.CountRecoveryCodesAsync(user),
            };

            return View(model);
        }

        #endregion

        #region Disable2Fa

        [HttpGet]
        [SessionLifeTime]
        [Route(PortalEndpoint.Profile.DisableAuthenticator)]
        public async Task<IActionResult> Disable2Fa()
        {
            RefreshToken(HttpContext.Session.GetString("JWToken"), HttpContext.Session.GetString("refreshToken"));
            ViewBag.CookieValue = Request.Cookies["Roles"];
            var user = await UserManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{UserManager.GetUserId(User)}'.");
            }

            if (!user.TwoFactorEnabled)
            {
                throw new ApplicationException($"Unexpected error occured disabling 2FA for user with ID '{user.Id}'.");
            }

            return View(nameof(Disable2Fa), new LoginWith2FaViewModel());
        }

        [HttpPost]
        [Route(PortalEndpoint.Profile.DisableAuthenticator)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Disable2Fa(LoginWith2FaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await UserManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{UserManager.GetUserId(User)}'.");
            }

            //var is2FaTokenValid = await VerifyTwoFactorTokenAsync(user, model.TwoFactorCode);
            var accessToken = HttpContext.Session.GetString("JWToken");
            var baseUrl = FundistHelper.ClientConfigs.BaseUrlSso;

            var client = new RestClient(baseUrl)
            {
                Authenticator = new JwtAuthenticator(accessToken)
            };
            var request = new RestRequest("two-authen/disable", Method.POST)
                .AddJsonBody(new { Code = model.TwoFactorCode });
            var response = await client.ExecuteAsync<bool>(request);
            bool is2FaTokenValid = false;
            if (response.IsSuccessful)
            {
                is2FaTokenValid = response.Data;
            };
            if (is2FaTokenValid)
            {
                var disable2FaResult = await UserManager.SetTwoFactorEnabledAsync(user, false);
                if (!disable2FaResult.Succeeded)
                {
                    //throw new ApplicationException($"Unexpected error occured disabling 2FA for user with ID '{user.Id}'.");
                    this.SetNotification("", $"Unexpected error occured disabling 2FA for user with ID '{user.Id}'.",
                        NotificationStatus.Error);
                }
                await _signInManager.SignOutAsync();
                return RedirectToAction("Index", "Home",
                            new { status = Messages.Status.Successful, messages = "Disable success !", type = NotificationStatus.Success });
                //return RedirectToAction(nameof(TwoFactorAuthentication));
            }

            ModelState.AddModelError(string.Empty, "Invalid authenticator code.");
            return View();
        }

        #endregion

        #region Helpers

        private async Task LoadSharedKeyAndQrCodeUriAsync(UserEntity user, EnableAuthenticatorViewModel model)
        {
            var unformattedKey = await UserManager.GetAuthenticatorKeyAsync(user);
            if (string.IsNullOrEmpty(unformattedKey))
            {
                var accessToken = HttpContext.Session.GetString("JWToken");
                var baseUrl = FundistHelper.ClientConfigs.BaseUrlSso;

                var client = new RestClient(baseUrl)
                {
                    Authenticator = new JwtAuthenticator(accessToken)
                };
                var request = new RestRequest("two-authen/qr-code", Method.GET);

                var response = await client.ExecuteAsync<EnableAuthenticatorViewModel>(request);
                if (!response.IsSuccessful) return;

                model.sharedKey = response.Data.sharedKey;
                model.authenticatorUri = response.Data.authenticatorUri;
                unformattedKey = response.Data.sharedKey.Replace(" ", "").ToUpper();
                await UserManager.SetAuthenticationTokenAsync(user, tokenName: "AuthenticatorKey", tokenValue: unformattedKey, loginProvider: "[AspNetUserStore]");

                return;
                //await UserManager.ResetAuthenticatorKeyAsync(user);
                //unformattedKey = await UserManager.GetAuthenticatorKeyAsync(user);
            }

            model.sharedKey = FormatKey(unformattedKey);
            model.authenticatorUri = GenerateQrCodeUri(user.UserName, unformattedKey);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private string FormatKey(string unformattedKey)
        {
            var result = new StringBuilder();
            int currentPosition = 0;
            while (currentPosition + 4 < unformattedKey.Length)
            {
                result.Append(unformattedKey.Substring(currentPosition, 4)).Append(" ");
                currentPosition += 4;
            }

            if (currentPosition < unformattedKey.Length)
            {
                result.Append(unformattedKey.Substring(currentPosition));
            }

            return result.ToString().ToLowerInvariant();
        }

        private string GenerateQrCodeUri(string username, string unformattedKey)
        {
            return string.Format(
                AuthenticatorUriFormat,
                _urlEncoder.Encode("NagaClubs"),
                _urlEncoder.Encode(username),
                unformattedKey);
        }

        #endregion Helpers
    }
}