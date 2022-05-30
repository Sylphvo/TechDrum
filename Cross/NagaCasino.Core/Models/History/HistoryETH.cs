namespace TechDrum.Core.Models.History
{
    public class DepositETH
    {
        public string TransactionId { get; set; }
        public string Time { get; set; }
        public decimal AmountFrom { get; set; }
        public decimal Amount { get; set; }
        public string TxHash { get; set; }
        public string Status { get; set; }
        public int Resend { get; set; }
    }
    public class WithdrawETH
    {
        public string TransactionId { get; set; }
        public string Time { get; set; }
        public string AddressTo { get; set; }
        public decimal AmountFrom { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public int Resend { get; set; }
    }
    
}
