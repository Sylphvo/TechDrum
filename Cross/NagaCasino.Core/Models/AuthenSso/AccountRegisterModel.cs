namespace TechDrum.Core.Models.AuthenSso
{
    public class AccountRegisterModel
    {
        public string title { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string confirmPassword { get; set; }
        public bool acceptTerms { get; set; }
    }
}
