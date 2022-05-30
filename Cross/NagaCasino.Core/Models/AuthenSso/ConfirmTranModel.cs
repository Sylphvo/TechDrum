using System;

namespace TechDrum.Core.Models.AuthenSso
{
    public class ConfirmTranModel
    {
        public int totalRecord { get; set; }
        public Data data { get; set; }
        public Tokens token { get; set; }
        public string strToken { get; set; }

    }
    public class Data
    {
        public string type { get; set; }
        public double fee { get; set; }
        public double amountExecuted { get; set; }
        public string toAddress { get; set; }
        public string status { get; set; }
        public DateTimeOffset createdTime { get; set; }

    }
    public class Tokens
    {
        public string transId { get; set; }
        public string token { get; set; }
        public string ip { get; set; }
        public string timeStamp { get; set; }
    }
}
