using Invedia.DI.Attributes;
using TechDrum.Contract.Service;
using TechDrum.Core.Constants;
using TechDrum.Core.Utils;
using RestSharp;
using System.Collections.Generic;

namespace TechDrum.Service
{
    [ScopedDependency(ServiceType = typeof(ISDKFundistCommonService))]
    public class SDKFundistCommonService:ISDKFundistCommonService
    {
        //private IMemoryCache _cache;

        //public SDKFundistCommonService(IServiceProvider serviceProvider)
        //{
        //    _cache = serviceProvider.GetRequiredService<IMemoryCache>();
        //}

        public string GetAll(string timestamp)
        {
            var param = new Dictionary<string, string>
            {
                {FundistHelper.TID, timestamp }
            };
            var hash = FundistHelper.HashSum(FundistType.HashFullListGame, param);
            var baseUrl = FundistHelper.FundistConfigs.BaseUrl;
            var client = new RestClient(baseUrl);
            var request = new RestRequest("System/Api/{key}/Game/FullList")
                .AddUrlSegment("key", FundistHelper.FundistConfigs.AppKey)
                .AddParameter("TID", timestamp)
                .AddParameter("Hash", hash);
            var restResponse = client.Post(request);
            return restResponse.Content;
        }
        /// <summary>
        /// Load iframe game
        /// </summary>
        /// <param name="login"></param>
        /// <param name="tid"></param>
        /// <param name="pass"></param>
        /// <param name="userIp"></param>
        /// <param name="pageCode"></param>
        /// <param name="system"></param>
        /// <returns></returns>
        public string SDKFund_Authorization(string login, string tid, string pass, string userIp, string pageCode, string system)
        {
            var param = new Dictionary<string, string>
            {
                {FundistHelper.TID, tid },
                {FundistHelper.LOGIN, login },
                {FundistHelper.PASSUSER, pass }
            };
            var hash = FundistHelper.HashSum(FundistType.HashDirectAuth, param);
            var baseUrl = FundistHelper.FundistConfigs.BaseUrl;
            var client = new RestClient(baseUrl);
            var request = new RestRequest("System/Api/{key}/User/DirectAuth")
                .AddUrlSegment("key", FundistHelper.FundistConfigs.AppKey)
                .AddParameter("Login", login)
                .AddParameter("Password", pass)
                .AddParameter("System", FundistHelper.FundistConfigs.SystemCode)
                .AddParameter("TID", tid)
                .AddParameter("Hash", hash)
                .AddParameter("Page", pageCode)
                .AddParameter("UserIP", userIp)
                .AddParameter("Language", "en")
                .AddParameter("Country", "VN")
                .AddParameter("Currency", "USD");
            var restResponse = client.Post(request);
            return restResponse.Content;
        }

        public string SDKFund_Authorization_Html(string login, string tid, string pass, string userIp, string pageCode, string system)
        {
            var param = new Dictionary<string, string>
            {
                {FundistHelper.TID, tid },
                {FundistHelper.LOGIN, login },
                {FundistHelper.PASSUSER, pass }
            };
            var hash = FundistHelper.HashSum(FundistType.HashAuthHtml, param);
            var baseUrl = FundistHelper.FundistConfigs.BaseUrl;
            var client = new RestClient(baseUrl);
            var request = new RestRequest("System/Api/{key}/User/AuthHTML")
                .AddUrlSegment("key", FundistHelper.FundistConfigs.AppKey)
                .AddParameter("Login", login)
                .AddParameter("Password", pass)
                .AddParameter("System", FundistHelper.FundistConfigs.SystemCode)
                .AddParameter("TID", tid)
                .AddParameter("Hash", hash)
                .AddParameter("Page", pageCode)
                .AddParameter("UserIP", userIp);
            var restResponse = client.Post(request);
            return restResponse.Content;
        }
        /// <summary>
        /// Get Full List Game
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public string SDKFund_FullList(string timestamp)
        {
            var param = new Dictionary<string, string>
            {
                {FundistHelper.TID, timestamp }
            };
            var hash = FundistHelper.HashSum(FundistType.HashFullListGame, param);
            var baseUrl = FundistHelper.FundistConfigs.BaseUrl;
            var client = new RestClient(baseUrl);
            var request = new RestRequest("System/Api/{key}/Game/FullList")
                .AddUrlSegment("key", FundistHelper.FundistConfigs.AppKey)
                .AddParameter("TID", timestamp)
                .AddParameter("Hash", hash);
            var restResponse = client.Post(request);
            return restResponse.Content;
        }
    }
}
