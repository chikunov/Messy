using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Messy.DDD;
using Messy.Messaging;
using Messy.Messaging.MessageTypes.Events;

namespace Messy.MassTransitIntegration
{
    public class MassTransitDomainEventsDispatcher : IDomainEventsDispatcher
    {
        private readonly Func<IMessageBus> _messageBusProvider;

        public MassTransitDomainEventsDispatcher(Func<IMessageBus> messageBusProvider)
        {
            _messageBusProvider = messageBusProvider;
        }

        public async Task Dispatch(ICollection<DomainEventRecord> events)
        {
            var orderedEvents = events.OrderBy(t => t.DateTime).ToArray();

            var bus = _messageBusProvider();

            foreach (var domainEvent in orderedEvents)
            {
                var data = (IDomainEvent) domainEvent.Data;
                data.AggregateId = domainEvent.AggregateId;
                data.AggregateName = domainEvent.AggregateName;
                data.AggregateVersion = domainEvent.Version;

                await bus.Publish((dynamic) domainEvent.Data);
            }
        }
    }
}