using System;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Messy.DDD.Repositories
{
    public interface IGenericRepository<TAggregate> : IRepository
    {
        [ItemCanBeNull]
        Task<TAggregate> GetById(Guid id);

        Task Save(TAggregate aggregate);
    }
}
