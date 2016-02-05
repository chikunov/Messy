namespace Messy.MassTransitIntegration.Models
{
    public abstract class EndpointSettingsBase
    {
        /// <summary>
        ///     Число одновременно обрабатываемых сообщений
        /// </summary>
        public int? ConcurrencyLimit { get; protected set; }

        /// <summary>
        ///     Значение Prefetch RabbitMQ
        /// </summary>
        public ushort PrefetchCount { get; protected set; }

        /// <summary>
        ///     Очищать очередь при запуске
        /// </summary>
        public bool PurgeOnStartup { get; protected set; }

        /// <summary>
        ///     Наименование очереди
        /// </summary>
        public string QueueName { get; protected set; }
    }
}