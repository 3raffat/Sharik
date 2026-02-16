using FluentValidation;
using Sharik.Domain.Exchanges;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sharik.Application.Featuers.Exchanges.AcceptExchanges
{
    public sealed class AcceptExchangesCommandValidator :AbstractValidator<AcceptExchangesCommand>
    {
        public AcceptExchangesCommandValidator()
        {
            RuleFor(x => x.ExchangeId)
               .NotEmpty()
               .WithMessage(ExchangeErrors.ExchangeIdRequired.Description);

            RuleFor(x => x.ProviderId)
                .NotEmpty()
                .WithMessage(ExchangeErrors.ProviderIdRequired.Description);
        }
    }
}
