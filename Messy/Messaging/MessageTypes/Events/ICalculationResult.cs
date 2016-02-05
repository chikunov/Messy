using System;

namespace Messy.Messaging.MessageTypes.Events
{
    public interface ICalculationResult : IEvent
    {
        Guid CalculationId { get; }
    }
}