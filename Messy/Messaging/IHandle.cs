using System.Threading.Tasks;
using Messy.Messaging.MessageTypes;

namespace Messy.Messaging
{
    public interface IHandle<TMessage> where TMessage : class, IMessage
    {
        Task Handle(TMessage message);
    }
}