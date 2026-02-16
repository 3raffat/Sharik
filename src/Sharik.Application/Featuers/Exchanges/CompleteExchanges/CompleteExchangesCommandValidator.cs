using FluentValidation;
using Sharik.Domain.Exchanges;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sharik.Application.Featuers.Exchanges.CompleteExchanges
{
    public sealed class CompleteExchangesCommandValidator :AbstractValidator<CompleteExchangesCommand>
    {

        public CompleteExchangesCommandValidator()
        {
            RuleFor(x => x.ProviderId)
            .NotEmpty()
            .WithMessage(ExchangeErrors.ProviderIdRequired.Description);

            RuleFor(x => x.ExchangeId)
                .NotEmpty()
                .WithMessage(ExchangeErrors.ExchangeIdRequired.Description);
        }
    }
}
