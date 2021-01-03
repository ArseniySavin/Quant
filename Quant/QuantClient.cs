using System;
using Bis.Logger;
using Microsoft.Extensions.Logging;
using Bis.Logger.Extensions;


namespace Quant
{
    public class QuantClient
    {
        IQuantConfig _config;
        ILogger _loger;
        IQuantRepository<QuantMessage> _repo = new QuantRepository(new DB.CLI.Models.QuantContext()).Instance;

        public QuantClient()
        {
            _config = new QuantConfig();
            _loger = Bis.Logger.LoggerFactory.FromLoggerConfiguration(_config.AppsettingConfig, System.Guid.NewGuid());

           // Serilog.Debugging.SelfLog.Enable(e =>
           //{

           //});
        }
        /// <summary>
        /// For stream work with RES flow
        /// </summary>
        /// <returns>IQuantObserver</returns>
        public IQuantObserver<QuantMessage> RouteQueue()
        {
            IQuantObserver<QuantMessage> observer = null;

            try
            {
                observer = new QuantObserverLogDecorator<QuantMessage>(
                            new QuantObserverRepoDecorator<QuantMessage>(
                            new QuantObserver<QuantMessage>(_config, _loger), _loger, _repo), _loger);
            }
            catch(QuantBaseException e)
            {
                _loger.Error(e, e.Message);
            }
            catch (Exception e)
            {
                _loger.Critical(e, e.Message);
            }
            return observer;
        }

        /// <summary>
        /// For stream work p2p services
        /// </summary>
        /// <returns></returns>
        public IQuantObserver<T> P2PQueue<T>()
        {
            IQuantObserver<T> observer = null;

            try
            {
                observer = new QuantObserverLogDecorator<T>(
                            new QuantObserver<T>(_config, _loger), _loger);
            }
            catch (QuantBaseException e)
            {
                _loger.Error(e, e.Message);
            }
            catch (Exception e)
            {
                _loger.Critical(e, e.Message);
            }
            return observer;
        }

        public QuantMessage PopAwaits(string subject)
        {
            return _repo.GetTranAwait(subject, "AWAIT");
        }

        public QuantMessage PopErrors(string subject)
        {
            return _repo.GetTranAwait(subject, "ERROR");
        }
    }


}
