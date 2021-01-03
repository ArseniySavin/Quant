using System;
using System.Collections.Generic;

namespace Quant.DB.CLI.Models
{
    public partial class Transactions
    {
        public Transactions()
        {
            TransactionAwait = new HashSet<TransactionAwait>();
            TransactionsHistory = new HashSet<TransactionsHistory>();
        }

        public long TransactionId { get; set; }
        public string TransactionCode { get; set; }
        public Guid InstanceId { get; set; }
        public bool Parent { get; set; }
        public Guid? ParentIdRef { get; set; }
        public string SystemCode { get; set; }
        public string SystemReference { get; set; }

        public virtual ICollection<TransactionAwait> TransactionAwait { get; set; }
        public virtual ICollection<TransactionsHistory> TransactionsHistory { get; set; }
    }
}
