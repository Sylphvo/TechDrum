using System.ComponentModel.DataAnnotations;

namespace TechDrum.Core.ViewModels.AuthViewModels
{
    public class LoginWithRecoveryCodeViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Recovery Code")]
        public string RecoveryCode { get; set; }
        public string Url { get; set; }
    }
}