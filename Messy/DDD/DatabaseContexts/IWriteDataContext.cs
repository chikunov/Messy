using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Messy.DDD.DatabaseContexts
{
    public interface IWriteDataContext : IQueryDataContext
    {
        /// <summary>
        ///     Save aggregate
        /// </summary>
        /// <param name="aggregate">Aggregate</param>
        Task Save([NotNull] IAggregate aggregate);
    }
}