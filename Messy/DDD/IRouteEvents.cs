using System;

namespace Messy.DDD
{
    public interface IRouteEvents
    {
        void Dispatch(object eventMessage);
        void Register<T>(Action<T> handler);
        void Register(IAggregate aggregate);
    }
}