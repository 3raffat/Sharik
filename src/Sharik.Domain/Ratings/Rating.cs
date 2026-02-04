using Sharik.Domain.Common;
using Sharik.Domain.Common.Results;
using Sharik.Domain.Exchanges;
using Sharik.Domain.Ratings.Enums;
using Sharik.Infrastructure.Auth;
namespace Sharik.Domain.Ratings
{
    public sealed class Rating : AuditableEntity
    {
        public Guid ExchangeId { get; private set; }

        public Guid RaterId { get; private set; }

        public Guid RatedUserId { get; private set; }

        public int Score { get; private set; }

        public string? Comment { get; private set; }

        public RatingType Type { get; private set; }
        public Exchange Exchange { get; set; } = null!;
        public AppUser Rater { get; set; } = null!;
        public AppUser RatedUser { get; set; } = null!;

        private Rating() { }

        private Rating(Guid exchangeId, Guid raterId, Guid ratedUserId, int score, string? comment, RatingType type)
        {
            ExchangeId = exchangeId;
            RaterId = raterId;
            RatedUserId = ratedUserId;
            Score = score;
            Comment = comment;
            Type = type;
        }

        public static Result<Rating> Create(Guid exchangeId, Guid raterId, Guid ratedUserId, int score, string? comment, RatingType type)
        {
            var validation = Validate(exchangeId, raterId, ratedUserId, score, comment, type);

            if (validation.IsFailure)
                return validation.Errors;

            return new Rating(exchangeId, raterId, ratedUserId, score, comment, type);
        }

        public Result<Updated> Update(int score, string? comment)
        {
            var validation = Validate(score, comment);

            if (validation.IsFailure)
                return validation.Errors;

            Score = score;
            Comment = comment;

            return Result.Updated;
        }

        private static Result<Success> Validate(int score, string? comment)
        {
            if (score < 1 || score > 5)
                return RatingErrors.ScoreOutOfRange;

            if (comment != null && comment.Length > 500)
                return RatingErrors.CommentTooLong;

            return Result.Success;
        }

        private static Result<Success> Validate(Guid exchangeId,
                                                Guid raterId,
                                                Guid ratedUserId,
                                                int score,
                                                string? comment,
                                                RatingType type)
        {

            if (exchangeId == Guid.Empty)
                return RatingErrors.ExchangeIdRequired;

            if (raterId == Guid.Empty)
                return RatingErrors.RaterIdRequired;

            if (ratedUserId == Guid.Empty)
                return RatingErrors.RatedUserIdRequired;

            if (raterId == ratedUserId)
                return RatingErrors.CannotRateSelf;

            if (!Enum.IsDefined(type))
                return RatingErrors.InvalidRatingType;

            return Validate(score, comment);
        }

    }
}
