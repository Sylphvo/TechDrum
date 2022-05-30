using System.ComponentModel.DataAnnotations;

namespace TechDrum.Core.Models.Authentication
{
    public class ChangePasswordModel
    {
        public string Continue { get; set; }

        [Display(Name = "Old Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Old password is not empty")]
        public string OldPass { get; set; }

        [Display(Name = "Create new Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Create new password is not empty")]
        [RegularExpression(@"^[0-9a-zA-Z''-'\s]{6,17}$", ErrorMessage = "Sorry, your username must be between 6 and 17 characters long, don't backspace , don't special characters.")]
        public string CreatePasswordNew { get; set; }

        [Display(Name = "Confirm new password")]
        [DataType(DataType.Password)]
        [Compare("CreatePasswordNew")]
        [Required(ErrorMessage = "Confirm new password")]
        public string ConfirmPasswordNew { get; set; }
    }
}