using TechDrum.Core.Utils;
using RestSharp;

namespace TechDrum.Core.Commons
{
    public class SDKSsoCommon
    {
        /// <summary>
        /// Account Register Single Sign On
        /// </summary>
        /// <param name="jsonObject"></param>
        /// <returns></returns>
        public static string SDKSso_AccountRegister(object jsonObject)
        {
            var client = new RestClient(FundistHelper.ClientConfigs.BaseUrlSso);
            var request = new RestRequest("Accounts/register", Method.POST)
                .AddHeader("Accept", "*/*")
                .AddHeader("Content-Type", "application/json")
                .AddJsonBody(jsonObject);
            var response = client.Execute(request);
            return response.Content;
        }
    }
}
