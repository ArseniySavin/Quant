using System;
using Quant;
using Quant.Extensions;

namespace SubQueue
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("SubQueue start");

            IQuantObserver<QuantMessage> c = new QuantClient().RouteQueue();

            var s = c.SubscribeQueue("foo.chanel", (obj, arg) =>
            {

                QuantMessage m = arg.Message.Data.DeserializeMsg<QuantMessage>();

                var a = m.GetData<MyMsg>();

                Console.WriteLine("Received a message as data: {0}", System.Text.Encoding.UTF8.GetString(arg.Message.Data));

            });

            Console.ReadKey();
        }
    }

    [Serializable]
    class MyMsg
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
