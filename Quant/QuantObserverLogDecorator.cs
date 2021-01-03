using System;
using System.Linq;
using System.Threading.Tasks;
using STAN.Client;
using Microsoft.Extensions.Logging;
using Bis.Logger.Extensions;
using Quant.Extensions;

namespace Quant
{
    internal class QuantObserverLogDecorator<T> : IQuantObserver<T>
    {
        IQuantObserver<T> _obs;
        ILogger _log;

        internal QuantObserverLogDecorator(IQuantObserver<T> obs, ILogger log)
        {
            _obs = obs;
            _log = log;
        }

        public IStanSubscription StanSubscription => _obs.StanSubscription;

        public QuantConnectionOption ConnectionOption => _obs.ConnectionOption;

        public void Dispose()
        {
            _obs.Dispose();
        }


        public void Publish(string subject, T data)
        {
            try
            {
                _log.Trace(new { direction = "Publisher", subject = subject, data = data });

                _obs.Publish(subject, data);
            }
            catch(Exception e)
            {
                _log.Error(e, e.Message, data);
                // To need for writ log
                System.Threading.Thread.Sleep(2000);
                throw;
            }
        }

        public void Publish(string subject, T data, EventHandler<StanAckHandlerArgs> handler)
        {
            try
            {
                _log.Trace(new { direction = "Publisher", subject = subject, data = data });

                if (handler.GetInvocationList().Any(m => m.Method.Name != "LogAckHandler"))
                    handler += LogAckHandler;

                _obs.Publish(subject, data, handler);
            }
            catch(Exception e)
            {
                _log.Error(e, e.Message, data);
                throw;
            }
        }

        public async Task<string> PublishAsync(string subject, T data)
        {
            try
            { 
                _log.Trace(new { direction = "Publisher", subject = subject, data = data });

                return await _obs.PublishAsync(subject, data);
            }
            catch (Exception e)
            {
                _log.Error(e, e.Message, data);
                throw;
            }
        }

        public IStanSubscription Subscribe(string subject, EventHandler<StanMsgHandlerArgs> handler)
        {
            try
            {
                if (handler.GetInvocationList().Any(m => m.Method.Name != "LogMsgHandler"))
                    handler += LogMsgHandler;

                return _obs.Subscribe(subject, handler);
            }
            catch (Exception e)
            {
                _log.Error(e, e.Message);
                throw;
            }
        }

        public IStanSubscription Subscribe(string subject, StanSubscriptionOptions options, EventHandler<StanMsgHandlerArgs> handler)
        {
            try
            {
                _log.Trace(new { direction = "Subscriber", subject = subject, options = options.ToString()});

                if (handler.GetInvocationList().Any(m => m.Method.Name != "LogMsgHandler"))
                    handler += LogMsgHandler;

                return _obs.Subscribe(subject, options, handler);
            }
            catch (Exception e)
            {
                _log.Error(e, e.Message);
                throw;
            }
        }

        public IStanSubscription SubscribeQueue(string subject, EventHandler<StanMsgHandlerArgs> handler)
        {
            try
            {
                if (handler.GetInvocationList().Any(m => m.Method.Name != "LogMsgHandler"))
                    handler += LogMsgHandler;

                return _obs.SubscribeQueue(subject, handler);
            }
            catch (Exception e)
            {
                _log.Error(e, e.Message);
                throw;
            }
        }

        public IStanSubscription SubscribeQueue(string subject, string qgroup, StanSubscriptionOptions options, EventHandler<StanMsgHandlerArgs> handler)
        {
            try
            {
                _log.Trace(new { direction = "Subscriber", subject = subject, qgroup = qgroup, options = options.ToString() });

                if (handler.GetInvocationList().Any(m => m.Method.Name != "LogMsgHandler"))
                    handler += LogMsgHandler;

                return _obs.SubscribeQueue(subject, qgroup, options, handler);
            }
            catch (Exception e)
            {
                _log.Error(e, e.Message);
                throw;
            }
        }

        void LogMsgHandler(object obj, StanMsgHandlerArgs handler)
        {
            _log.Trace(new { direction = "Subscriber", subject = handler.Message.Subject, data = System.Text.Encoding.UTF8.GetString(handler.Message.Data) });

        }

        void LogAckHandler(object obj, StanAckHandlerArgs handler)
        {
            _log.Trace(handler.Error, handler.GUID.ToString());
        }

        public void PublishAwait(string subject, T data)
        {
            _obs.PublishAwait(subject, data);
        }

        public void PublishError(string subject, T data)
        {
            _obs.PublishError(subject, data);
        }
    }
}
