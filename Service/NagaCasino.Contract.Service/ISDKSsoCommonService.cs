using TechDrum.Core.Models.AuthenSso;
using RestSharp;
using System.Threading.Tasks;

namespace TechDrum.Contract.Service
{
    public interface ISDKSsoCommonService
    {
        public IRestResponse SDKSSO_RegisterUser(object jsonObject);
        public IRestResponse SDKSSO_VerifyMail(object jsonObject);
        public string SDKSSO_ResetPass ( object jsonObject);
        public IRestResponse SDKSSO_Validate ( object jsonObject);
        public IRestResponse SDKSSO_Authenticate(object jsonObject);
        public string SDKSSO_RefreshToken(object jsonObject, string jwtToken);
        public string SDKSSO_qrcode(string jwtToken);
        public string SDKSSO_enable(string code, string jwtToken);  
        public Task<bool> SDKSSO_VerifycodeAsync(string username, string twoFactor2fa);
    }
}
