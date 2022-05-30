using TechDrum.Core.Models.AuthenSso;
using RestSharp;

namespace TechDrum.Contract.Service
{
    public interface ISDKPortalCommonService
    {
        public IRestResponse SDKPortal_ForgetPassword(object jsonObject);
        public IRestResponse SDKPortal_CreateUser(object jsonObject, string jwtToken);
        public IRestResponse SDKPortal_RollBackTransaction(object jsonObject, string type);
        public string SDKPortal_HistoryUser(string type, object jsonObject, string jwtToken);
        public string SDKPortal_HistoryUserIns(string type, object jsonObject, string jwtToken);
        public string SDKPortal_AffiliateTree(object jsonObject, string jwtToken);
        public IRestResponse SDKPortal_WithdrawTransaction(string type, object jsonObject, string jwtToken);
        public string SDKPortal_PLayerCheckLimit(object jsonObject, string jwtToken);
        public string SDKPortal_PlayerCheckPolicy(object jsonObject, string jwtToken);
        public string SDKPortal_GetAddressCrypto(object jsonObject, string jwtToken);
        public IRestResponse SDKPortal_ResendTransaction(object jsonObject, string jwtToken);
        public IRestResponse SDKPortal_CancelTransaction(object jsonObject, string jwtToken);
        public IRestResponse SDKPortal_VerifyTransaction(object jsonObject, string type, string jwtToken);
        public IRestResponse SDKPortal_SendTxhash(object jsonObject, string jwtToken);
        public IRestResponse<ConfirmTranModel> SDKPortal_GetByToken(string TransId, string TimeStamp);
    }
}
