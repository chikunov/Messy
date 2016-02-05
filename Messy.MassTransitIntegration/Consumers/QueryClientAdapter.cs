using System.Threading.Tasks;
using MassTransit;
using Messy.Messaging;
using Messy.Messaging.MessageTypes;

namespace Messy.MassTransitIntegration.Consumers
{
    public class QueryClientAdapter<TQuery, TResponse> : IQueryClient<TQuery, TResponse>
        where TQuery : class, IQuery where TResponse : class, IMessage
    {
        private readonly IRequestClient<TQuery, TResponse> _requestClient;

        public QueryClientAdapter(IRequestClient<TQuery, TResponse> requestClient)
        {
            _requestClient = requestClient;
        }

        public Task<TResponse> Query(TQuery query)
        {
            return _requestClient.Request(query);
        }
    }
}