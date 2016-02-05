using System;
using System.Threading.Tasks;
using Messy.Messaging.MessageTypes;
using Messy.Messaging.MessageTypes.Events;

namespace Messy.Messaging
{
    public interface IMessageBus : IDisposable
    {
        Uri GetAddress();
        void Start();
        Task Publish<T>(T @event) where T : class, IEvent;
        Task Send<T>(T command) where T : class, ICommand;

        IQueryClient<TQuery, TResponse> CreateQueryClient<TQuery, TResponse>(Uri address, TimeSpan timeout)
            where TQuery : class, IQuery where TResponse : class, IMessage;
    }
}