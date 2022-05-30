namespace TechDrum.Core.Constants
{
    public class Messages
    {
        public class Common
        {
            public const string SomethingWentWrong = "Oops, something went wrong!";
            public const string InValidFormDataTitle = "Invalid Data";
            public const string InValidFormDataMessage = "Please see error detail in the Form";

            public const string Yes = "Yes";
            public const string No = "No";
        }

        public class Auth
        {
            // Page Title Auth
            public const string PageTitleAuthorized = "Authorized";

            public const string PageTitleConfirmEmail = "Confirm Email";
            public const string PageTitleForgetPassword = "Forget Password";
            public const string PageTitleSetPassword = "Set Password";
            public const string PageTitleSignIn = "Sign In";
            public const string PageTitleSignUp = "Sign Up";
            public const string PageTitleVerify = "Verify Sign Up";
            public const string PageTitleUnAuthorize = "Un Authorize";
            public const string PageTitleChangePassword = "Change Password";

            public const string _001 = "Please check email inbox to set new password for your account";
            public const string _002 = "Please check your email inbox to active your account";
            public const string _003 = "Register successful, please confirm mail";
            public const string _004 = "Change password successful";
            public const string _005 = "Sign-up failed. Please try again";
            public const string _006 = "This account can not create !";
            public const string _007 = "Please confirm email inbox";
            public const string _008 = "Affiliate not exist !";
            public const string _009 = "Affiliate not allowed register !";
            public const string _010 = "Username or email already exists !. Please try again"; 
            public const string _011 = "Affiliate not yet confirm mail !"; 
        }
        public class Status
        {
            public const string Successful = "Successful";
            public const string Fail = "Fail";
        }

        public class User
        {
            public const string AddedFormat = "Added the User {0}";
            public const string UpdatedFormat = "Updated the User {0}";
            public const string DoesNotExist = "The User doest not exist";

            public const string UpdatedProfile = "Updated your profile";

            // Page Title User
            public const string PageTitleList = "User";

            public const string PageTitleAdd = "New User";
            public const string PageTitleUpdate = "Update User";
            public const string PageTitleProfile = "Profile";
        }


        public class Home
        {
            // Page Title Home
            public const string PageTitleHome = "Home";
        }
        public class Profile
        {
            // Page Title Home
            public const string PageTitleProfile = "Security";
        }
        public class Affiliates
        {
            // Page Title Affiliates
            public const string PageTitleAffiliates = "Affiliates";
            public const string PageTitleCommission = "Commission";
        }
        public class AccountDashboard
        {
            // Page Title AccountDashboard
            public const string PageTitleAccountDashboard = "Account Dashboard";
        }
        public class LiveGame
        {
            // Page Title LiveGame
            public const string PageTitleLiveGame = "Live Game";
            public const string PageTitleSelectGame = "Select Game";
        }
        public class History
        {
            // Page Title History
            public const string PageTitleTransaction = "Transaction";
            public const string PageTitlePolicy = "Policy";
        }
    }
}
