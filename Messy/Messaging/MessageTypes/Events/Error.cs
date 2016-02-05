namespace Messy.Messaging.MessageTypes.Events
{
    public class Error : IError
    {
        public string Message { get; }
        public ErrorType Type { get; }

        public Error(string message, ErrorType type)
        {
            Message = message;
            Type = type;
        }
    }
}