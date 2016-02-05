using Messy.Messaging.MessageTypes;

namespace Messy.Messaging
{
    public interface IValidationService
    {
        bool Validate<T>(T message) where T : IMessage;
    }
}
