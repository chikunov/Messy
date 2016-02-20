namespace Messy.MassTransitIntegration.Models
{
    public abstract class EndpointSettingsBase
    {
        /// <summary>
        ///     Limit of concurrently processing messages
        /// </summary>
        public int? ConcurrencyLimit { get; protected set; }

        /// <summary>
        ///     RabbitMQ prefetch value
        /// </summary>
        public ushort PrefetchCount { get; protected set; }

        /// <summary>
        ///     Purge queue on startup
        /// </summary>
        public bool PurgeOnStartup { get; protected set; }

        /// <summary>
        ///     Queue name
        /// </summary>
        public string QueueName { get; protected set; }
    }
}