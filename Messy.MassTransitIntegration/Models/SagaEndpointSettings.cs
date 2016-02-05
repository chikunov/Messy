using System;

namespace Messy.MassTransitIntegration.Models
{
    public class SagaEndpointSettings : EndpointSettingsBase
    {
        public Type SagaInstanceType { get; }
        public Type SagaType { get; }

        public SagaEndpointSettings(
            Type sagaType,
            Type sagaInstanceType,
            ushort prefetchCount = 50,
            int? concurrencyLimit = 1,
            bool purgeOnStartup = false)
        {
            SagaType = sagaType;
            SagaInstanceType = sagaInstanceType;
            PrefetchCount = prefetchCount;
            ConcurrencyLimit = concurrencyLimit;
            PurgeOnStartup = purgeOnStartup;
            QueueName = sagaType.FullName;
        }
    }
}