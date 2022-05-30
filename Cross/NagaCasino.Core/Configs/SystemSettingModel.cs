using Microsoft.Extensions.Configuration;

namespace TechDrum.Core.Configs
{
    public class SystemSettingModel
    {
        public static SystemSettingModel Instance { get; set; }

        public static IConfiguration Configs { get; set; }
        public string Domain { get; set; }
        public string Sentry { get; set; }

    }
    public class FundistSettingModel
    {
        public static FundistSettingModel Instance { get; set; }
        public string BaseUrl { get; set; }
        public string AppKey { get; set; }
        public string AppSecret { get; set; }
        public string SystemCode { get; set; }
        public string Currency { get; set; }
        public string Language { get; set; }
    }
  
    public class ClientSettingModel
    {
        public static ClientSettingModel Instance { get; set; }
        public string BaseUrlIp { get; set; }
        public string BaseUrlRegist { get; set; }
        public string BaseUrlPortal { get; set; }
        public string ETHrating { get; set; }
        public string BaseUrlSso { get; set; }
    }
}
