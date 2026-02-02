using FluentValidation;
using Sharik.Domain.Common.Results;
using MediatR;

namespace Sharik.Application.Common.Behaviors
{
    public sealed class ValidationBehavior<TRequest, TResponse>(IValidator<TRequest>? validator = null)
        : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull

    {
        private readonly IValidator<TRequest>? _validator = validator;

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
        {
            if (_validator is null)
                return await next(ct);

            var ValidationResult = await _validator.ValidateAsync(request, ct);

            if (ValidationResult.IsValid)
                return await next(ct);

            var errors = ValidationResult.Errors.ConvertAll(e => Error.Validation(e.ErrorMessage, e.PropertyName));

            return (dynamic)errors;
        }
    }
}
