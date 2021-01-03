using System;
using Quant;
using Quant.Extensions;

namespace SubDurableWithReply
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("SubQueueWithReply start");

            IQuantObserver<QuantMessage> c = new QuantClient().RouteQueue();

            int counter = 1;

            var s = c.Subscribe("foo.chanel", c.ConnectionOption.StanSubscriptionOptions, (obj, arg) =>
            {
                QuantMessage m = arg.Message.Data.DeserializeMsg<QuantMessage>();

                var a = m.GetData<MyMsg>();

                Console.WriteLine("Received a message as data: {0}", System.Text.Encoding.UTF8.GetString(arg.Message.Data));
            });

            QuantMessage roseModel = new QuantMessage().Init(Guid.NewGuid(), "SubQueueWithRep", "PubReply", "200000");


            while (true)
            {

                Console.WriteLine("Enter message");

                MyMsg model = new MyMsg
                {
                    Id = counter++,
                    Name = Console.ReadLine()
                };

                roseModel.AddOrUpdateData(model);

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
