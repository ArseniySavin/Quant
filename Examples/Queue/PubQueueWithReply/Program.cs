using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;
using STAN.Client;
using Quant;
using Quant.Extensions;

namespace PubQueueWithReply
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("PubQueueWithReply start");
            
            IQuantObserver<QuantMessage> c = new QuantClient().RouteQueue();

            var s = c.Subscribe("foo.chanel.rep", c.ConnectionOption.StanSubscriptionOptions, (obj, arg) =>
            {
                QuantMessage m = arg.Message.Data.DeserializeMsg<QuantMessage>();

                var a = m.GetData<MyMsg>("MyMsg");

                Console.WriteLine("Received a message as data: {0}", System.Text.Encoding.UTF8.GetString(arg.Message.Data));
            });

            QuantMessage roseModel = new QuantMessage();


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
