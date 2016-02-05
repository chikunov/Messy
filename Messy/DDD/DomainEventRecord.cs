using System;

namespace Messy.DDD
{
    public class DomainEventRecord
    {
        public Guid AggregateId { get; private set; }
        public string AggregateName { get; private set; }
        public int Version { get; private set; }
        public string EventName { get; private set; }
        public DateTime DateTime { get; private set; }
        public object Data { get; private set; }

        public DomainEventRecord(Guid aggregateId, string aggregateName, int version, string eventName, DateTime dateTime, object data)
        {
            AggregateId = aggregateId;
            AggregateName = aggregateName;
            Version = version;
            EventName = eventName;
            DateTime = dateTime;
            Data = data;
        }
    }
}