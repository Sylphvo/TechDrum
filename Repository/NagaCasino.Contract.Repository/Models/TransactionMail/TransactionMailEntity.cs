using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechDrum.Contract.Repository.Models.TransactionMail
{
    //[Table("TransactionMail")]
    public class TransactionMailEntity:Entity
    {
        public string TypeMail { get; set; }
        public string Token { get; set; }
        public string Temp { get; set; }
        public string Temp1 { get; set; }
        public string Temp2 { get; set; }
        public string Temp3 { get; set; }
        public string Temp4 { get; set; }
        public string Temp5 { get; set; }
    }
}
