using BuildingBlocks.CQRS;
using FluentValidation;
using MediatR;
using ResultPattern;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BuildingBlocks.Behavoirs
{
    public class ValidateBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> _validators)
            : IPipelineBehavior<TRequest, TResponse>
            where TRequest : ICommand<TResponse>
            where TResponse : class // Ensure TResponse can be a Result type
    {
        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            if (!_validators.Any())
            {
                return await next();
            }

            var context = new ValidationContext<TRequest>(request);

            var validationResults = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

            var failures = validationResults
                .SelectMany(r => r.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Any())
            {
                // Build error message from all validation failures
                var errorMessages = failures
                    .Select(f => f.ErrorMessage)
                    .ToList();

                var combinedError = string.Join("; ", errorMessages);

                // Create a validation error
                var error = ResultPattern.Error.Validation(combinedError, failures.ToDictionary(
                    f => f.PropertyName,
                    f => f.ErrorMessage
                ));

                // Return a failed Result
                return CreateFailureResult<TResponse>(error);
            }

            return await next();
        }

        private static TResponse CreateFailureResult<T>(ResultPattern.Error error)
        {
            // Use reflection to create Result<T>.Failure
            var resultType = typeof(T);

            // Check if TResponse is Result<TValue>
            if (resultType.IsGenericType &&
                resultType.GetGenericTypeDefinition() == typeof(Result<>))
            {
                var valueType = resultType.GetGenericArguments()[0];
                var failureMethod = typeof(Result<>)
                    .MakeGenericType(valueType)
                    .GetMethod("Failure", new[] { typeof(ResultPattern.Error) });

                return (TResponse)failureMethod!.Invoke(null, new object[] { error })!;
            }

            throw new InvalidOperationException(
                $"TResponse must be Result<T>, but was {resultType.Name}");
        }
    }
}
