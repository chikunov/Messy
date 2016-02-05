using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Caching;
using System.Threading.Tasks;
using Messy.AdoPersistence;
using Messy.DDD;
using Messy.DDD.DatabaseContexts;
using Messy.Serialization;
using Messy.Serialization.Serializers;

namespace Messy.EventSourcingPersistence.Contexts
{
    internal class EventSourcingBatchDataContext : IBatchWriteDataContext
    {
        private const int MaxAggregateNum = 10000;
        private readonly ISerialize _serializer;
        private readonly IDataContext _context;
        private readonly IWriteDataContext _writeDataContext;
        private readonly MemoryCache _memoryCache;

        public EventSourcingBatchDataContext(
            ISerialize serializer,
            IDataContext context,
            IWriteDataContext writeDataContext)
        {
            _serializer = serializer;
            _context = context;
            _writeDataContext = writeDataContext;
            _memoryCache = new MemoryCache("AggregatesCache");
        }

        public Task<TAggregate> GetById<TAggregate>(Guid id, bool useSnapshots = false, int? version = null) where TAggregate : class, IAggregate
        {
            var aggregate = (TAggregate)_memoryCache.Get(id.ToString());
            if (aggregate != null)
            {
                return Task.FromResult(aggregate);
            }

            return _writeDataContext.GetById<TAggregate>(id);
        }

        public Task<bool> Exists(Guid id)
        {
            if (_memoryCache.Contains(id.ToString()))
            {
                return Task.FromResult(true);
            }

            return _writeDataContext.Exists(id);
        }

        public Task Save(IAggregate aggregate)
        {
            _memoryCache.Set(aggregate.Id.ToString(), aggregate, DateTimeOffset.MaxValue);

            if (_memoryCache.GetCount() > MaxAggregateNum)
            {
                return BatchSave();
            }

            return Task.FromResult(true);
        }

        public async Task BatchSave()
        {
            var aggregateIds = _memoryCache.Select(c => c.Key).ToArray();
            var aggregates = aggregateIds
                .Select(id => (IAggregate)_memoryCache.Remove(id))
                .Where(agg => agg != null)
                .ToArray();

            var eventList = aggregates
                .SelectMany(a => a.GetUncommittedEvents())
                .OrderBy(a => a.DateTime)
                .ToArray();
            var dataTable = ConvertToDataTable(eventList);

            using (var connection = _context.CreateConnection())
            {
                await connection.OpenAsync().ConfigureAwait(false);

                using (var transaction = connection.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        using (var bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction))
                        {
                            bulkCopy.DestinationTableName = $"[{_context.SchemaName}].[Events]";
                            bulkCopy.ColumnMappings.Add("AggregateId", "AggregateId");
                            bulkCopy.ColumnMappings.Add("AggregateName", "AggregateName");
                            bulkCopy.ColumnMappings.Add("Version", "Version");
                            bulkCopy.ColumnMappings.Add("DateTime", "DateTime");
                            bulkCopy.ColumnMappings.Add("EventName", "EventName");
                            bulkCopy.ColumnMappings.Add("Data", "Data");

                            await bulkCopy.WriteToServerAsync(dataTable).ConfigureAwait(false);

                            transaction.Commit();
                        }
                    }
                    catch (SqlException)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        private DataTable ConvertToDataTable(DomainEventRecord[] events)
        {
            var dt = new DataTable();
            dt.Columns.Add("AggregateId", typeof(Guid));
            dt.Columns.Add("AggregateName", typeof(string));
            dt.Columns.Add("Version", typeof(int));
            dt.Columns.Add("EventName", typeof (string));
            dt.Columns.Add("DateTime", typeof(DateTime));
            dt.Columns.Add("Data", typeof(object));

            foreach (var domainEvent in events)
            {
                var row = dt.NewRow();

                row["AggregateId"] = domainEvent.AggregateId;
                row["AggregateName"] = domainEvent.AggregateName;
                row["Version"] = domainEvent.Version;
                row["DateTime"] = domainEvent.DateTime;
                row["EventName"] = domainEvent.EventName;
                row["Data"] = _serializer.Serialize(domainEvent.Data);

                dt.Rows.Add(row);
            }

            return dt;
        }
    }
}
