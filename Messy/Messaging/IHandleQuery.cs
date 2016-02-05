using System.Threading.Tasks;
using Messy.Messaging.MessageTypes;

namespace Messy.Messaging
{
    public interface IHandleQuery<TQuery, TResponse> where TQuery : class, IQuery where TResponse : class, IResponse
    {
        Task<TResponse> Handle(TQuery query);
    }
}