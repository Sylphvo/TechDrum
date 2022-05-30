using Invedia.Core.SecurityUtils;
using Microsoft.Extensions.Caching.Memory;
using TechDrum.Core.Configs;
using TechDrum.Core.Constants;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace TechDrum.Core.Utils
{
    public class FundistHelper
    {
        private static MemoryCache Cache { get; } = new MemoryCache(new MemoryCacheOptions());
        public static ClientSettingModel ClientConfigs => ClientSettingModel.Instance;
        public static FundistSettingModel FundistConfigs => FundistSettingModel.Instance;
        public const string TID = "TID";
        public const string LOGIN = "USER_NAME";
        public const string PASSUSER = "PASSWORD_USER";

        public static string HashSum(FundistType type, IDictionary<string, string> param)
        {
            var value = "";
            param.TryGetValue(TID, out var tid);
            param.TryGetValue(LOGIN, out var username);
            param.TryGetValue(PASSUSER, out var password);

            var ip = GetIp();

            switch (type)
            {
                case FundistType.HashCreateUser:
                    break;
                case FundistType.HashUpdateUser:
                    break;
                case FundistType.HashSetBalance:
                    break;
                case FundistType.HashGetBalance:
                    break;
                case FundistType.HashGetWithdrawBalance:
                    break;
                case FundistType.HashDirectAuth:
                    value =
                        $"User/DirectAuth/{ip}/{tid}/{FundistConfigs.AppKey}/{username}/{password}/{FundistConfigs.SystemCode}/{FundistConfigs.AppSecret}";
                    break;
                case FundistType.HashAuthHtml:
                    value =
                        $"User/AuthHTML/{ip}/{tid}/{FundistConfigs.AppKey}/{username}/{password}/{FundistConfigs.SystemCode}/{FundistConfigs.AppSecret}";
                    break;
                case FundistType.HashKillAuth:
                    value =
                        $"User/KillAuth/{ip}/{tid}/{FundistConfigs.AppKey}/{username}/{FundistConfigs.AppSecret}";
                    break;
                case FundistType.HashUniverBetsAuth:
                    break;
                case FundistType.HashStatsBetAuth:
                    break;
                case FundistType.HashDetailStateAuth:
                    break;
                case FundistType.HashGameDetail:
                    break;
                case FundistType.HashEnabledUser:
                    break;
                case FundistType.HashFullListGame:
                    value =
                        $"Game/FullList/{ip}/{tid}/{FundistConfigs.AppKey}/{FundistConfigs.AppSecret}";
                    break;
                default:
                    break;
            }

            var hash = SecurityHelper.EncryptMd5(value);
            return hash;
        }

        public static string GetIp()
        {
            Cache.TryGetValue(TempDataKey.ServerIp, out string ip);
            if (!string.IsNullOrWhiteSpace(ip)) return ip;

            var httpClient = new HttpClient();
            ip = httpClient.GetStringAsync(ClientConfigs.BaseUrlIp).Result;

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromHours(1));

            Cache.Set(TempDataKey.ServerIp, ip, cacheEntryOptions);

            return ip;
        }
    }
}