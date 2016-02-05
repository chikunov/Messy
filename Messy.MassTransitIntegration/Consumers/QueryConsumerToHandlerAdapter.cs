using System.Threading.Tasks;
using MassTransit;
using Messy.Messaging;
using Messy.Messaging.MessageTypes;

namespace Messy.MassTransitIntegration.Consumers
{
    public class QueryConsumerToHandlerAdapter<TQuery, TResponse> : IConsumer<TQuery>
        where TQuery : class, IQuery where TResponse : class, IResponse
    {
        private readonly IHandleQuery<TQuery, TResponse> _queryHandler;

        public QueryConsumerToHandlerAdapter(IHandleQuery<TQuery, TResponse> queryHandler)
        {
            _queryHandler = queryHandler;
        }

        public async Task Consume(ConsumeContext<TQuery> context)
        {
            var response = await _queryHandler.Handle(context.Message);

            context.Respond(response);
        }
    }
}