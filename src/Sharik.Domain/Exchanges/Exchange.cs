using Sharik.Domain.Common;
using Sharik.Domain.Common.Results;
using Sharik.Domain.Exchanges.Enums;
using Sharik.Domain.Ratings;
using Sharik.Domain.Skills;
using Sharik.Infrastructure.Auth;

namespace Sharik.Domain.Exchanges
{
    public sealed class Exchange : AuditableEntity
    {
        public Guid RequesterId { get; private set; } // id user need to learn
        public Guid ProviderId { get; private set; } // id learnar user

        public Guid SkillOfferedId { get; private set; }

        public Guid SkillRequestedId { get; private set; }

        public ExchangeType Type { get; private set; }

        public int? Duration { get; private set; }

        public int? PointsValue { get; private set; }

        public string? RequesterMessage { get; private set; }

        public string? ProviderResponse { get; private set; }

        public string? CancellationReason { get; private set; }

        public ExchangeStatus ExchangeStatus = ExchangeStatus.Pending;
        public Skill SkillOffered { get; set; } = null!;
        public Skill SkillRequested { get; set; } = null!;
        public AppUser Requester { get; set; } = null!;
        public AppUser Provider { get; set; } = null!;
        private readonly List<Rating> _ratings = new();
        public IEnumerable<Rating> Ratings => _ratings.AsReadOnly();
        private Exchange() { }

        private Exchange(Guid requesterId ,
                         Guid providerId ,
                         Guid skillOfferedId ,
                         Guid skillRequestedId ,
                         ExchangeType type ,
                         int? duration ,
                         int? pointsValue ,
                         string? requesterMessage)
        {
            RequesterId = requesterId;
            ProviderId = providerId;
            SkillOfferedId = skillOfferedId;
            SkillRequestedId = skillRequestedId;
            Type = type;
            Duration = duration;
            PointsValue = pointsValue;
            RequesterMessage = requesterMessage;
        }
        private Exchange(Guid requesterId ,
                         Guid providerId ,
                         Guid skillOfferedId ,
                         Guid skillRequestedId ,
                         ExchangeType type ,
                         string? requesterMessage)
        {
            RequesterId = requesterId;
            ProviderId = providerId;
            SkillOfferedId = skillOfferedId;
            SkillRequestedId = skillRequestedId;
            Type = type;
            RequesterMessage = requesterMessage;
        }
        public static Result<Exchange> Create(Guid requesterId ,
                                              Guid providerId ,
                                              Guid skillOfferedId ,
                                              Guid skillRequestedId ,
                                              ExchangeType type ,
                                              int? duration ,
                                              int? pointsValue ,
                                              string? requesterMessage)
        {

            var validation = Validate(requesterId ,
                                      providerId ,
                                      skillOfferedId ,
                                      skillRequestedId ,
                                      type ,
                                      duration ,
                                      pointsValue ,
                                      requesterMessage);

            if (validation.IsFailure)
                return validation.Errors;

            if (type == ExchangeType.Swap)
                return new Exchange(requesterId , providerId , skillOfferedId , skillRequestedId , type , requesterMessage);

            return new Exchange(requesterId , providerId , skillOfferedId , skillRequestedId , type , duration , pointsValue , requesterMessage);
        }

        public Result<Updated> AcceptExchange(Guid providerId)
        {

            if (ExchangeStatus != ExchangeStatus.Pending)
                return ExchangeErrors.CanOnlyApprovePendingExchanges;

            if (ProviderId != providerId)
                return ExchangeErrors.Unauthorized;

            ExchangeStatus = ExchangeStatus.Accepted;

            return Result.Updated;
        }

        public Result<Updated> CancelExchange(string? cancellationReason)
        {
            if (ExchangeStatus == ExchangeStatus.Cancelled)
                return ExchangeErrors.ExchangeAlreadyCancelled;

            if (ExchangeStatus == ExchangeStatus.Completed)
                return ExchangeErrors.ExchangeAlreadyCompleted;

            if (PointsValue < 15)
                return ExchangeErrors.InsufficientPoints;


            CancellationReason = cancellationReason;
            ExchangeStatus = ExchangeStatus.Cancelled;
            PointsValue -= 15;


            return Result.Updated;
        }

        public Result<Updated> CompleteExchange()
        {
            if (ExchangeStatus != ExchangeStatus.Accepted)
                return ExchangeErrors.CanOnlyCompleteAcceptedExchanges;

            ExchangeStatus = ExchangeStatus.Completed;
            return Result.Updated;
        }
        public Result<Rating> RateExchange(Guid raterId , Guid ratedUserId , int score , string? comment)
        {
            if (ExchangeStatus != ExchangeStatus.Completed)
                return ExchangeErrors.CanOnlyRateCompletedExchanges;

            if (raterId != RequesterId && raterId != ProviderId)
                return ExchangeErrors.Unauthorized;

            if (raterId == ratedUserId)
                return ExchangeErrors.CannotRateOwnExchange;

            if (_ratings.Any(r => r.RatedUserId == ratedUserId) || _ratings.Any(r => r.RaterId == raterId))
                return ExchangeErrors.AlreadyRatedExchange;

            var ratingresult = Rating.Create(Id , raterId , ratedUserId , score , comment);

            if (ratingresult.IsFailure)
                return ratingresult.Errors;

            _ratings.Add(ratingresult.Value);

            return ratingresult.Value;

        }

        private static Result<Success> Validate(Guid requesterId ,
                                              Guid providerId ,
                                              Guid skillOfferedId ,
                                              Guid skillRequestedId ,
                                              ExchangeType type ,
                                              int? duration ,
                                              int? pointsValue ,
                                              string? requesterMessage)
        {
            if (requesterId == Guid.Empty)
                return ExchangeErrors.RequesterIdRequired;

            if (providerId == Guid.Empty)
                return ExchangeErrors.ProviderIdRequired;

            if (skillOfferedId == Guid.Empty)
                return ExchangeErrors.SkillOfferedIdRequired;

            if (skillRequestedId == Guid.Empty)
                return ExchangeErrors.SkillRequestedIdRequired;

            if (!Enum.IsDefined(type))
                return ExchangeErrors.InvalidExchangeType;

            if (requesterId == providerId)
                return ExchangeErrors.CannotExchangeWithSelf;

            if (skillOfferedId == skillRequestedId)
                return ExchangeErrors.CannotExchangeSameSkill;

            if (type == ExchangeType.Points && (duration == null || duration <= 0))
                return ExchangeErrors.DurationRequiredForPoints;

            if (type == ExchangeType.Points && (pointsValue == null || pointsValue <= 0))
                return ExchangeErrors.PointsValueRequiredForPointsExchange;

            if (requesterMessage != null && requesterMessage.Length > 1000)
                return ExchangeErrors.RequesterMessageTooLong;


            return Result.Success;
        }

    }
}
