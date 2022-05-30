namespace TechDrum.Core.Models.History
{

    public class Commission
    {
        public string TransactionId { get; set; }
        public string Time { get; set; }
        public decimal Amount { get; set; }   
        public string Status { get; set; }
        public int Resend { get; set; }
    }
}
