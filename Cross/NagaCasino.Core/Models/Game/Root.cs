using Newtonsoft.Json;
using System.Collections.Generic;

namespace TechDrum.Core.Models.Game
{
    public class Root
    {
        public List<Category> categories { get; set; }
        public List<Game> games { get; set; }
        public Merchants merchants { get; set; }
        public CountriesRestrictions countriesRestrictions { get; set; }
        public List<MerchantsCurrency> merchantsCurrencies { get; set; }
    }
    public class Category
    {
        public string ID { get; set; }
        public Trans Trans { get; set; }
        public List<string> Tags { get; set; }
        public Name Name { get; set; }
        public string CSort { get; set; }
        public string CSubSort { get; set; }
        public string Slug { get; set; }
    }
    public class Trans
    {
        public string en { get; set; }
        [JsonProperty("zh-cn")]
        public string ZhCn { get; set; }
        public string de { get; set; }
        public string pt { get; set; }
        [JsonProperty("pt-pt")]
        public string PtPt { get; set; }
        public string sv { get; set; }
        public string b5 { get; set; }
        public string ru { get; set; }
        public string uk { get; set; }
        public string zh { get; set; }
        public string cn { get; set; }
        public string ja { get; set; }
        [JsonProperty("zh-hant")]
        public string ZhHant { get; set; }
        [JsonProperty("zh-hans")]
        public string ZhHans { get; set; }
        public string ko { get; set; }
        public string da { get; set; }
        public string no { get; set; }
        public string th { get; set; }
        public string tr { get; set; }
    }
    public class Name
    {
        public string en { get; set; }
        [JsonProperty("zh-cn")]
        public string ZhCn { get; set; }
        public string de { get; set; }
        public string pt { get; set; }
        [JsonProperty("pt-pt")]
        public string PtPt { get; set; }
        public string sv { get; set; }
        public string b5 { get; set; }
        public string ru { get; set; }
        public string uk { get; set; }
        public string zh { get; set; }
        public string cn { get; set; }
        public string ja { get; set; }
        [JsonProperty("zh-hant")]
        public string ZhHant { get; set; }
        [JsonProperty("zh-hans")]
        public string ZhHans { get; set; }
        public string ko { get; set; }
        public string da { get; set; }
        public string no { get; set; }
        public string th { get; set; }
        public string tr { get; set; }
    }
    public class Game
    {
        public string ID { get; set; }
        public string Image { get; set; }
        public string Url { get; set; }
        public Name Name { get; set; }
        public Description Description { get; set; }
        public string MobileUrl { get; set; }
        public int Branded { get; set; }
        public int SuperBranded { get; set; }
        public int hasDemo { get; set; }
        public List<string> CategoryID { get; set; }
        public object SortPerCategory { get; set; }
        public string MerchantID { get; set; }
        public string SubMerchantID { get; set; }
        public string AR { get; set; }
        public string IDCountryRestriction { get; set; }
        public string Sort { get; set; }
        public string PageCode { get; set; }
        public string MobilePageCode { get; set; }
        public object MobileAndroidPageCode { get; set; }
        public object MobileWindowsPageCode { get; set; }
        public object ExternalCode { get; set; }
        public object MobileExternalCode { get; set; }
        public string ImageFullPath { get; set; }
        public object WorkingHours { get; set; }
        public string IsVirtual { get; set; }
        public string TableID { get; set; }
        public string Freeround { get; set; }
    }
    public class Merchants
    {
        public _998 _998 { get; set; }
        public _980 _980 { get; set; }
    }
    public class CountriesRestrictions
    {
        public _15 _15 { get; set; }
        public _20 _20 { get; set; }
        public _22 _22 { get; set; }
        public _61 _61 { get; set; }
        public _124 _124 { get; set; }
        public _125 _125 { get; set; }
    }

    public class MerchantsCurrency
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string IDMerchant { get; set; }
        public List<string> Currencies { get; set; }
        public string DefaultCurrency { get; set; }
        public string IsDefault { get; set; }
    }
    public class _998
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public object IDParent { get; set; }
        public string Alias { get; set; }
        public string Image { get; set; }
    }

    public class _980
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public object IDParent { get; set; }
        public string Alias { get; set; }
        public string Image { get; set; }
    }
    public class _15
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string IDMerchant { get; set; }
        public List<string> Countries { get; set; }
        public string IsDefault { get; set; }
        public string IDParent { get; set; }
        public string IDApiTemplate { get; set; }
    }

    public class _20
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string IDMerchant { get; set; }
        public List<string> Countries { get; set; }
        public string IsDefault { get; set; }
        public string IDParent { get; set; }
        public string IDApiTemplate { get; set; }
    }

    public class _22
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string IDMerchant { get; set; }
        public List<string> Countries { get; set; }
        public string IsDefault { get; set; }
        public string IDParent { get; set; }
        public string IDApiTemplate { get; set; }
    }

    public class _61
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string IDMerchant { get; set; }
        public List<string> Countries { get; set; }
        public string IsDefault { get; set; }
        public string IDParent { get; set; }
        public string IDApiTemplate { get; set; }
    }

    public class _124
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string IDMerchant { get; set; }
        public List<string> Countries { get; set; }
        public string IsDefault { get; set; }
        public string IDParent { get; set; }
        public string IDApiTemplate { get; set; }
    }

    public class _125
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string IDMerchant { get; set; }
        public List<string> Countries { get; set; }
        public string IsDefault { get; set; }
        public string IDParent { get; set; }
        public string IDApiTemplate { get; set; }
    }
    public class Description
    {
    }
}
