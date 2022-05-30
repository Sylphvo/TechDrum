using System.ComponentModel.DataAnnotations;
using TechDrum.Core.Constants;

namespace TechDrum.Core.Models.Authentication
{
    public class VerifyCodeModel
    {
        [Display(Name = "Email")]
        public string Username { get; set; }
        public string Code { get; set; }
        public string ClientId { get; set; }
        public VerifyCodeType Type { get; set; }
    }
}
