using System.Collections.Generic;
using System.Threading.Tasks;

namespace Messy.DDD
{
    public interface IDomainEventsDispatcher
    {
        Task Dispatch(ICollection<DomainEventRecord> events);
    }
}