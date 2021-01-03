using System;
using System.Collections.Generic;
using System.Text;
using STAN.Client;

namespace Quant
{
    public class QuantConnectionOption
    {
        public StanOptions StanOptions { get; set; }
        public StanSubscriptionOptions StanSubscriptionOptions { get; set; }
        public string ClusterId { get; set; }
        public string ClientId { get; set; }
        public string QueueGroup { get; set; }
    }
}
