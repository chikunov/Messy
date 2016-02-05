using System.Threading.Tasks;

namespace Messy.DDD.DatabaseContexts
{
    public interface IBatchWriteDataContext : IWriteDataContext
    {
        /// <summary>
        ///     Пакетное сохранение агрегатов
        /// </summary>
        Task BatchSave();
    }
}