using FluentValidation;
using Sharik.Domain.Exchanges;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sharik.Application.Featuers.Exchanges.CancelleExchanges
{
    public sealed class CancelleExchangesCommandValidator :AbstractValidator<CancelleExchangesCommand>
    {
        public CancelleExchangesCommandValidator()
        {
            RuleFor(x => x.ProviderId)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage(ExchangeErrors.ProviderIdRequired.Description);

            RuleFor(x => x.ExchangeId)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage(ExchangeErrors.ExchangeIdRequired.Description);

            RuleFor(x => x.cancellationReason)
                .Cascade(CascadeMode.Stop)
                .MaximumLength(500)
                .WithMessage(ExchangeErrors.CancellationReasonTooLong.Description);

        }
    }
}
