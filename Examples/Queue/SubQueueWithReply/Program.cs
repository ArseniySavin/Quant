using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Quant;
using STAN.Client;
using System.Text.Json;
using Quant.Extensions;

namespace SubQueueWithReply
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("SubQueueWithReply start");

            IQuantObserver<QuantMessage> c = new QuantClient().RouteQueue();

            int counter = 1;

            var s = c.SubscribeQueue("foo.chanel", (obj, arg) =>
            {
                QuantMessage m = arg.Message.Data.DeserializeMsg<QuantMessage>();

                var a = m.GetData<MyMsg>();

                Console.WriteLine("Received a message as data: {0}", System.Text.Encoding.UTF8.GetString(arg.Message.Data));
            });

            QuantMessage roseModel = new QuantMessage();


            while (true)
            {

                Console.WriteLine("Enter message");

                MyMsg model = new MyMsg
                {
                    Id = counter++,
                    Name = Console.ReadLine()
                };

                roseModel.AddData(model);

                c.Publish("foo.chanel.rep", roseModel);
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
