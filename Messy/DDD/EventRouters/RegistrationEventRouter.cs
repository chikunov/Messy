using System;
using System.Collections.Generic;
using Messy.DDD.Exceptions;

namespace Messy.DDD.EventRouters
{
    public class RegistrationEventRouter : IRouteEvents
    {
        private readonly IDictionary<Type, Action<object>> _handlers = new Dictionary<Type, Action<object>>();
        private IAggregate _registered;

        public virtual void Register<T>(Action<T> handler)
        {
            _handlers[typeof (T)] = @event => handler((T) @event);
        }

        public virtual void Register(IAggregate aggregate)
        {
            if (aggregate == null)
            {
                throw new ArgumentNullException(nameof(aggregate));
            }

            _registered = aggregate;
        }

        public virtual void Dispatch(object eventMessage)
        {
            Action<object> handler;

            if (!_handlers.TryGetValue(eventMessage.GetType(), out handler))
            {
                _registered.ThrowHandlerNotFound(eventMessage);
            }

            handler(eventMessage);
        }
    }
}