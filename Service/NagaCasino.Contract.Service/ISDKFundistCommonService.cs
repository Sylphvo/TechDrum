namespace TechDrum.Contract.Service
{
    public interface ISDKFundistCommonService
    {
        public string SDKFund_FullList(string timestamp);
        public string SDKFund_Authorization(string login, string tid, string pass, string userIp, string pageCode, string system);
        public string SDKFund_Authorization_Html(string login, string tid, string pass, string userIp, string pageCode, string system);
    }
}
