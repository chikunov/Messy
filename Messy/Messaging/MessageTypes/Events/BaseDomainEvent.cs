using System;

namespace Messy.Messaging.MessageTypes.Events
{
    public abstract class BaseDomainEvent : IDomainEvent
    {
        public Guid AggregateId { get; set; }
        public string AggregateName { get; set; }
        public int AggregateVersion { get; set; }
    }
}