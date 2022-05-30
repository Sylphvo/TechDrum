using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechDrum.Contract.Repository.Models.Transaction
{
    //[Table("Transaction")]
    public class TransactionEnity : Entity
    {
        public string UserId { get; set; }
        public string Token { get; set; }
        public string IpRequest { get; set; }
        public string IpConfirm { get; set; }
        public string TimeStamp { get; set; }
        /// <summary>
        /// Thời gian hết hạn token
        /// </summary>
        public DateTimeOffset? ExpirTime { get; set; }
        /// <summary>
        /// Thời gian user confirm mail
        /// </summary>
        public DateTimeOffset? UserConfirmTime { get; set; }
        public string Request { get; set; }
        public string UrlSent { get; set; }
        public string Status { get; set; }
        public string ResponseData { get; set; }
        public string TypeTransaction { get; set; }
        public string Temp { get; set; }
        public string Temp1 { get; set; }
        public string Temp2 { get; set; }
        public string Temp3 { get; set; }
        public string Temp4 { get; set; }
        public string Temp5 { get; set; }
    }
}
