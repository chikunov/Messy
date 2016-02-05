using System.Threading.Tasks;
using Messy.Messaging;
using Messy.Messaging.MessageTypes;

namespace Messy.MassTransitIntegration.Consumers
{
    public class HandleToHandleCheckedAdapter<TMessage> : 
        IHandleChecked<TMessage> where TMessage : class, IMessage
    {
        private readonly IHandle<TMessage> _handler;

        public HandleToHandleCheckedAdapter(IHandle<TMessage> handler)
        {
            _handler = handler;
        }

        public Task Handle(TMessage command)
        {
            return _handler.Handle(command);
        }

        public Task<bool> Check(TMessage message)
        {
            return Task.FromResult(true);
        }
    }
}