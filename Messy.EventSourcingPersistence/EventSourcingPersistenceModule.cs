using Autofac;
using Messy.AdoPersistence;
using Messy.DDD;
using Messy.DDD.DatabaseContexts;
using Messy.EventSourcingPersistence.Contexts;
using Messy.MassTransitIntegration;
using Messy.Serialization;
using Messy.Serialization.Serializers;

namespace Messy.EventSourcingPersistence
{
    public class EventSourcingPersistenceModule : Module
    {
        private readonly string _contextName;

        public EventSourcingPersistenceModule(string contextName)
        {
            _contextName = contextName;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule(new AdoPersistenceModule(_contextName, _contextName));

            builder.RegisterType<JsonSerializer>().As<ISerialize>();
            builder.RegisterType<MassTransitDomainEventsDispatcher>().As<IDomainEventsDispatcher>().SingleInstance();

            builder.RegisterType<EventSourcingDataContext>().As<IWriteDataContext>().SingleInstance();
            builder.RegisterType<EventSourcingDataContext>().As<IQueryDataContext>().SingleInstance();
            builder.RegisterType<EventSourcingDataContext>().As<IDatabaseInitializer>().SingleInstance();
            builder.RegisterType<EventSourcingBatchDataContext>().As<IBatchWriteDataContext>().SingleInstance();
        }
    }
}
