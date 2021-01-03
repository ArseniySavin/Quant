using System;
using Quant;
using Quant.Extensions;

namespace PubDurableWithAwait
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("PubDurableWithAwait start");

            IQuantObserver<QuantMessage> c = new QuantClient().RouteQueue();

            QuantMessage m;

            var s = c.Subscribe("foo.chanel.rep.await", c.ConnectionOption.StanSubscriptionOptions, (obj, arg) =>
            {
                m = arg.Message.Data.DeserializeMsg<QuantMessage>();

                var a = m.GetData<MyMsg>("MyMsg");

                m.AddOrUpdateCorrelation($"SYSTEM_N_100", "9003399");

                c.PublishAwait("foo.chanel.await", m);

                c.Publish("foo.chanel.rep", m);

                Console.WriteLine("Received a message as data: {0}", System.Text.Encoding.UTF8.GetString(arg.Message.Data));
            });
        }
    }

    [Serializable]
    class MyMsg
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
