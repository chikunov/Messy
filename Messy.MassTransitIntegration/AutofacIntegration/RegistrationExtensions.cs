using System;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Core;
using MassTransit;
using Messy.MassTransitIntegration.AutofacIntegration.Models;
using Messy.MassTransitIntegration.Consumers;
using Messy.Messaging.MessageTypes;
using Messy.Messaging.MessageTypes.Events;

namespace Messy.MassTransitIntegration.AutofacIntegration
{
    public static class RegistrationExtensions
    {
        /// <summary>
        ///     Register message handlers from assemblies in the AutoFac container 
        /// </summary>
        /// <param name="builder">Autofac container builder</param>
        /// <param name="handlerAssemblies">Assemblies to check</param>
        public static void RegisterHandlers(this ContainerBuilder builder, params Assembly[] handlerAssemblies)
        {
            var messageHandlers = GetMessageHandlersInAssemblies(handlerAssemblies);

            foreach (var messageHandler in messageHandlers)
            {
                builder.RegisterType(messageHandler.Type).AsSelf();

                foreach (var messageHandlerInterface in messageHandler.Interfaces)
                {
                    builder.RegisterType(messageHandler.Type)
                        .Named(messageHandler.Name, messageHandlerInterface.DefinedInterfaceType);

                    if (messageHandlerInterface.IsHandle)
                    {
                        RegisterHandleToHandleCheckedAdapter(builder, messageHandlerInterface);
                    }

                    RegisterMassTransitConsumerAdapter(builder, messageHandlerInterface);
                }
            }
        }

        private static MessageHandler[] GetMessageHandlersInAssemblies(Assembly[] assemblies)
        {
            return
                assemblies.SelectMany(a => a.DefinedTypes)
                    .Where(t => t.ImplementedInterfaces.Any(MessageHandler.IsClosedTypeOfHandler))
                    .Select(t => new MessageHandler(t))
                    .ToArray();
        }

        private static void RegisterHandleToHandleCheckedAdapter(
            ContainerBuilder builder,
            MessageHandlerInterface messageHandlerInterface)
        {
            var closedHandleAdapterType =
                typeof(HandleToHandleCheckedAdapter<>).MakeGenericType(messageHandlerInterface.MessageType);

            builder.RegisterType(closedHandleAdapterType)
                .Named(messageHandlerInterface.Handler.Name, messageHandlerInterface.EffectiveInterfaceType)
                .WithParameter(
                    new ResolvedParameter(
                        (pi, ctx) => MessageHandler.IsClosedTypeOfHandler(pi.ParameterType),
                        (pi, ctx) =>
                            ctx.ResolveNamed(
                                messageHandlerInterface.Handler.Name,
                                messageHandlerInterface.DefinedInterfaceType)));
        }

        private static void RegisterMassTransitConsumerAdapter(
            ContainerBuilder builder,
            MessageHandlerInterface messageHandlerInterface)
        {
            var closedConsumerAdapterType = CreateClosedConsumerAdapterType(messageHandlerInterface);
            var closedMassTransitConsumerType = typeof(IConsumer<>).MakeGenericType(
                messageHandlerInterface.MessageType);

            builder.RegisterType(closedConsumerAdapterType)
                .Named(messageHandlerInterface.Handler.Name, closedMassTransitConsumerType)
                .WithParameter(
                    new ResolvedParameter(
                        (pi, ctx) => MessageHandler.IsClosedTypeOfHandler(pi.ParameterType),
                        (pi, ctx) =>
                            ctx.ResolveNamed(
                                messageHandlerInterface.Handler.Name,
                                messageHandlerInterface.EffectiveInterfaceType)));
        }

        private static Type CreateClosedConsumerAdapterType(MessageHandlerInterface messageHandlerInterface)
        {
            if (typeof(ICommand).IsAssignableFrom(messageHandlerInterface.MessageType))
            {
                return typeof(CommandConsumerToHandleCheckedAdapter<>).MakeGenericType(messageHandlerInterface.MessageType);
            }

            if (typeof(IEvent).IsAssignableFrom(messageHandlerInterface.MessageType))
            {
                return typeof(EventConsumerToHandleCheckedAdapter<>).MakeGenericType(messageHandlerInterface.MessageType);
            }

            if (typeof(IQuery).IsAssignableFrom(messageHandlerInterface.MessageType))
            {
                return typeof(QueryConsumerToHandlerAdapter<,>).MakeGenericType(
                    messageHandlerInterface.MessageType,
                    messageHandlerInterface.ResponseType);
            }

            throw new Exception("Unknown type");
        }
    }
}