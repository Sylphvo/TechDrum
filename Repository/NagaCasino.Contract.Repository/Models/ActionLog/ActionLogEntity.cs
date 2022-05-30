namespace TechDrum.Contract.Repository.Models.ActionLog
{
    public class ActionLogEntity:Entity
    {
        public string Login { get; set; }
        public string LoginIp { get; set; }
        public string Device { get; set; }
        public string Browser { get; set; }
        public string Position { get; set; }
        public string ThreeLetterWindowsLanguageName { get; set; }
        public string ThreeLetterISOLanguageName { get; set; }
        public string NativeName { get; set; }
        public string Name { get; set; }
        public string LCID { get; set; }
        public string KeyboardLayoutId { get; set; }
        public string IetfLanguageTag { get; set; }
        public string EnglishName { get; set; }
        public string TwoLetterISOLanguageName { get; set; }
    }
}
