using Invedia.DI.Attributes;
using TechDrum.Contract.Service;
using TechDrum.Core.Models.AuthenSso;
using TechDrum.Core.Utils;
using Newtonsoft.Json;
using RestSharp;
using System.Threading.Tasks;

namespace TechDrum.Service
{
    [ScopedDependency(ServiceType = typeof(ISDKSsoCommonService))]

    public class SDKSsoCommonService : ISDKSsoCommonService
    {
        public IRestResponse SDKSSO_Authenticate(object jsonObject)
        {
            var baseUrl = FundistHelper.ClientConfigs.BaseUrlSso;
            var client = new RestClient(baseUrl);
            var request = new RestRequest("/accounts/authenticate", Method.POST)
                .AddHeader("accept", "*/*")
                .AddHeader("Content-Type", "application/json")
                .AddJsonBody(jsonObject);
            var response = client.Execute(request);
            return response;
        }

        public string SDKSSO_RefreshToken(object jsonObject, string jwtToken)
        {
            var bearer = $"Bearer {jwtToken}";
            var baseUrl = FundistHelper.ClientConfigs.BaseUrlSso;
            var client = new RestClient(baseUrl);
            var request = new RestRequest("/accounts/refresh-token", Method.POST)
                .AddHeader("Authorization", bearer)
                .AddHeader("Accept", "*/*")
                .AddHeader("Content-Type", "application/json")
                .AddJsonBody(jsonObject);
            var response = client.Execute(request);
            if (response.IsSuccessful)
            {
                return response.Content;
            }
            return null;
        }


        public string SDKSSO_qrcode(string jwtToken)
        {
            var bearer = $"Bearer {jwtToken}";
            var baseUrl = FundistHelper.ClientConfigs.BaseUrlSso;
            var client = new RestClient(baseUrl);
            var request = new RestRequest("/two-authen/qr-code", Method.GET)
                .AddHeader("Authorization", bearer)
                .AddHeader("Accept", "*/*")
                .AddHeader("Content-Type", "application/json");
            var response = client.Execute(request);
            if (response.IsSuccessful)
            {
                return response.Content;
            }
            return null;
        }
        public string SDKSSO_enable(string code, string jwtToken)
        {
            var bearer = $"Bearer {jwtToken}";
            var baseUrl = FundistHelper.ClientConfigs.BaseUrlSso;
            var client = new RestClient(baseUrl);
            var request = new RestRequest("/two-authen/enable", Method.POST)
                .AddHeader("Authorization", bearer)
                .AddHeader("Accept", "*/*")
                .AddHeader("Content-Type", "application/json");
            var response = client.Execute(request);
            if (response.IsSuccessful)
            {
                return response.Content;
            }
            return null;
        }
        public IRestResponse SDKSSO_RegisterUser(object jsonObject)
        {
            var baseUrl = FundistHelper.ClientConfigs.BaseUrlSso;
            var client = new RestClient(baseUrl);
            var request = new RestRequest("/accounts/register", Method.POST)
                .AddHeader("Accept", "*/*")
                .AddHeader("Content-Type", "application/json")
                .AddJsonBody(jsonObject);
            var response = client.Execute(request);
            return response;

        }

        public string SDKSSO_ResetPass(object jsonObject)
        {
            var baseUrl = FundistHelper.ClientConfigs.BaseUrlSso;
            var client = new RestClient(baseUrl);
            var request = new RestRequest("/accounts/reset-password", Method.POST)
                .AddHeader("Accept", "*/*")
                .AddHeader("Content-Type", "application/json")
                .AddJsonBody(jsonObject);
            var response = client.Execute(request);
            return response.Content;
        }
        public IRestResponse SDKSSO_Validate(object jsonObject)
        {
            var baseUrl = FundistHelper.ClientConfigs.BaseUrlSso;
            var client = new RestClient(baseUrl);
            var request = new RestRequest("/accounts/validate-reset-token", Method.POST)
                .AddHeader("Accept", "*/*")
                .AddHeader("Content-Type", "application/json")
                .AddJsonBody(jsonObject);
            var response = client.Execute(request);
            return response;
        }

        public IRestResponse SDKSSO_VerifyMail(object jsonObject)
        {
            var baseUrl = FundistHelper.ClientConfigs.BaseUrlSso;
            var client = new RestClient(baseUrl);
            var request = new RestRequest("/accounts/verify-email", Method.POST)
                .AddHeader("Accept", "*/*")
                .AddHeader("Content-Type", "application/json")
                .AddJsonBody(jsonObject);
            var response = client.Execute(request);
            return response;

        }
        public async Task<bool> SDKSSO_VerifycodeAsync(string username, string twoFactorToken)
        {
            var body = new { username = username, twoFactorCode = twoFactorToken };
            var baseUrl = FundistHelper.ClientConfigs.BaseUrlSso;
            var client = new RestClient(baseUrl);
            var request = new RestRequest("/two-authen/verify", Method.POST)
                .AddHeader("Accept", "*/*")
                .AddHeader("Content-Type", "application/json")
                .AddJsonBody(body);
            var response = await client.ExecuteAsync<bool>(request);
            return response.Data;
        }
    }
}
