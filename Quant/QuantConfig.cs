using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using STAN.Client;
using Quant.Extensions;

namespace Quant
{
    /// <summary>
    /// Manage the default StanOptions are using configuration overload out of file 
    /// </summary>
    public interface IQuantConfig
    {
        /// <summary>
        /// Get all options
        /// </summary>
        QuantConnectionOption Options();

        /// <summary>
        /// Getting file configuration of appsettings.json
        /// </summary>
        IConfiguration AppsettingConfig { get; }
    }

    public class QuantConfig : IQuantConfig
    {
        string _enviroment => Environment.GetEnvironmentVariable("Quant", EnvironmentVariableTarget.Machine);
        string _curDir => Directory.GetCurrentDirectory();

        IConfiguration _config;

        public IConfiguration AppsettingConfig => _config;

        /// <summary>
        /// Create default configuration using pattern config file `appsettings.{Enviroment}.json`
        /// </summary>
        public QuantConfig()
        {
            _config = Init();

        }

        /// <summary>
        /// Create configuration by custom config name
        /// </summary>
        /// <param name="config">new ConfigurationBuilder().AddJsonFile(...)</param>
        public QuantConfig(IConfiguration config)
        {
            if (config == null)
                throw new QuantBaseException("IConfiguration is not initial");

            _config = config;
        }

        public QuantConnectionOption Options()
        {
            try
            {
                StanOptions stanOptions = StanOptions.GetDefaultOptions();


                stanOptions.NatsURL            = _config["NATS:StanOption:NatsURL"]        ?? stanOptions.NatsURL;
                stanOptions.DiscoverPrefix     = _config["NATS:StanOption:DiscoverPrefix"] ?? stanOptions.DiscoverPrefix;
                stanOptions.ConnectTimeout     = _config["NATS:StanOption:ConnectTimeout"].ToInt(stanOptions.ConnectTimeout);
                stanOptions.MaxPubAcksInFlight = _config["NATS:StanOption:MaxPubAcksInFlight"].ToLong(stanOptions.MaxPubAcksInFlight);
                stanOptions.PingInterval       = _config["NATS:StanOption:PingInterval"].ToInt(stanOptions.PingInterval);
                stanOptions.PingMaxOutstanding = _config["NATS:StanOption:PingMaxOutstanding"].ToInt(stanOptions.PingMaxOutstanding);
                stanOptions.PubAckWait         = _config["NATS:StanOption:PubAckWait"].ToLong(stanOptions.PubAckWait);


                StanSubscriptionOptions stanSubscriptionOptions = StanSubscriptionOptions.GetDefaultOptions();

                stanSubscriptionOptions.DurableName = _config["NATS:StanSubscriptionOptions:DurableName"] ?? stanSubscriptionOptions.DurableName;
                stanSubscriptionOptions.AckWait     = _config["NATS:StanSubscriptionOptions:AckWait"].ToInt(stanSubscriptionOptions.AckWait);
                stanSubscriptionOptions.MaxInflight = _config["NATS:StanSubscriptionOptions:DurableName"].ToInt(stanSubscriptionOptions.MaxInflight);
                stanSubscriptionOptions.LeaveOpen   = _config["NATS:StanSubscriptionOptions:LeaveOpen"].ToBool(stanSubscriptionOptions.LeaveOpen);
                stanSubscriptionOptions.ManualAcks  = _config["NATS:StanSubscriptionOptions:ManualAcks"].ToBool(stanSubscriptionOptions.ManualAcks);

                return new QuantConnectionOption
                {
                    ClusterId               = _config["NATS:StanConnectionOption:ClusterId"]   ?? throw new QuantNotSetAppsettingParException("Not set StanOption:ClusterId"),
                    ClientId                = _config["NATS:StanConnectionOption:ClientId"]    ?? throw new QuantNotSetAppsettingParException("Not set StanOption:ClientId"),
                    QueueGroup              = _config["NATS:StanSubscriptionOptions:QueueGroup"],
                    StanOptions             = stanOptions,
                    StanSubscriptionOptions = stanSubscriptionOptions

                };
            }
            catch
            {
                throw;
            }
        }

        bool EnvoromentExists
        {
            get
            {
                if (string.IsNullOrEmpty(_enviroment))
                    return false;

                return true;
            }
        }

        bool AppsettingsExists
        {
            get
            {                
                return File.Exists($"appsettings.{_enviroment}.json");
            }
        }

        IConfiguration Init()
        {
            try
            {
                if (!EnvoromentExists)
                    throw new QuantEnviromentNotFoundException("Environment variable Rose not fount or null");

                if (!AppsettingsExists)
                    throw new QuantAppsettingsNotFoundException($"appsettings.{_enviroment}.json not found in {_curDir}");

                return new ConfigurationBuilder()
                    .AddJsonFile($"appsettings.{_enviroment}.json", optional: true)
                    .Build();

            }
            catch (Exception)
            {
                throw;
            }
            
        }
    }
}
