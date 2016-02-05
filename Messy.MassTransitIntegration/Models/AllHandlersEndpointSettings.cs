using System;

namespace Messy.MassTransitIntegration.Models
{
    public class AllHandlersEndpointSettings : EndpointSettingsBase
    {
        public AllHandlersEndpointSettings(
            string queueName,
            ushort prefetchCount = 50,
            int? concurrencyLimit = 1,
            bool purgeOnStartup = false)
        {
            if (string.IsNullOrEmpty(queueName))
            {
                throw new Exception();
            }

            QueueName = queueName;
            PurgeOnStartup = purgeOnStartup;
            ConcurrencyLimit = concurrencyLimit;
            PrefetchCount = prefetchCount;
        }
    }
}