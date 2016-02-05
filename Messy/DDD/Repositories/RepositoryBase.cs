using System;
using System.Threading.Tasks;
using Messy.DDD.DatabaseContexts;

namespace Messy.DDD.Repositories
{
    public abstract class RepositoryBase<TAggregate> : IGenericRepository<TAggregate> where TAggregate : class, IAggregate
    {
        protected readonly IWriteDataContext Context;

        protected RepositoryBase(IWriteDataContext context)
        {
            Context = context;
        }

        public Task<TAggregate> GetById(Guid id)
        {
            return Context.GetById<TAggregate>(id);
        }

        public Task Save(TAggregate aggregate)
        {
            return Context.Save(aggregate);
        }
    }
}
