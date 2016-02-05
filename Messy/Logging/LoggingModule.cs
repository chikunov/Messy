using System.Linq;
using Autofac;
using Autofac.Core;

namespace Messy.Logging
{
    public class LoggingModule : Module
    {
        private static void OnComponentPreparing(object sender, PreparingEventArgs e)
        {
            var t = e.Component.Activator.LimitType;

            e.Parameters = e.Parameters.Union(
                new[]
                {
                    new ResolvedParameter(
                        (p, i) => p.ParameterType == typeof (ILogger),
                        (p, i) => new NLogLogger(t))
                });
        }

        protected override void AttachToComponentRegistration(IComponentRegistry componentRegistry, IComponentRegistration registration)
        {
            registration.Preparing += OnComponentPreparing;
        }
    }
}