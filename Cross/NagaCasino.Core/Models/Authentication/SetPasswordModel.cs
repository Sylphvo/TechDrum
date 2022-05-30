using System.ComponentModel.DataAnnotations;

namespace TechDrum.Core.Models.Authentication
{
    public class SetPasswordModel
    {
        public string Token { get; set; }

        [Required(ErrorMessage = "Password is not empty !")]
        [RegularExpression(@"^[0-9a-zA-Z''-'\s]{6,17}$", ErrorMessage = "Sorry, your password must be between 6 and 17 characters long, don't backspace , don't special characters.")]
        [DataType(DataType.Password)] 
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is not empty !")]
        [RegularExpression(@"^[0-9a-zA-Z''-'\s]{6,17}$", ErrorMessage = "Sorry, your confirm password must be between 6 and 17 characters long, don't backspace , don't special characters.")]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        public string Username { get; set; }
    }
}