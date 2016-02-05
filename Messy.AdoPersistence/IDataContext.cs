using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Threading.Tasks;

namespace Messy.AdoPersistence
{
    public interface IDataContext
    {
        string ConnectionString { get; }
        string SchemaName { get; }

        SqlConnection CreateConnection();

        void MigrateDatabase(params Assembly[] assemblies);

        Task<IEnumerable<T>> QueryProcedure<T>(string storedProcedureName, object param = null, TimeSpan? commandTimeout = null);

        Task<T> ExecuteScalar<T>(
            string storedProcedureName,
            object param = null,
            TimeSpan? commandTimeout = null,
            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);

        Task<int> ExecuteProcedure(
            string storedProcedureName,
            object param = null,
            TimeSpan? commandTimeout = null,
            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
    }
}