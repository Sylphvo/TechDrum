using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace TechDrum.Core.Models.User
{
    public class UserHistoryModel
    {
        [DisplayName("Code")]
        public string TransactionId { get; set; }
        [DisplayName("Week")]
        public int Week { get; set; }
        [DisplayName("Time")]
        public string Time { get; set; }
        [DisplayName("AddressFrom")]
        public string From { get; set; }
        [DisplayName("AddressTo")]
        public string To { get; set; }
        [DisplayName("TxHash")]
        public string TxHash { get; set; }
        [DisplayName("Amount")]
        public double Amount { get; set; }
        [DisplayName("Fee")]
        public double Fee { get; set; }
        [DisplayName("Status")]
        public string Status { get; set; }
        [DisplayName("Expired")]
        public DateTimeOffset? Expired { get; set; }
        [DisplayName("Resend")]
        public int Resend { get; set; }
        public string Levels { get; set; }
    }
    public class AdditionalData
    {
    }

    public class Root
    {
        public ICollection<UserHistoryModel> data { get; set; }
        public int totalRecord { get; set; }
    }
}
