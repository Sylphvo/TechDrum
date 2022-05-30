namespace TechDrum.Core.Models.User
{
    public class WithdrawModel
    {
        public string userId { get; set; }
        public string username { get; set; }
        public string timeStamp { get; set; }
        public string blockchain { get; set; }
        public decimal amount { get; set; }
        public string addressTo { get; set; }
        public string ip { get; set; }
        public string token { get; set; }
        public string code { get; set; }

    }
}
