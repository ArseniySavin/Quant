using System;
using System.Collections.Generic;

namespace Quant.DB.CLI.Models
{
    public partial class Payload
    {
        public Payload()
        {
            TransactionAwait = new HashSet<TransactionAwait>();
            TransactionsHistory = new HashSet<TransactionsHistory>();
        }

        public long PayloadId { get; set; }
        public string Content { get; set; }
        public DateTime? TimeStamp { get; set; }

        public virtual ICollection<TransactionAwait> TransactionAwait { get; set; }
        public virtual ICollection<TransactionsHistory> TransactionsHistory { get; set; }
    }
}
