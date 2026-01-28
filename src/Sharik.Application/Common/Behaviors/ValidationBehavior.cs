using FluentValidation;
using Sharik.Domain.Common.Results;
using MediatR;

namespace Sharik.Application.Common.Behaviors
{
    public sealed class ValidationBehavior<TRequest, TResponse>(IValidator<TRequest>? validator = null)
        : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull

    {
        private readonly IValidator<TRequest>? _validator = validator;

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_validator is null)
                await next(cancellationToken);

            var ValidationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (ValidationResult.IsValid)
                await next(cancellationToken);

            var errors = ValidationResult.Errors.ConvertAll(e => Error.Validation(e.ErrorMessage, e.PropertyName));

            return (dynamic)errors;
        }
    }
}
