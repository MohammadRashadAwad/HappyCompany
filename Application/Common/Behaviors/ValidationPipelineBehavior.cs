using System.Reflection;
using FluentValidation.Results;
namespace Application.Common.Behaviors
{

    internal class ValidationPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : class
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        public ValidationPipelineBehavior(IEnumerable<IValidator<TRequest>> validators) =>
            _validators = validators;
        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            ValidationFailure[] validationFailures = await ValidateAsync(request);

            if (validationFailures.Length == 0)
            {
                return await next();
            }

            if (typeof(TResponse).IsGenericType &&
                typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
            {
                Type resultType = typeof(TResponse).GetGenericArguments()[0];

                MethodInfo? failureMethod = typeof(Result<>)
                    .MakeGenericType(resultType)
                    .GetMethod(nameof(Result<object>.ValidationFailure));

                if (failureMethod is not null)
                {
                    return (TResponse)failureMethod.Invoke(
                        null,
                        new object[] { CreateValidationError(validationFailures) });
                }
            }
            else if (typeof(TResponse) == typeof(Result))
            {
                return (TResponse)(object)Result.Failure(CreateValidationError(validationFailures));
            }

            throw new ValidationException(validationFailures);
        }

        private async Task<ValidationFailure[]> ValidateAsync(TRequest request)
        {
            if (!_validators.Any())
            {
                return Array.Empty<ValidationFailure>();
            }

            var context = new ValidationContext<TRequest>(request);

            ValidationResult[] validationResults = await Task.WhenAll(
                _validators.Select(validator => validator.ValidateAsync(context)));

            ValidationFailure[] validationFailures = validationResults
                .Where(validationResult => !validationResult.IsValid)
                .SelectMany(validationResult => validationResult.Errors)
                .ToArray();

            return validationFailures;
        }

        private static ValidationError CreateValidationError(ValidationFailure[] validationFailures) =>
            new(validationFailures.Select(f => new Error(f.ErrorCode, f.ErrorMessage)).ToArray());
    }


}
