using System;
using System.Threading.Tasks;
using MassTransit;
using Messy.Messaging;
using Messy.Messaging.MessageTypes.Events;

namespace Messy.MassTransitIntegration.Consumers
{
    public class EventConsumerToHandleCheckedAdapter<TEvent> : IConsumer<TEvent> where TEvent : class, IEvent
    {
        protected readonly IHandleChecked<TEvent> MessageHandler;

        public EventConsumerToHandleCheckedAdapter(IHandleChecked<TEvent> messageHandler)
        {
            MessageHandler = messageHandler;
        }

        public async Task Consume(ConsumeContext<TEvent> context)
        {
            var check = await MessageHandler.Check(context.Message);

            if (check)
            {
                await MessageHandler.Handle(context.Message).ConfigureAwait(false);
            }
            else
            {
                await context.Redeliver(TimeSpan.FromSeconds(1)).ConfigureAwait(false);
            }
        }
    }
}