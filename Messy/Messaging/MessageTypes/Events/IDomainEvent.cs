using System;

namespace Messy.Messaging.MessageTypes.Events
{
    public interface IDomainEvent : IEvent
    {
        Guid AggregateId { get; set; }
        string AggregateName { get; set; }
        int AggregateVersion { get; set; }
    }
}