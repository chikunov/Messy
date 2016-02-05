using Autofac;

namespace Messy.AdoPersistence
{
    public class AdoPersistenceModule : Module
    {
        private readonly string _schemaName;
        private readonly string _connectionName;

        public AdoPersistenceModule(string schemaName, string connectionName)
        {
            _schemaName = schemaName;
            _connectionName = connectionName;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<DataContext>()
                .As<IDataContext>()
                .WithParameters(new []
                {
                    new NamedParameter("connectionName", _connectionName),
                    new NamedParameter("schemaName", _schemaName),
                });
        }
    }
}
