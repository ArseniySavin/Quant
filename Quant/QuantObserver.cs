using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using STAN.Client;
using Bis.Logger.Extensions;

namespace Quant
{
    internal class QuantObserver<T> : IQuantObserver<T>
    {
        //IQuantObserver _obs;
        IQuantConfig _config;
        QuantConnectionOption _options;
        StanOptions _stanOptions;
        StanSubscriptionOptions _stanSubscriptionOptions;
        IStanConnection _stanClient;
        IStanSubscription _stanSubscription;
        ILogger _log;
        public QuantObserver(IQuantConfig config, ILogger log)
        {
            try
            {
                _log = log;
                _config = config;
                _options = _config.Options();
                _stanOptions = _options.StanOptions;
                InitStanClient();
            }
            catch(StanConnectionException e)
            {
                _log.Critical(e, e.Message);

                // To need for writ log
                System.Threading.Thread.Sleep(2000);
                throw;

            }
            catch (Exception e)
            {
                _log.Critical(e, e.Message);

                // To need for writ log
                System.Threading.Thread.Sleep(2000);
                throw;
            }
        }

        void InitStanClient()
        {
            var stanConnectionFactory = new StanConnectionFactory();
            _stanClient = stanConnectionFactory.CreateConnection(_options.ClusterId, _options.ClientId, _stanOptions);

            _stanSubscriptionOptions = StanSubscriptionOptions.GetDefaultOptions();
            _stanSubscriptionOptions.DurableName = _options.StanSubscriptionOptions.DurableName;
        }

        public IStanSubscription StanSubscription => _stanSubscription;

        public QuantConnectionOption ConnectionOption => _options;

        public void Dispose()
        {
            _stanClient.Dispose();
        }

        public void Publish(string subject, T data)
        {
            _stanClient.Publish(subject, Encoding.UTF8.GetBytes(JsonSerializer.Serialize<T>(data)));
        }

        public void Publish(string subject, T data, EventHandler<StanAckHandlerArgs> handler)
        {
            _stanClient.Publish(subject, Encoding.UTF8.GetBytes(JsonSerializer.Serialize<T>(data)), handler);
        }

        public async Task<string> PublishAsync(string subject, T data)
        {

            return await _stanClient.PublishAsync(subject, Encoding.UTF8.GetBytes(JsonSerializer.Serialize<T>(data)));

        }

        public IStanSubscription Subscribe(string subject, EventHandler<StanMsgHandlerArgs> handler)
        {

            return _stanSubscription = _stanClient.Subscribe(subject, handler);


        }

        public IStanSubscription Subscribe(string subject, StanSubscriptionOptions options, EventHandler<StanMsgHandlerArgs> handler)
        {

            return _stanSubscription = _stanClient.Subscribe(subject, options, handler);

        }

        public IStanSubscription SubscribeQueue(string subject, EventHandler<StanMsgHandlerArgs> handler)
        {

            if (_options.StanSubscriptionOptions.DurableName != _options.QueueGroup)
                throw new QuantQueueGroupNotEqualsException($"DurableName {_options.StanSubscriptionOptions.DurableName} and QueueGroup {_options.QueueGroup} not equals");

            return _stanSubscription = _stanClient.Subscribe(subject, _options.QueueGroup, handler);

        }

        public IStanSubscription SubscribeQueue(string subject, string qgroup, StanSubscriptionOptions options, EventHandler<StanMsgHandlerArgs> handler)
        {

            if (_options.StanSubscriptionOptions.DurableName != qgroup)
                throw new QuantQueueGroupNotEqualsException($"DurableName {_options.QueueGroup} and QueueGroup {qgroup} not equals");


            return _stanSubscription = _stanClient.Subscribe(subject, qgroup, options, handler);

        }

        public void PublishAwait(string subject, T data)
        {
            return;
        }

        public void PublishError(string subject, T data)
        {
            return;
        }
    }
}
