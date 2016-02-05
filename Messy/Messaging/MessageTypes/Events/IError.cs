namespace Messy.Messaging.MessageTypes.Events
{
    public interface IError : IEvent
    {
        string Message { get; }

        ErrorType Type { get; }
    }
}