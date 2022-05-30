using System.ComponentModel.DataAnnotations;

namespace TechDrum.Core.Models.Authentication
{
    public class ConfirmEmailModel : SetPasswordModel
    {
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
