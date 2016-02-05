using System;
using System.Threading.Tasks;
using MassTransit;
using Messy.Messaging;
using Messy.Messaging.MessageTypes;

namespace Messy.MassTransitIntegration.Consumers
{
    public class CommandConsumerToHandleCheckedAdapter<TCommand> : IConsumer<TCommand> where TCommand : class, ICommand
    {
        protected readonly IHandleChecked<TCommand> MessageHandler;

        public CommandConsumerToHandleCheckedAdapter(IHandleChecked<TCommand> messageHandler)
        {
            MessageHandler = messageHandler;
        }

        public async Task Consume(ConsumeContext<TCommand> context)
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