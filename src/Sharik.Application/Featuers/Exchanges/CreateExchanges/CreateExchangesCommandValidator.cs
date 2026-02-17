using FluentValidation;
using Sharik.Domain.Exchanges;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sharik.Application.Featuers.Exchanges.CreateExchanges
{
    public sealed class CreateExchangesCommandValidator :AbstractValidator<CreateExchangesCommand>
    {
        public CreateExchangesCommandValidator() 
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

            RuleFor(x => x.type).Cascade(CascadeMode.Stop)
                .IsInEnum()
                .WithMessage(ExchangeErrors.TypeRequired.Description);

            RuleFor(x => x.duration).Cascade(CascadeMode.Stop)
                .GreaterThan(0)
                .When(x => x.duration.HasValue)
                .WithMessage(ExchangeErrors.DurationInvalid.Description);

            RuleFor(x => x.pointsValue).Cascade(CascadeMode.Stop)
                .GreaterThan(0)
                .When(x => x.pointsValue.HasValue)
                .WithMessage(ExchangeErrors.PointsValueInvalid.Description);

            RuleFor(x => x.requesterMessage).Cascade(CascadeMode.Stop)
                .MaximumLength(500)
                .WithMessage(ExchangeErrors.RequesterMessageTooLong.Description);

        }
    }
}
