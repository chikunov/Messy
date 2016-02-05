using System;
using System.Linq;

namespace Messy.MassTransitIntegration.Models
{
    public class HandlersEndpointSettings : EndpointSettingsBase
    {
        /// <summary>
        ///     Массив типов обработчиков
        /// </summary>
        public Type[] HandlerTypes { get; private set; }

        public HandlersEndpointSettings(
            ushort prefetchCount = 50,
            int? concurrencyLimit = 1,
            bool purgeOnStartup = false,
            string queueName = null,
            params Type[] handlerTypes)
        {
            if (handlerTypes == null || handlerTypes.Length == 0)
            {
                throw new Exception();
            }

            if (handlerTypes.Length > 1 && string.IsNullOrEmpty(queueName))
            {
                throw new Exception();
            }

            if (handlerTypes.Length == 1)
            {
                queueName = handlerTypes.First().FullName;
            }

            PrefetchCount = prefetchCount;
            ConcurrencyLimit = concurrencyLimit;
            PurgeOnStartup = purgeOnStartup;
            QueueName = queueName;
            HandlerTypes = handlerTypes;
        }
    }
}