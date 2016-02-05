using System;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Messy.DDD.DatabaseContexts
{
    public interface IQueryDataContext
    {
        /// <summary>
        ///     Получить агрегат определенной версии по идентификатору
        /// </summary>
        /// <typeparam name="TAggregate">Тип агрегата</typeparam>
        /// <param name="id">Идентификатор агрегата</param>
        /// <param name="useSnapshots">
        ///     Использовать ли снимки состояния агрегата. Даст прирост скорости, если агрегат с большим
        ///     количеством версий (событий). По умолчанию не используется
        /// </param>
        /// <param name="version">Версия агрегата для загрузки. null для возврата последней версии</param>
        [ItemCanBeNull]
        Task<TAggregate> GetById<TAggregate>(Guid id, bool useSnapshots = false, int? version = null) where TAggregate : class, IAggregate;

        /// <summary>
        ///     Проверка существования агрегата по его идентификатору
        /// </summary>
        /// <param name="id">Идентификатор агрегата</param>
        Task<bool> Exists(Guid id);
    }
}