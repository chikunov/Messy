using System;
using System.Collections.Generic;

namespace Messy.DDD
{
    public interface IAggregate
    {
        Guid Id { get; }
        int Version { get; }
        void ApplyEvent(object @event);
        void ClearUncommitted();

        ICollection<DomainEventRecord> GetUncommittedEvents();
    }
}