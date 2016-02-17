using Autofac;
using FluentValidation;
using Messy.DDD;
using Messy.MassTransitIntegration.Validation;
using Messy.Messaging;
using Messy.Serialization;
using Messy.Serialization.Serializers;

namespace Messy.MassTransitIntegration
{
    public class MassTransitIntegrationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<JsonSerializer>().As<ISerialize>();
            builder.RegisterType<MassTransitDomainEventsDispatcher>().As<IDomainEventsDispatcher>().SingleInstance();
            builder.RegisterType<ValidationService>().As<IValidationService>().SingleInstance();
            builder.RegisterType<AutofacValidatorFactory>().As<IValidatorFactory>().SingleInstance();
        }
    }
}
