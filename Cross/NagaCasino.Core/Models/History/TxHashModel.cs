using System.ComponentModel.DataAnnotations;

namespace TechDrum.Core.Models.History
{
    public class TxHashModel
    {
        [Required(ErrorMessage = "Please provide txhash transaction")]
        [RegularExpression(@"^[0-9a-zA-Z''-'\s]$", ErrorMessage = "Sorry, Txhash don't backspace , don't special characters.")]
        public string Txhash { get; set; }
        public string UserId { get; set; }
    }
}
