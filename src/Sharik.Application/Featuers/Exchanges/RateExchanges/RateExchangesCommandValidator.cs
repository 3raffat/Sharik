using FluentValidation;
using Sharik.Domain.Exchanges;
using Sharik.Domain.Ratings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sharik.Application.Featuers.Exchanges.RateExchanges
{
    public sealed class RateExchangesCommandValidator : AbstractValidator<RateExchangesCommand>
    {
        public RateExchangesCommandValidator()
        {

            RuleFor(x => x.exchangeId)
             .NotEmpty()
             .WithErrorCode(RatingErrors.ExchangeIdRequired.Code)
             .WithMessage(RatingErrors.ExchangeIdRequired.Description);

            RuleFor(x => x.raterId)
                .NotEmpty()
                .WithErrorCode(RatingErrors.RaterIdRequired.Code)
                .WithMessage(RatingErrors.RaterIdRequired.Description);

            RuleFor(x => x.ratedUserId)
                .NotEmpty()
                .WithErrorCode(RatingErrors.RatedUserIdRequired.Code)
                .WithMessage(RatingErrors.RatedUserIdRequired.Description);

            RuleFor(x => x.score)
                .InclusiveBetween(1 , 5)
                .WithMessage(RatingErrors.ScoreOutOfRange.Description);

            RuleFor(x => x.comment)
                .MaximumLength(500)
                .WithErrorCode(RatingErrors.CommentTooLong.Code)
                .WithMessage(RatingErrors.CommentTooLong.Description);

        }
    }
}
