namespace TechDrum.Core.Models.AuthenSso
{
    public class VerifyEmailModel
    {
        public string token { get; set; }
        public string clientId { get; set; }
        public string username { get; set; }

        public string affiliate { get; set; }
    }
}
