using Invedia.DI.Attributes;
using TechDrum.Contract.Service;
using TechDrum.Core.Constants;
using TechDrum.Core.Models.AuthenSso;
using TechDrum.Core.Utils;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace TechDrum.Service
{
    [ScopedDependency(ServiceType = typeof(ISDKPortalCommonService))]
    public class SDKPortalCommonService : ISDKPortalCommonService
    {
        //public SDKPortalCommonService()
        //{

        //}
        public string SDKPortal_AffiliateTree(object jsonObject, string jwtToken)
        {
            var bearer = $"Bearer {jwtToken}";
            var baseUrl = FundistHelper.ClientConfigs.BaseUrlPortal;
            var client = new RestClient(baseUrl);
            var request = new RestRequest("api/user/my-affiliate", Method.POST)
                .AddHeader("Authorization", bearer)
                .AddHeader("Accept", "*/*")
                .AddHeader("Content-Type", "application/json")
                .AddJsonBody(jsonObject);
            var response = client.Execute(request);
            return response.Content;
        }

        public IRestResponse SDKPortal_CreateUser(object jsonObject, string jwtToken)
        {
            var bearer = $"Bearer {jwtToken}";
            var baseUrl = FundistHelper.ClientConfigs.BaseUrlPortal;
            var client = new RestClient(baseUrl);
            var request = new RestRequest("api/user/register", Method.POST)
                .AddHeader("Authorization", bearer)
                .AddHeader("Accept", "*/*")
                .AddHeader("Content-Type", "application/json")
                .AddJsonBody(jsonObject);
            var restResponse = client.Post(request);
            return restResponse;
        }

        public IRestResponse SDKPortal_ForgetPassword(object jsonObject)
        {
            var baseUrl = FundistHelper.ClientConfigs.BaseUrlSso;
            var client = new RestClient(baseUrl);
            var request = new RestRequest("accounts/forgot-password", Method.POST)
                .AddHeader("Accept", "*/*")
                .AddHeader("Content-Type", "application/json")
                .AddJsonBody(jsonObject);
            var response = client.Execute(request);
            return response;
        }

        public string SDKPortal_GetAddressCrypto(object jsonObject, string jwtToken)
        {
            var bearer = $"Bearer {jwtToken}";
            var client = new RestClient(FundistHelper.ClientConfigs.BaseUrlPortal);
            var request = new RestRequest("api/user/get-address", Method.POST)
                .AddHeader("Authorization", bearer)
                .AddHeader("Accept", "*/*")
                .AddHeader("Content-Type", "application/json")
                .AddJsonBody(jsonObject);
            var response = client.Execute(request);
            var js = JObject.Parse(response.Content);
            return js["data"]?.ToString();
        }

        public string SDKPortal_HistoryUser(string type, object jsonObject, string jwtToken)
        {
            var bearer = $"Bearer {jwtToken}";
            var baseUrl = FundistHelper.ClientConfigs.BaseUrlPortal;
            var client = new RestClient(baseUrl);
            var request = new RestRequest("api/transaction/history", Method.POST)
                .AddHeader("Authorization", bearer)
                .AddHeader("Accept", "*/*")
                .AddHeader("Content-Type", "application/json")
                .AddJsonBody(jsonObject);
            var resposne = client.Execute(request);
            return resposne.Content;
        }

        public string SDKPortal_PLayerCheckLimit(object jsonObject, string jwtToken)
        {
            var bearer = $"Bearer {jwtToken}";
            var baseUrl = FundistHelper.ClientConfigs.BaseUrlPortal;
            var client = new RestClient(baseUrl);
            var request = new RestRequest("api/user/get-limit", Method.POST)
                .AddHeader("Authorization", bearer)
                .AddHeader("Accept", "*/*")
                .AddHeader("Content-Type", "application/json")
                .AddJsonBody(jsonObject);
            var response = client.Execute(request);
            return response.Content;
        }

        public string SDKPortal_PlayerCheckPolicy(object jsonObject, string jwtToken)
        {
            var bearer = $"Bearer {jwtToken}";
            var baseUrl = FundistHelper.ClientConfigs.BaseUrlPortal;
            var client = new RestClient(baseUrl);
            var request = new RestRequest("api/user/check-policy-eth", Method.POST)
                .AddHeader("Authorization", bearer)
                .AddHeader("Accept", "*/*")
                .AddHeader("Content-Type", "application/json")
                .AddJsonBody(jsonObject);
            var response = client.Execute(request);
            return response.Content;
        }

        public IRestResponse SDKPortal_ResendTransaction(object jsonObject, string jwtToken)
        {
            var bearer = $"Bearer {jwtToken}";
            var baseUrl = FundistHelper.ClientConfigs.BaseUrlPortal;
            var client = new RestClient(baseUrl);
            var request = new RestRequest("/api/transaction/resend-mail-V2", Method.POST)
                .AddHeader("Authorization", bearer)
                .AddHeader("Accept", "*/*")
                .AddHeader("Content-Type", "application/json")
                .AddJsonBody(jsonObject);
            var response = client.Execute(request);
            return response;
        }
        public IRestResponse SDKPortal_CancelTransaction(object jsonObject, string jwtToken)
        {
            var bearer = $"Bearer {jwtToken}";
            var baseUrl = FundistHelper.ClientConfigs.BaseUrlPortal;
            var client = new RestClient(baseUrl);
            var request = new RestRequest("api/transaction/cancel", Method.POST)
                .AddHeader("Authorization", bearer)
                .AddHeader("Accept", "*/*")
                .AddHeader("Content-Type", "application/json")
                .AddJsonBody(jsonObject);
            var response = client.Execute(request);
            return response;
        }
        public IRestResponse SDKPortal_VerifyTransaction(object jsonObject, string type, string jwtToken)
        {
            var url = "";
            var bearer = $"Bearer {jwtToken}";
            switch (type)
            {
                case "ETH":
                    url = "api/transaction/confirm-eth";
                    break;
                case "NAGA":
                    url = "api/user/deposit-naga";
                    break;
                case "NAGA-INS":
                    url = "api/user/deposit-naga";
                    break;
                default:
                    break;
            }
            var baseUrl = FundistHelper.ClientConfigs.BaseUrlPortal;
            var client = new RestClient(baseUrl);
            var request = new RestRequest(url, Method.POST)
                .AddHeader("Authorization", bearer)
                .AddHeader("Accept", "*/*")
                .AddHeader("Content-Type", "application/json")
                .AddJsonBody(jsonObject);
            var response = client.Execute(request);
            return response;
        }

        public IRestResponse SDKPortal_RollBackTransaction(object jsonObject, string type)
        {
            var key = "";
            //var bearer = $"Bearer {jwtToken}";
            switch (type)
            {
                case TransactionType.CANCLEETH:
                    key = "transaction/rollback-eth";
                    break;
                case TransactionType.CANCLEONE:
                    key = "user/rollback-naga";
                    break;
                case TransactionType.WITHDRAWETH:
                    key = "api/transaction/confirm-eth-v2";
                    break;
                case TransactionType.TRANSFER:
                    key = "api/user/deposit-naga-v2";
                    break;
                default:
                    break;
            }
            var client = new RestClient(FundistHelper.ClientConfigs.BaseUrlPortal);
            var request = new RestRequest(key, Method.POST)
                //.AddHeader("Authorization", bearer)
                .AddHeader("Accept", "*/*")
                .AddHeader("Content-Type", "application/json")
                .AddJsonBody(jsonObject);
            var response = client.Execute(request);
            return response;
        }

        public IRestResponse SDKPortal_SendTxhash(object jsonObject, string jwtToken)
        {
            var bearer = $"Bearer {jwtToken}";
            var baseUrl = FundistHelper.ClientConfigs.BaseUrlPortal;
            var client = new RestClient(baseUrl);
            var request = new RestRequest("api/transaction/deposit-eth", Method.POST)
                .AddHeader("Authorization", bearer)
                .AddHeader("Accept", "*/*")
                .AddHeader("Content-Type", "application/json")
                .AddJsonBody(jsonObject);
            var response = client.Execute(request);
            return response;
        }

        public IRestResponse SDKPortal_WithdrawTransaction(string type, object jsonObject, string jwtToken)
        {
            var bearer = $"Bearer {jwtToken}";
            var typeTrans = "";
            switch (type)
            {
                case "ETH":
                    typeTrans = "api/transaction/withdraw-eth";
                    break;
                case "NAGA":
                    typeTrans = "api/user/transfer-naga";
                    break;
                default:
                    break;
            }
            var baseUrl = FundistHelper.ClientConfigs.BaseUrlPortal;
            var client = new RestClient(baseUrl);
            var request = new RestRequest(typeTrans, Method.POST)
                .AddHeader("Authorization", bearer)
                .AddHeader("Accept", "*/*")
                .AddHeader("Content-Type", "application/json")
                .AddJsonBody(jsonObject);
            var response = client.Execute(request);
            return response;
        }

        public IRestResponse<ConfirmTranModel> SDKPortal_GetByToken(string TransId, string TimeStamp)
        {
            var obj = new { transId = TransId, timeStamp = TimeStamp };
            var baseUrl = FundistHelper.ClientConfigs.BaseUrlPortal;
            var client = new RestClient(baseUrl);
            var request = new RestRequest("api/transaction/get-by-token", Method.POST)
                .AddHeader("Accept", "*/*")
                .AddHeader("Content-Type", "application/json")
                .AddJsonBody(obj);
            var response = client.Execute<ConfirmTranModel>(request);
            return response;
        }

        public string SDKPortal_HistoryUserIns(string type, object jsonObject, string jwtToken)
        {
            var bearer = $"Bearer {jwtToken}";
            var baseUrl = FundistHelper.ClientConfigs.BaseUrlPortal;
            var client = new RestClient(baseUrl);
            var request = new RestRequest("api/transaction/history", Method.POST)
                .AddHeader("Authorization", bearer)
                .AddHeader("Accept", "*/*")
                .AddHeader("Content-Type", "application/json")
                .AddJsonBody(jsonObject);
            var resposne = client.Execute(request);
            return resposne.Content;
        }
    }
}
