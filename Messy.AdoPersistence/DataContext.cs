using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DbUp;
using DbUp.ScriptProviders;
using Messy.AdoPersistence.Exceptions;
using Messy.AdoPersistence.Resources;
using Messy.Logging;

namespace Messy.AdoPersistence
{
    public class DataContext : IDataContext
    {
        public string ConnectionString { get; }
        public string SchemaName { get; }
        public string DatabaseName { get; }
        public TimeSpan DefaultTimeout { get; }

        private readonly ILogger _logger;

        public DataContext(ILogger logger, string schemaName, string connectionName)
        {
            _logger = logger;

            SchemaName = schemaName;
            ConnectionString = ConfigurationManager.ConnectionStrings[$"{connectionName}"].ConnectionString;
            DatabaseName = new SqlConnectionStringBuilder(ConnectionString).InitialCatalog;

            DefaultTimeout = TimeSpan.FromMinutes(1);
        }

        public void MigrateDatabase(params Assembly[] assemblies)
        {
            try
            {
                _logger.Debug(DataContextMessages.MigrateDatabaseStart);

                var upgrader = DeployChanges.To.SqlDatabase(ConnectionString, SchemaName)
                                            .WithScripts(
                                                new EmbeddedScriptsProvider(
                                                    assemblies,
                                                    s => s.EndsWith(".sql", StringComparison.InvariantCultureIgnoreCase),
                                                    Encoding.UTF8))
                                            .WithTransactionPerScript()
                                            .WithVariable("SchemaName", SchemaName)
                                            .Build();

                if (upgrader.IsUpgradeRequired())
                {
                    var scriptsToExecute = upgrader.GetScriptsToExecute();
                    var result = upgrader.PerformUpgrade();

                    if (!result.Successful)
                    {
                        var allScriptsNames = scriptsToExecute.Select(s => s.Name).ToArray();
                        var appliedScriptNames = result.Scripts.Select(s => s.Name).ToArray();

                        var notExecutedScriptsNames = allScriptsNames
                            .Except(appliedScriptNames)
                            .OrderBy(s => s)
                            .ToArray();
                        var notExecutedScripts = string.Join(",\n\r", notExecutedScriptsNames);

                        throw new PersistenceException(
                            string.Format(DataContextMessages.NotCommitedScripts, notExecutedScripts),
                            result.Error);
                    }

                    _logger.Debug(DataContextMessages.MigrateDatabaseFinished);
                }
            }
            catch (Exception exception)
            {
                _logger.Error(exception, DataContextMessages.MigrateDatabaseError);

                throw new PersistenceException(DataContextMessages.MigrateDatabaseError, exception);
            }
        }

        public SqlConnection CreateConnection()
        {
            var connection = new SqlConnection(ConnectionString);

            return connection;
        }

        public async Task<T> ExecuteScalar<T>(
            string storedProcedureName,
            object param = null,
            TimeSpan? commandTimeout = null,
            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            using (var connection = CreateConnection())
            {
                await connection.OpenAsync().ConfigureAwait(false);

                using (var transaction = connection.BeginTransaction(isolationLevel))
                {
                    try
                    {
                        var timeout = (commandTimeout ?? DefaultTimeout).Seconds;

                        var result = await connection.ExecuteScalarAsync<T>(storedProcedureName, param, transaction, timeout, CommandType.StoredProcedure);

                        transaction.Commit();

                        return result;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();

                        throw;
                    }
                }
            }
        }

        public async Task<IEnumerable<T>> QueryProcedure<T>(
            string storedProcedureName,
            object param = null,
            TimeSpan? commandTimeout = null)
        {
            using (var connection = CreateConnection())
            {
                var timeout = (commandTimeout ?? DefaultTimeout).Seconds;

                await connection.OpenAsync().ConfigureAwait(false);

                var result = await connection
                    .QueryAsync<T>(storedProcedureName, param, null, timeout, CommandType.StoredProcedure)
                    .ConfigureAwait(false);

                return result;
            }
        }

        public async Task<int> ExecuteProcedure(
            string storedProcedureName,
            object param = null,
            TimeSpan? commandTimeout = null,
            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            using (var connection = CreateConnection())
            {
                await connection.OpenAsync().ConfigureAwait(false);

                using (var transaction = connection.BeginTransaction(isolationLevel))
                {
                    try
                    {
                        var timeout = (commandTimeout ?? DefaultTimeout).Seconds;

                        var rows = await connection
                            .ExecuteAsync(storedProcedureName, param, transaction, timeout, CommandType.StoredProcedure)
                            .ConfigureAwait(false);

                        transaction.Commit();

                        return rows;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();

                        throw;
                    }
                }
            }
        }
    }
}