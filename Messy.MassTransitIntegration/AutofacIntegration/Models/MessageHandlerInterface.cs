using System;
using System.Linq;
using Autofac;
using Messy.Messaging;

namespace Messy.MassTransitIntegration.AutofacIntegration.Models
{
    internal class MessageHandlerInterface
    {
        public Type DefinedInterfaceType { get; }
        public Type EffectiveInterfaceType { get; }
        public MessageHandler Handler { get; }
        public bool IsHandle => DefinedInterfaceType.IsClosedTypeOf(typeof (IHandle<>));

        public bool IsHandleChecked => DefinedInterfaceType.IsClosedTypeOf(typeof (IHandleChecked<>));
        public bool IsHandleQuery => DefinedInterfaceType.IsClosedTypeOf(typeof (IHandleQuery<,>));
        public Type MessageType { get; }
        public Type ResponseType { get; }

        public MessageHandlerInterface(MessageHandler handler, Type definedInterfaceType)
        {
            DefinedInterfaceType = definedInterfaceType;
            EffectiveInterfaceType = definedInterfaceType;
            Handler = handler;
            MessageType = definedInterfaceType.GenericTypeArguments.First();

            if (IsHandleQuery)
            {
                ResponseType = definedInterfaceType.GenericTypeArguments.Skip(1).First();
            }

            if (IsHandle)
            {
                var closedHandleCheckedInterfaceType = typeof (IHandleChecked<>).MakeGenericType(MessageType);
                EffectiveInterfaceType = closedHandleCheckedInterfaceType;
            }
        }
    }
}