using FluentValidation;
using Messy.Messaging;
using Messy.Messaging.MessageTypes;

namespace Messy.MassTransitIntegration.Validation
{
    internal class ValidationService : IValidationService
    {
        private readonly IValidatorFactory _validatorFactory;

        public ValidationService(IValidatorFactory validatorFactory)
        {
            _validatorFactory = validatorFactory;
        }

        public bool Validate<T>(T message) where T : IMessage
        {
            var validator = _validatorFactory.GetValidator<T>();

            if (validator == null)
            {
                return true;
            }

            var validationResult = validator.Validate(this);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            return true;
        }
    }
}
