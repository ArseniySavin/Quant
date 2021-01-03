using System;
using System.Collections.Generic;

namespace Quant.DB.CLI.Models
{
    public partial class TransactionsHistory
    {
        public long TransactionsHistoryId { get; set; }
        public long TransactionIdRef { get; set; }
        public string ClasterId { get; set; }
        public string App { get; set; }
        public string Chanel { get; set; }
        public DateTime? TimeStamp { get; set; }
        public long PayloadIdRef { get; set; }

        public virtual Payload PayloadIdRefNavigation { get; set; }
        public virtual Transactions TransactionIdRefNavigation { get; set; }
    }
}
