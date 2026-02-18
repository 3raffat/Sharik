using FluentValidation;
using Sharik.Domain.Exchanges;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sharik.Application.Featuers.Exchanges.CreateExchanges
{
    public sealed class CreateSwapExchangesCommandValidator :AbstractValidator<CreateSwapExchangesCommand>
    {
        public CreateSwapExchangesCommandValidator() 
        {
            RuleFor(x => x.requesterId).Cascade(CascadeMode.Stop)
               .NotEmpty()
               .WithMessage(ExchangeErrors.RequesterIdRequired.Description);

            RuleFor(x => x.providerId).Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage(ExchangeErrors.ProviderIdRequired.Description);

            RuleFor(x => x.skillOfferedId).Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage(ExchangeErrors.SkillOfferedIdRequired.Description);

            RuleFor(x => x.skillRequestedId).Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage(ExchangeErrors.SkillRequestedIdRequired.Description);

            RuleFor(x => x.requesterMessage).Cascade(CascadeMode.Stop)
                .MaximumLength(500)
                .WithMessage(ExchangeErrors.RequesterMessageTooLong.Description);

        }
    }
}
