using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using STAN.Client;
using Bis.Logger.Extensions;
using System.Linq;

namespace Quant
{
    internal class QuantObserverRepoDecorator<T> : IQuantObserver<T> where T : QuantMessage
    {
        IQuantObserver<T> _obs;
        ILogger _log;
        IQuantRepository<T> _repo;

        internal QuantObserverRepoDecorator(IQuantObserver<T> obs, ILogger log, IQuantRepository<T> repo)
        {
            try
            { 
                _obs = obs;
                _log = log;
                _repo = repo;
            }
            catch (Exception e)
            {
                _log.Critical(e, e.Message);

                // To need for writ log
                System.Threading.Thread.Sleep(2000);
                throw;
            }
        }

        public IStanSubscription StanSubscription => _obs.StanSubscription;

        public QuantConnectionOption ConnectionOption => _obs.ConnectionOption;


        public void Dispose()
        {
            _obs.Dispose();
        }

        public void Publish(string subject, T data)
        {
            // init
            data.TransactionInfo.TransactionId = AddTransaction(subject, data);

            // update
            AddPayloadAndHistory(subject, data, data.TransactionInfo.TransactionId);

            _obs.Publish(subject, data);
        }

        public void Publish(string subject, T data, EventHandler<StanAckHandlerArgs> handler)
        {

            // init
            data.TransactionInfo.TransactionId = AddTransaction(subject, data);

            // update
            AddPayloadAndHistory(subject, data, data.TransactionInfo.TransactionId);

            _obs.Publish(subject, data, handler);
        }

        public async Task<string> PublishAsync(string subject, T data)
        {
            // init
            data.TransactionInfo.TransactionId = AddTransaction(subject, data);

            // update
            AddPayloadAndHistory(subject, data, data.TransactionInfo.TransactionId);

            return await _obs.PublishAsync(subject, data);

        }

        public void PublishAwait(string subject, T data)
        {
            data.Call.Status = "AWAIT";
            data.Call.NextCall = DateTime.Now.AddMinutes(15);
            data.Call.RetryCount += 1;

            if (data.Call.RetryCount > 20)
                throw new QuantCallRetryCountException($"Retry count was finishes 20");

            // correlation
            AddCorellation(subject, data, data.TransactionInfo.TransactionId);

            _obs.PublishAwait(subject, data);
        }

        public void PublishError(string subject, T data)
        {
            data.Call.Status = "ERROR";

            _obs.PublishError(subject, data);
        }

        public IStanSubscription Subscribe(string subject, EventHandler<StanMsgHandlerArgs> handler)
        {

            return _obs.Subscribe(subject, handler);

        }

        public IStanSubscription Subscribe(string subject, StanSubscriptionOptions options, EventHandler<StanMsgHandlerArgs> handler)
        {

            return _obs.Subscribe(subject, options, handler);

        }

        public IStanSubscription SubscribeQueue(string subject, EventHandler<StanMsgHandlerArgs> handler)
        {

            return _obs.SubscribeQueue(subject, handler);

        }

        public IStanSubscription SubscribeQueue(string subject, string qgroup, StanSubscriptionOptions options, EventHandler<StanMsgHandlerArgs> handler)
        {
            return _obs.SubscribeQueue(subject, qgroup, options, handler);
        }

        private void AddCorellation(string subject, T data, long tranId)
        {
            if (data.Correlations != null && data.Correlations.Any(m => !m.WasExecut))
            {
                var corellation = data.Correlations.LastOrDefault(m => !m.WasExecut);

                if (_repo.ExistsTranAwait(corellation.Reference, corellation.System))
                    return;

                var payLoadId = _repo.AddPayload(data);
                _repo.AddTranAwait(subject, tranId, payLoadId, data);
                var historyId = _repo.AddTranHistory(tranId, payLoadId, _obs.ConnectionOption.ClusterId, _obs.ConnectionOption.ClientId, subject, DateTime.Now, data);

            }
        }

        private void AddPayloadAndHistory(string subject, T data, long tranId)
        {
            if (tranId != 0)
            {
                var payLoadId = _repo.AddPayload(data);
                var historyId = _repo.AddTranHistory(tranId, payLoadId, _obs.ConnectionOption.ClusterId, _obs.ConnectionOption.ClientId, subject, DateTime.Now, data);

            }
        }

        private long AddTransaction(string subject, T data)
        {
            long tranId = data.TransactionInfo.TransactionId;
            long payLoadId = 0;

            if (tranId == 0)
            {
                tranId = _repo.AddTran(data);
                payLoadId = _repo.AddPayload(data);

                _repo.AddTranHistory(tranId, payLoadId, _obs.ConnectionOption.ClusterId, _obs.ConnectionOption.ClientId, subject, DateTime.Now, data);
            }

            return tranId;
        }

        private void AdddError(string subject, T data, long tranId)
        {
            var payLoadId = _repo.AddPayload(data);
            _repo.AddTranAwait(subject, tranId, payLoadId, data);
            var historyId = _repo.AddTranHistory(tranId, payLoadId, _obs.ConnectionOption.ClusterId, _obs.ConnectionOption.ClientId, subject, DateTime.Now, data);

        }
    }

}
