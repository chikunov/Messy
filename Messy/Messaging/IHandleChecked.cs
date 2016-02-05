using System.Threading.Tasks;
using Messy.Messaging.MessageTypes;

namespace Messy.Messaging
{
    public interface IHandleChecked<TMessage> where TMessage : class, IMessage
    {
        Task Handle(TMessage command);
        Task<bool> Check(TMessage message);
    }
}