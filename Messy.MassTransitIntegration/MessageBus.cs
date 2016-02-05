using System;
using System.Threading.Tasks;
using Autofac;
using MassTransit;
using MassTransit.QuartzIntegration;
using Messy.MassTransitIntegration.AutofacIntegration;
using Messy.MassTransitIntegration.Consumers;
using Messy.MassTransitIntegration.Models;
using Messy.Messaging;
using Messy.Messaging.MessageTypes;
using Messy.Messaging.MessageTypes.Events;
using Quartz;
using Quartz.Impl;

namespace Messy.MassTransitIntegration
{
    public class MessageBus : IMessageBus
    {
        private BusHandle _busHandle;
        private readonly IScheduler _scheduler;
        private readonly IBusControl _serviceBus;
        private readonly IValidationService _validationService;

        public MessageBus(
            IValidationService validationService,
            ILifetimeScope scope,
            bool purgeOnStartUp,
            params EndpointSettingsBase[] endpointSettings)
        {
            var serverName = ApplicationConfig.GetConfigValue("RabbitMqServer", "localhost");
            var userName = ApplicationConfig.GetConfigValue("RabbitMqLogin", "");
            var password = ApplicationConfig.GetConfigValue("RabbitMqPassword", "");
            var queueUri = $"rabbitmq://{serverName}";

            _validationService = validationService;
            _scheduler = CreateScheduler();
            _serviceBus = Bus.Factory.CreateUsingRabbitMq(
                configurator =>
                {
                    var uri = new Uri(queueUri);

                    var host = configurator.Host(
                        uri,
                        hostConfigurator =>
                        {
                            hostConfigurator.Username(userName);
                            hostConfigurator.Password(password);
                        });

                    configurator.PurgeOnStartup = purgeOnStartUp;

                    configurator.ReceiveEndpoint(
                        host,
                        "Quartz",
                        e =>
                        {
                            e.PrefetchCount = 1;

                            e.Consumer(() => new ScheduleMessageConsumer(_scheduler));
                            e.Consumer(() => new CancelScheduledMessageConsumer(_scheduler));
                            configurator.UseMessageScheduler(e.InputAddress);
                        });

                    foreach (var endpoint in endpointSettings)
                    {
                        configurator.ReceiveEndpoint(
                            host,
                            endpoint.QueueName,
                            endpointConfigurator =>
                            {
                                endpointConfigurator.PurgeOnStartup = endpoint.PurgeOnStartup;
                                endpointConfigurator.PrefetchCount = endpoint.PrefetchCount;
                                if (endpoint.ConcurrencyLimit.HasValue)
                                {
                                    endpointConfigurator.UseConcurrencyLimit(endpoint.ConcurrencyLimit.Value);
                                }

                                var handlersEndpoint = endpoint as HandlersEndpointSettings;
                                if (handlersEndpoint != null)
                                {
                                    endpointConfigurator.LoadHandlers(scope, handlersEndpoint.HandlerTypes);
                                }

                                var allHandlersEndpoint = endpoint as AllHandlersEndpointSettings;
                                if (allHandlersEndpoint != null)
                                {
                                    endpointConfigurator.LoadHandlers(scope, null);
                                }

                                var sagaEndpoint = endpoint as SagaEndpointSettings;
                                if (sagaEndpoint != null)
                                {
                                    endpointConfigurator.LoadSaga(scope, sagaEndpoint.SagaType, sagaEndpoint.SagaInstanceType);
                                }
                            });
                    }
                });
        }

        public IQueryClient<TQuery, TResponse> CreateQueryClient<TQuery, TResponse>(Uri address, TimeSpan timeout)
            where TQuery : class, IQuery where TResponse : class, IMessage
        {
            var requestClient = _serviceBus.CreateRequestClient<TQuery, TResponse>(address, timeout);
            return new QueryClientAdapter<TQuery, TResponse>(requestClient);
        }

        public Uri GetAddress()
        {
            return _serviceBus.Address;
        }

        public void Start()
        {
            try
            {
                _busHandle = _serviceBus.Start();

                _scheduler.JobFactory = new MassTransitJobFactory(_serviceBus);

                _scheduler.Start();
            }
            catch (Exception)
            {
                _scheduler.Shutdown();
                throw;
            }
        }

        public Task Send<T>(T command) where T : class, ICommand
        {
            if (_validationService.Validate(command))
            {
                return _serviceBus.Publish(command);
            }

            return Task.CompletedTask;
        }

        public Task Publish<T>(T @event) where T : class, IEvent
        {
            if (_validationService.Validate(@event))
            {
                return _serviceBus.Publish(@event);
            }

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _scheduler.Standby();

            if (_busHandle != null)
            {
                _busHandle.Stop();
                _busHandle.Dispose();
            }

            _scheduler.Shutdown();
        }

        private static IScheduler CreateScheduler()
        {
            var schedulerFactory = new StdSchedulerFactory();

            var scheduler = schedulerFactory.GetScheduler();

            return scheduler;
        }
    }
}