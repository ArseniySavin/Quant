using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Quant;
using STAN.Client;

namespace Quant
{
    public interface IQuantObserver<T> : IDisposable
    {
        IStanSubscription StanSubscription { get; }

        QuantConnectionOption ConnectionOption { get; }
        void Publish(string subject, T data);
        Task<string> PublishAsync(string subject, T data);
        void Publish(string subject, T data, EventHandler<StanAckHandlerArgs> handler);
        IStanSubscription Subscribe(string subject, EventHandler<StanMsgHandlerArgs> handler);
        IStanSubscription Subscribe(string subject, StanSubscriptionOptions options, EventHandler<StanMsgHandlerArgs> handler);
        IStanSubscription SubscribeQueue(string subject, EventHandler<StanMsgHandlerArgs> handler);
        IStanSubscription SubscribeQueue(string subject, string qgroup, StanSubscriptionOptions options, EventHandler<StanMsgHandlerArgs> handler);
        void PublishAwait(string subject, T data);
        void PublishError(string subject, T data);
    }
}
