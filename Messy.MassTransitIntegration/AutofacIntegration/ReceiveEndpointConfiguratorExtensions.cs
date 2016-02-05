using System;
using System.Linq;
using Autofac;
using Autofac.Core;
using Automatonymous;
using MassTransit;
using MassTransit.Saga;

namespace Messy.MassTransitIntegration.AutofacIntegration
{
    public static class ReceiveEndpointConfiguratorExtensions
    {
        public static void LoadSaga(
            this IReceiveEndpointConfigurator configurator,
            ILifetimeScope scope,
            Type sagaType,
            Type sagaInstanceType)
        {
            var saga = scope.Resolve(sagaType);
            var sagaRepositoryInterfaceClosedType = typeof (ISagaRepository<>).MakeGenericType(sagaInstanceType);
            var sagaRepository = scope.Resolve(sagaRepositoryInterfaceClosedType);

            var stateMachineSagaMethod =
                typeof (StateMachineSubscriptionExtensions).GetMethod("StateMachineSaga").MakeGenericMethod(sagaInstanceType);

            stateMachineSagaMethod.Invoke(null, new[] {configurator, saga, sagaRepository, null});
        }

        public static void LoadHandlers(
            this IReceiveEndpointConfigurator configurator,
            ILifetimeScope scope,
            params Type[] handlerTypes)
        {
            var consumerAdapters =
                scope.ComponentRegistry.Registrations.SelectMany(x => x.Services.OfType<KeyedService>())
                     .Where(s => typeof (IConsumer).IsAssignableFrom(s.ServiceType))
                     .Select(s => new {ServiceKey = (string) s.ServiceKey, s.ServiceType});

            if (handlerTypes != null)
            {
                var handlerTypesNames = handlerTypes.Select(t => t.FullName).ToArray();
                consumerAdapters = consumerAdapters.Where(a => handlerTypesNames.Contains(a.ServiceKey));
            }

            consumerAdapters = consumerAdapters.ToArray();

            foreach (var consumerAdapter in consumerAdapters)
            {
                configurator.Consumer(
                    consumerAdapter.ServiceType,
                    type => scope.ResolveNamed(consumerAdapter.ServiceKey, consumerAdapter.ServiceType));
            }
        }
    }
}