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
            RuleFor(x => x.RequesterId)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage(ExchangeErrors.RequesterIdRequired.Description);

            RuleFor(x => x.ExchangeId)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage(ExchangeErrors.ExchangeIdRequired.Description);


        }
    }
}
