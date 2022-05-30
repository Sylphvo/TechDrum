namespace TechDrum.Core.Models.History
{
    public class CancelViewModel
    {
        public string Id { get; set; }
        public string Receiver { get; set; }
        public decimal Amount { get; set; }
        public decimal Fee { get; set; }
        public string Type { get; set; }
    }
}
