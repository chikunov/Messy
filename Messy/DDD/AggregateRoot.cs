using System;
using System.Collections.Generic;
using Messy.DDD.EventRouters;
using Messy.Messaging.MessageTypes.Events;

namespace Messy.DDD
{
    public abstract class AggregateRoot : Entity, IAggregate
    {
        public int Version { get; protected set; }

        protected IRouteEvents RegisteredRoutes
        {
            get { return _registeredRoutes ?? (_registeredRoutes = new ConventionEventRouter(true, this)); }
            set
            {
                if (value == null)
                {
                    throw new InvalidOperationException("AggregateBase must have an event router to function");
                }

                _registeredRoutes = value;
            }
        }

        private IRouteEvents _registeredRoutes;
        private readonly ICollection<DomainEventRecord> _uncommittedEvents = new LinkedList<DomainEventRecord>();

        protected AggregateRoot() : this(null)
        {
        }

        protected AggregateRoot(IRouteEvents handler)
        {
            if (handler == null)
            {
                return;
            }

            RegisteredRoutes = handler;
            RegisteredRoutes.Register(this);
        }

        void IAggregate.ApplyEvent(object @event)
        {
            RegisteredRoutes.Dispatch(@event);
            Version++;
        }

        ICollection<DomainEventRecord> IAggregate.GetUncommittedEvents()
        {
            return _uncommittedEvents;
        }

        void IAggregate.ClearUncommitted()
        {
            _uncommittedEvents.Clear();
        }

        protected void RaiseEvent(IDomainEvent eventData)
        {
            ((IAggregate) this).ApplyEvent(eventData);

            var aggregateName = GetType().FullName;
            eventData.AggregateId = Id;
            eventData.AggregateName = aggregateName;
            eventData.AggregateVersion = Version;
            var domainEvent = new DomainEventRecord(Id, aggregateName, Version, @eventData.GetType().FullName, DateTime.UtcNow, eventData);
            _uncommittedEvents.Add(domainEvent);
        }

        protected void Register<T>(Action<T> route)
        {
            RegisteredRoutes.Register(route);
        }
    }
}