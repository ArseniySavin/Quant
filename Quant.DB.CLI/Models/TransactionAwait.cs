using System;
using System.Collections.Generic;

namespace Quant.DB.CLI.Models
{
    public partial class TransactionAwait
    {
        public long TransactionAwaitId { get; set; }
        public long TransactionIdRef { get; set; }
        public string CorrelationReference { get; set; }
        public string CorrelationSystem { get; set; }
        public string Status { get; set; }
        public DateTime? NextCall { get; set; }
        public int RetryCount { get; set; }
        public long PayloadIdRef { get; set; }
        public string Chanel { get; set; }

        public virtual Payload PayloadIdRefNavigation { get; set; }
        public virtual Transactions TransactionIdRefNavigation { get; set; }
    }
}
