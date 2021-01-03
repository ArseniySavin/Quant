using System;
using System.Collections;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quant;
using Quant.Extensions;

namespace Test
{
    [TestClass]
    public class MessageTest
    {
        [TestMethod]
        public void Add_dublicate_Service_model_Test()
        {
            QuantMessage model = new QuantMessage();
            


            model.AddData<TestModel>(new TestModel
            {
                Id = 100,
                Name = "Test-1"
            });

            model.AddData<TestModel>(new TestModel
            {
                Id = 102,
                Name = "Test-2"
            });


        }

        [TestMethod]
        public void Add_Or_Update_Service_model_Test()
        {
            QuantMessage model = new QuantMessage();

                model.AddOrUpdateData<TestModel>(new TestModel
                {
                    Id = 100,
                    Name = "Test - 1"
                });

                model.AddOrUpdateData<TestModel>(new TestModel
                {
                    Id = 102,
                    Name = "Test - 2"
               });
            Assert.AreEqual(model["TestModel"], "{\"Id\":102,\"Name\":\"Test - 2\"}");


        }

        [TestMethod]
        public void Serialazer_Test()
        {
            QuantMessage model = new QuantMessage();

            model.Call = new Call { NextCall = DateTime.Now, RetryCount = 0, Status = "DONE" };

            model.AddOrUpdateData<TestModel>(new TestModel
            {
                Id = 100,
                Name = "Test - 1"
            });

            model.AddOrUpdateData<TestModel2>(new TestModel2
            {
                Id = 102,
                Name = "Test - 2"
            });

            var ser = model.SerializeMsg();
            var des = ser.DeserializeMsg<QuantMessage>();

            Assert.AreEqual(model["TestModel"], "{\"Id\":102,\"Name\":\"Test - 2\"}");


        }

    }

    [Serializable]
    class TestModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    [Serializable]
    class TestModel2
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
