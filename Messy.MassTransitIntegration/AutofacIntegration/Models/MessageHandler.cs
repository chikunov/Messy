using System;
using System.Linq;
using System.Reflection;
using Autofac;
using Messy.Messaging;

namespace Messy.MassTransitIntegration.AutofacIntegration.Models
{
    internal class MessageHandler
    {
        public MessageHandlerInterface[] Interfaces { get; private set; }
        public string Name { get; private set; }
        public TypeInfo Type { get; private set; }

        public MessageHandler(TypeInfo type)
        {
            Type = type;
            Name = type.FullName;
            Interfaces =
                type.ImplementedInterfaces.Where(IsClosedTypeOfHandler)
                    .Select(i => new MessageHandlerInterface(this, i))
                    .ToArray();
        }

        public static bool IsClosedTypeOfHandler(Type type)
        {
            var openHandleType = typeof (IHandle<>);
            var openHandleChecked = typeof (IHandleChecked<>);
            var openQueryHandleType = typeof (IHandleQuery<,>);

            return type.IsClosedTypeOf(openHandleType) || type.IsClosedTypeOf(openQueryHandleType) ||
                   type.IsClosedTypeOf(openHandleChecked);
        }
    }
}