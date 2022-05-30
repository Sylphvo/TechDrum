namespace TechDrum.Core.Models.History
{
    public class DepositNaga
    {
        public string TransactionId { get; set; }
        public string Time { get; set; }
        public string AddressFrom { get; set; }
        public string AddressTo { get; set; }
        public decimal Amount { get; set; }
        public decimal Fee { get; set; }
        public string Status { get; set; }
        public int Resend { get; set; }
    }
    public class TransferNaga
    {
        public string TransactionId { get; set; }
        public string Time { get; set; }
        public string AddressFrom { get; set; }
        public string AddressTo { get; set; }
        public decimal Amount { get; set; }
        public decimal Fee { get; set; }
        public int Resend { get; set; }
    }

}
