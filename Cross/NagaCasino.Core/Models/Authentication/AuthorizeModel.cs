using System.ComponentModel.DataAnnotations;

namespace TechDrum.Core.Models.Authentication
{
    public class AuthorizeModel
    {
        /// <summary>
        ///     Redirect URI
        /// </summary>
        public string Continue { get; set; }
        public bool Remember { get; set; }

        /// <summary>
        ///     Hint - pre-enter User Name
        /// </summary>
        [Display(Name = "Username")]
        [Required(ErrorMessage ="User name is not empty")]
        [RegularExpression(@"^[0-9a-zA-Z''-'\s]{1,50}$", ErrorMessage = "Sorry, your username is only 50 characters long max, don't backspace , don't special characters.")]
        public string Login { get; set; }

        /// <summary>
        ///     Hint - pre-enter Password
        /// </summary>
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is not empty !")]
        [RegularExpression(@"^[0-9a-zA-Z''-'\s]{6,100}$", ErrorMessage = "Sorry, your password must be between 6 and 100 characters long, don't backspace , don't special characters.")]

        public string Password { get; set; }
        public string Url { get; set; }
    }
    public class SignUpModel : AuthorizeModel
    {
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage ="Email is not empty")]
        [RegularExpression(@"^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,}$", ErrorMessage = "Sorry, Please fill right email. Ex: naga@gmail.com")]
        public string Email { get; set; }

        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage = "Confirm Password is not empty !")] 
        [RegularExpression(@"^[0-9a-zA-Z''-'\s]{6,100}$", ErrorMessage = "Sorry, your confirm password must be between 6 and 100 characters long, don't backspace , don't special characters.")]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Affiliates Id")]
        [Required(ErrorMessage = "Affiliate is not empty !")]
        
        public string AffiliatesId { get; set; }
        public string Affiliatestemp { get; set; }
    }
    public class UpdatePass:AuthorizeModel
    {
        [Display(Name = "Current Password")]
        [Required(ErrorMessage = "Current Password is not empty !")]
        [RegularExpression(@"^[0-9a-zA-Z''-'\s]{6,100}$", ErrorMessage = "Sorry, your current password must be between 6 and 100 characters long, don't backspace , don't special characters.")]
        public string CurrentPass { get; set; }

        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage = "Confirm Password is not empty !")]
        [RegularExpression(@"^[0-9a-zA-Z''-'\s]{6,100}$", ErrorMessage = "Sorry, your confirm password must be between 6 and 100 characters long, don't backspace , don't special characters.")]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
    public class Validate : UpdatePass
    {
        public string CodeValidate { get; set; }
    }
}