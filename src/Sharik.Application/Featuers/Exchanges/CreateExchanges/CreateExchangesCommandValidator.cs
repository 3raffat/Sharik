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
            RuleFor(x => x.requesterId)
               .NotEmpty()
               .WithMessage(ExchangeErrors.RequesterIdRequired.Description);

            RuleFor(x => x.providerId)
                .NotEmpty()
                .WithMessage(ExchangeErrors.ProviderIdRequired.Description);

            RuleFor(x => x.skillOfferedId)
                .NotEmpty()
                .WithMessage(ExchangeErrors.SkillOfferedIdRequired.Description);

            RuleFor(x => x.skillRequestedId)
                .NotEmpty()
                .WithMessage(ExchangeErrors.SkillRequestedIdRequired.Description);

            RuleFor(x => x.type)
                .IsInEnum()
                .WithMessage(ExchangeErrors.TypeRequired.Description);

            RuleFor(x => x.duration)
                .GreaterThan(0)
                .When(x => x.duration.HasValue)
                .WithMessage(ExchangeErrors.DurationInvalid.Description);

            RuleFor(x => x.pointsValue)
                .GreaterThan(0)
                .When(x => x.pointsValue.HasValue)
                .WithMessage(ExchangeErrors.PointsValueInvalid.Description);

            RuleFor(x => x.requesterMessage)
                .MaximumLength(500)
                .WithMessage(ExchangeErrors.RequesterMessageTooLong.Description);

        }
    }
}
