namespace TechDrum.Core.Models.Portal
{
    public class TransactionResendlModel
    {
        public string id { get; set; }
    }
    public class CancelModel
    {
        public string TransId { get; set; }
    }

    public class VerifylModel
    {
        public string ip { get; set; }
        public string transId { get; set; }
        public string token { get; set; }
    }
}
