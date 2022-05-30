namespace TechDrum.Core.Constants
{
    public class PortalEndpoint
    {
        public const string AreaName = "portal";

        public static class Home
        {
            private const string BaseEndpoint = "~/" + AreaName;

            public const string IndexEndpoint = BaseEndpoint;

            public const string OopsEndpoint = "~/oops";

            public const string OopsWithParamEndpoint = OopsEndpoint + "/{statusCode}";
            public const string HomeEndpoint = "~/Home";
            public const string NotFoundEndpoint = "~/NotFound";
        }
        public static class TwoFactorAuth
        {
            public const string TwoFactorAuthUser = "~/two-2fa-user";
            public const string ValidateTwoFactor = "~/validate-two-2fa-user";
        }
        public static class Affiliates
        {

            public const string AffiliatesEndpoint = "~/affiliates";
            public const string CommissionEndpoint = "~/commission";
        }
        public static class AccountDashboard
        {
            public const string AccountDashboardEndpoint = "~/accountdashboard";
        }
        public static class LiveGame
        {
            public const string LiveGameEndpoint = "~/livegame";
            public const string SelectGameEndpoint = "~/select-game";
        }
        public static class History
        {
            public const string CommissionEndpoint = "~/Commission";
            public const string ETHEndpoint = "~/ETH";
            public const string NAGAEndpoint = "~/NAGA";
            public const string NAGAInsEndpoint = "~/NAGA-INS";
            public const string CancelEndpoint = "~/history/cancel";
            public const string VerifyEndpoint = "~/Verifyhistory";
            public const string PolicyEndpoint = "~/policy";
            public const string TxhashEndpoint = "~/check";
        }
        public static class MailReceive
        {
            public const string MailConfirm = "~/confirm-account";
            public const string MailConfirmRollback = "~/rollback";
            public const string MailConfirmRollbackETH = "~/rollback-eth";
            public const string MailConfirmRollbackONE = "~/rollback-naga";
            public const string MailConfirmETH = "~/confirm-eth";
            public const string MailConfirmNAGA = "~/confirm-naga";
            //public const string MailConfirmForgetPass = "~/confirm-forget-pass";
        }
        public static class Profile
        {
            public const string ProfileEndpoint = "~/profile";
            public const string EnableAuthenticator = "~/enable-2fa";
            public const string DisableAuthenticator = "~/disable-2fa";
            public const string RecoveryCodes = "~/recovery-codes";
            public const string GenerateRecoveryCodes = "~/generate-recovery-codes";
            public const string ChangePasswordEndpoint = "~/profile/change-password";
            public const string SetPasswordEndpoint = "~/set-password";
            public const string ResetAuthenticator = "~/reset-authenticator";
            public const string TwoFactorAuthentication = "~/two-factor-authentication";
        }
        public static class User
        {
            private const string BaseEndpoint = "~/" + AreaName + "/users";
            public const string GetPagedEndpoint = BaseEndpoint;
            public const string AddEndpoint = BaseEndpoint + "/add";
            public const string EditEndpoint = BaseEndpoint + "/{id}";

        }

        public static class Auth
        {
            public const string SignInEndpoint = "~/signin";
            public const string LockOutEndpoint = "~/lockout";
            public const string SignIn2FaEndpoint = "~/sign-in-2fa";
            public const string SignInRecoveryCode = "~/sign-in-recovery-code";
            public const string SignUpEndpoint = "~/signup";
            public const string SignUpAffiliateEndpoint = "~/signup/{affiliate}";
            public const string SignOutEndpoint = "~/signout";
            public const string AuthorizedEndpoint = "~/authorized";
            public const string ResendmailRegisterEndpoint = "~/ResendmailRegister";
            public const string ForgetPasswordEndpoint = "~/forget-password";
            public const string ConfirmEmailEndpoint = "~/confirm-email";
            public const string VerifyCodeEndpoint = "~/verify";
            public const string ChangePasswordEndpoint = "~/change-password";
            public const string SetPasswordEndpoint = "~/confirm-forget-pass";
            public const string NotificationForgetPasswordEndpoint = "~/notification-forget-pass";
        }

        public static class Marketing
        {
            public const string AffiliateMarketingEndpoint = "~/affiliate-marketing";
            public const string BuyInsuranceEndpoint = "~/BuyInsurance";
            public const string InsuranceHistoryEndpoint = "~/InsuranceHistory";
            public const string ClaimInsuranceEndpoint = "~/ClaimInsurance";
            public const string ClaimHistoryEndpoint = "~/ClaimHistory";
        }
        public static class Shop
        {
            public const string ShopEndpoint = "~/shop";
        }

    }
}