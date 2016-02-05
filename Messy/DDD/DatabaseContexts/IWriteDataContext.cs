using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Messy.DDD.DatabaseContexts
{
    public interface IWriteDataContext : IQueryDataContext
    {
        /// <summary>
        ///     Сохранить агрегат
        /// </summary>
        /// <param name="aggregate">Агрегат</param>
        Task Save([NotNull] IAggregate aggregate);
    }
}