using System;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quant;

namespace Test
{
    [TestClass]
    public class OptionsTest
    {
        [TestMethod]
        public void Options_All_Test()
        {
            IQuantConfig optBuild = new QuantConfig();

            QuantConnectionOption opt = optBuild.Options();

            Assert.AreEqual(opt.ClientId, "TestApp");
            Assert.AreEqual(opt.ClusterId, "hqw6001");
            Assert.AreEqual(opt.StanOptions.NatsURL, "nats://localhost:4222");
            Assert.AreEqual(opt.StanOptions.DiscoverPrefix, "_TEST");
            Assert.AreEqual(opt.StanSubscriptionOptions.DurableName, "RothschildTest-1");

        }

        [TestMethod]
        public void Options_Default_StanOptions_Test()
        {
            IQuantConfig optBuild = new QuantConfig(
                new ConfigurationBuilder()
                    .AddJsonFile($"appsettings.default.json", optional: true)
                    .Build());

            QuantConnectionOption opt = optBuild.Options();

            // Default
            Assert.AreEqual(opt.StanOptions.NatsURL, "nats://localhost:4222");
            Assert.AreEqual(opt.StanOptions.DiscoverPrefix, "_STAN.discover");

            Assert.AreEqual(opt.ClientId, "TestApp");
            Assert.AreEqual(opt.ClusterId, "hqw6001");
            Assert.AreEqual(opt.StanOptions.NatsURL, "nats://localhost:4222");

        }

        [TestMethod]
        public void Options_EmptyConfig_Test()
        {
            try
            {
                IQuantConfig optBuild = new QuantConfig(
                    new ConfigurationBuilder()
                        .AddJsonFile($"appsettings.empty.json", optional: true)
                        .Build());
                QuantConnectionOption opt = optBuild.Options();

            }
            catch (QuantNotSetAppsettingParException e)
            {
                Assert.IsTrue(true);
            }

        }

        [TestMethod]
        public void Options_QueueGroup_Equals_Test()
        {
            try
            {
                IQuantConfig optBuild = new QuantConfig(
                    new ConfigurationBuilder()
                        .AddJsonFile($"appsettings.localhost.json", optional: true)
                        .Build());
                QuantConnectionOption opt = optBuild.Options();

                Assert.AreEqual(opt.QueueGroup, "RothschildTest-1");
                Assert.AreEqual(opt.StanSubscriptionOptions.DurableName, "RothschildTest-1");

            }
            catch (QuantQueueGroupNotEqualsException e)
            {
                Assert.IsTrue(true);
            }
            catch (Exception e)
            {
                Assert.IsTrue(false);
            }

        }
    }
}
