using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.SqlServer;

namespace Messy.EFPersistence
{
    public class BaseDbContextConfiguration : DbConfiguration
    {
        public BaseDbContextConfiguration()
        {
            SetDefaultConnectionFactory(new SqlConnectionFactory());
            SetProviderServices("System.Data.SqlClient", SqlProviderServices.Instance);
        }
    }
}