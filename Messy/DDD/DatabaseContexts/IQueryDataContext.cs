using System;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Messy.DDD.DatabaseContexts
{
    public interface IQueryDataContext
    {
        /// <summary>
        ///     Get aggregate version by ID
        /// </summary>
        /// <typeparam name="TAggregate">Aggregate type</typeparam>
        /// <param name="id">Aggregate ID</param>
        /// <param name="useSnapshots">Use aggregate snapshots. Speeds things up for aggregates with large event streams. Slows things down otherwise</param>
        /// <param name="version">Aggregate version. Omit for the latest version</param>
        [ItemCanBeNull]
        Task<TAggregate> GetById<TAggregate>(Guid id, bool useSnapshots = false, int? version = null) where TAggregate : class, IAggregate;

        /// <summary>
        ///     Check existence of the aggregate by ID
        /// </summary>
        /// <param name="id">Aggregate ID</param>
        Task<bool> Exists(Guid id);
    }
}