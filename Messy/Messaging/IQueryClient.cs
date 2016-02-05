using System.Threading.Tasks;
using Messy.Messaging.MessageTypes;

namespace Messy.Messaging
{
    public interface IQueryClient<in TQuery, TResponse> where TQuery : class, IQuery where TResponse : class, IMessage
    {
        Task<TResponse> Query(TQuery query);
    }
}