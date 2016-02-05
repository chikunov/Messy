using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Messy.AdoPersistence;
using Messy.AdoPersistence.Extensions;
using Messy.DDD;
using Messy.DDD.DatabaseContexts;
using Messy.Serialization;
using Messy.Serialization.Serializers;

namespace Messy.EventSourcingPersistence.Contexts
{
    internal class EventSourcingDataContext : IWriteDataContext, IDatabaseInitializer
    {
        private readonly IDataContext _context;
        private readonly ISerialize _serializer;
        private readonly IDomainEventsDispatcher _eventsDispatcher;

        public EventSourcingDataContext(
            IDataContext context, 
            ISerialize serializer, 
            IDomainEventsDispatcher eventsDispatcher)
        {
            _context = context;
            _serializer = serializer;
            _eventsDispatcher = eventsDispatcher;
        }

        [ItemCanBeNull]
        public async Task<TAggregate> GetById<TAggregate>(Guid id, bool useSnapshots = false, int? version = null) where TAggregate : class, IAggregate
        {
            TAggregate aggregate;
            LinkedList<DomainEventRecord> domainEvents;
            if (useSnapshots)
            {
                aggregate = await GetSnapshot<TAggregate>(id, version);

                if (aggregate == null)
                {
                    domainEvents = await GetAggregateEvents(id, 0, version);
                    if (domainEvents.Count == 0)
                    {
                        return null;
                    }

                    aggregate = (TAggregate)Activator.CreateInstance(typeof(TAggregate), true);
                }
                else
                {
                    domainEvents = await GetAggregateEvents(id, aggregate.Version, version);
                }
            }
            else
            {
                domainEvents = await GetAggregateEvents(id, 0, version);
                if (domainEvents.Count == 0)
                {
                    return null;
                }

                aggregate = (TAggregate)Activator.CreateInstance(typeof(TAggregate), true);
            }

            foreach (var domainEvent in domainEvents)
            {
                aggregate.ApplyEvent(domainEvent.Data);
            }

            return aggregate;
        }

        public async Task<bool> Exists(Guid id)
        {
            var isExists = await _context
                .ExecuteScalar<bool>($"[{_context.SchemaName}].[IsAggregateExists]", new {AggregateId = id})
                .ConfigureAwait(false);

            return isExists;
        }

        public async Task Save(IAggregate aggregate)
        {
            var events = aggregate.GetUncommittedEvents();

            var eventsDataTable = events
                .Select(
                    e => new
                    {
                        e.AggregateId,
                        e.AggregateName,
                        e.Version,
                        e.DateTime,
                        e.EventName,
                        Data = _serializer.Serialize(e.Data)
                    })
                .ToArray()
                .MakeDataTable();

            await _context.ExecuteProcedure($"[{_context.SchemaName}].[SaveEvents]", new {Events = eventsDataTable}).ConfigureAwait(false);

            await _eventsDispatcher.Dispatch(events).ConfigureAwait(false);

            aggregate.ClearUncommitted();
        }

        [ItemCanBeNull]
        private async Task<TAggregate> GetSnapshot<TAggregate>(Guid id, int? version) where TAggregate : class, IAggregate
        {
            TAggregate aggregate = null;
            using (var connection = _context.CreateConnection())
            {
                var command = connection.CreateCommand();
                command.CommandText = $"[{_context.SchemaName}].[GetAggregateSnapshot]";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@AggregateId", id);
                command.Parameters.AddWithValue("@MaxVersion", version);

                await connection.OpenAsync().ConfigureAwait(false);

                using (var reader = await command.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    if (await reader.ReadAsync())
                    {
                        aggregate = _serializer.Deserialize<TAggregate>((byte[])reader["Data"]);
                    }
                }
            }

            return aggregate;
        }

        private async Task<LinkedList<DomainEventRecord>> GetAggregateEvents(Guid id, int sinceVersion, int? toVersion = null)
        {
            var eventList = new LinkedList<DomainEventRecord>();
            using (var connection = _context.CreateConnection())
            {
                var command = connection.CreateCommand();
                command.CommandText = $"[{_context.SchemaName}].[GetEventsByAggregateId]";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@AggregateId", id);
                command.Parameters.AddWithValue("@SinceVersion", sinceVersion);
                command.Parameters.AddWithValue("@ToVersion", toVersion);

                await connection.OpenAsync().ConfigureAwait(false);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        eventList.AddLast(
                            new DomainEventRecord(
                                reader.GetGuid(0),
                                reader.GetString(1),
                                reader.GetInt32(2),
                                reader.GetString(3),
                                reader.GetDateTime(4),
                                _serializer.Deserialize<object>((byte[])reader["Data"])));
                    }
                }
            }

            return eventList;
        }

        public void Initialize()
        {
            _context.MigrateDatabase(Assembly.GetExecutingAssembly());
        }
    }
}