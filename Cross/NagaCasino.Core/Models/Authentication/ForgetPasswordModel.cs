using System.ComponentModel.DataAnnotations;

namespace TechDrum.Core.Models.Authentication
{
    public class ForgetPasswordModel
    {
        [Display(Name = "User Name")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "User Name is not empty")]
        [RegularExpression(@"^[0-9a-zA-Z''-'\s]{1,50}$", ErrorMessage = "Sorry, your username must be between 6 and 17 characters long, don't backspace , don't special characters.")]
        public string UserName { get; set; }
       
        
    }
    public class ForgetPassModel
    {
        public string username { get; set; }
        public string clientId { get; set; }
        public string timestamp { get; set; }
    }
}
