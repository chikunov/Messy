using System;
using Autofac;
using FluentValidation;
using JetBrains.Annotations;

namespace Messy.MassTransitIntegration.Validation
{
    internal class AutofacValidatorFactory : ValidatorFactoryBase
    {
        private readonly ILifetimeScope _scope;

        public AutofacValidatorFactory(ILifetimeScope scope)
        {
            _scope = scope;
        }

        [CanBeNull]
        public override IValidator CreateInstance(Type validatorType)
        {
            object validator;
            if (_scope.TryResolve(validatorType, out validator))
            {
                return validator as IValidator;
            }

            return null;
        }
    }
}
