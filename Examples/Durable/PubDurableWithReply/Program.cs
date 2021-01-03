using System;
using Quant;
using Quant.Extensions;
using Serilog.Formatting.Elasticsearch;

namespace PubDurableWithReply
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("PubDurableWithReply start");

                IQuantObserver<QuantMessage> c = new QuantClient().RouteQueue();

                var s = c.Subscribe("foo.chanel.rep", c.ConnectionOption.StanSubscriptionOptions, (obj, arg) =>
                {
                    QuantMessage m = arg.Message.Data.DeserializeMsg<QuantMessage>();

                    var a = m.GetData<MyMsg>("MyMsg");

                    Console.WriteLine("Received a message as data: {0}", System.Text.Encoding.UTF8.GetString(arg.Message.Data));
                });


                QuantMessage roseModel = new QuantMessage().Init(Guid.NewGuid(), "PubDurableWithRep", "SubReply", "100200");


                int counter = 1;
                while (true)
                {
                    Console.WriteLine("Enter message");

                    MyMsg model = new MyMsg
                    {
                        Id = counter++,
                        Name = Console.ReadLine()
                    };

                    roseModel.Data.Clear();

                    roseModel.AddData(model);

                    c.Publish("foo.chanel", roseModel);
                    c.Publish("foo.chanel.rep.await", roseModel);
                
                }
        }
    }

    [Serializable]
    class MyMsg
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
