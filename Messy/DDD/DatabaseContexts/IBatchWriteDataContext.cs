using System.Threading.Tasks;

namespace Messy.DDD.DatabaseContexts
{
    public interface IBatchWriteDataContext : IWriteDataContext
    {
        Task BatchSave();
    }
}