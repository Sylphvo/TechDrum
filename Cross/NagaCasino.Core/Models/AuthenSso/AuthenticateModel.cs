namespace TechDrum.Core.Models.AuthenSso
{
    public class AuthenticateModel
    {
        public string id { get; set; }
        public string amr { get; set; }
        public string email { get; set; }
        public string role { get; set; }
        public bool isVerified { get; set; }
        public string jwtToken { get; set; }
        public string refreshToken { get; set; }

    }
}
