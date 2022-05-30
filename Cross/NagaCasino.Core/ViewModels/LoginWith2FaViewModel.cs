using System.ComponentModel.DataAnnotations;

namespace TechDrum.Core.ViewModels
{
    public class LoginWith2FaViewModel
    {
        [Required]
        [StringLength(6, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
            MinimumLength = 6)]

        [DataType(DataType.Text)]
        [Display(Name = "Authenticator code")]
        public string TwoFactorCode { get; set; }

        [Display(Name = "Remember this machine")]
        public bool RememberMachine { get; set; }

        public bool RememberMe { get; set; }
        public string Url { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
        public string otpCode { get; set; }
    }
}