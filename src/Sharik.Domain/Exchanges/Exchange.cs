using Sharik.Domain.Common;
using Sharik.Domain.Common.Results;
using Sharik.Domain.Exchanges.Enums;
using Sharik.Domain.Messages;
using Sharik.Domain.Ratings;
using Sharik.Domain.Skills;
using Sharik.Infrastructure.Auth;

namespace Sharik.Domain.Exchanges
{
    public sealed class Exchange : AuditableEntity
    {
        public Guid RequesterId { get; private set; } // id user need to learn
        public Guid ProviderId { get; private set; } // id learnar user

        public Guid? SkillOfferedId { get; private set; }  // for requester

        public Guid SkillRequestedId { get; private set; } // for provider

        public ExchangeType Type { get; private set; }

        public int? Duration { get; private set; }

        public int? PointsValue { get; private set; }

        public string? RequesterMessage { get; private set; }

        public string? ProviderResponse { get; private set; }

        public string? CancellationReason { get; private set; }

        public ExchangeStatus ExchangeStatus = ExchangeStatus.Pending;
        public Skill? SkillOffered { get; set; } = null!;
        public Skill SkillRequested { get; set; } = null!;
        public AppUser Requester { get; set; } = null!;
        public AppUser Provider { get; set; } = null!;
        private readonly List<Rating> _ratings = new();
        public IEnumerable<Rating> Ratings => _ratings.AsReadOnly();
        private readonly List<Message> _message = new();
        public IEnumerable<Message> Messages => _message.AsReadOnly();
        private Exchange() { }

        private Exchange(Guid requesterId ,
                         Guid providerId ,
                         Guid skillRequestedId ,
                         ExchangeType type ,
                         int duration ,
                         int pointsValue ,
                         string? requesterMessage)
        {
            RequesterId = requesterId;
            ProviderId = providerId;
            SkillRequestedId = skillRequestedId;
            Type = type;
            Duration = duration;
            PointsValue = pointsValue;
            RequesterMessage = requesterMessage;
        }

        private Exchange(Guid requesterId ,
                         Guid providerId ,
                         Guid skillRequestedId ,
                         ExchangeType type ,
                         string? requesterMessage)
        {
            RequesterId = requesterId;
            ProviderId = providerId;
            SkillRequestedId = skillRequestedId;
            Type = type;
            RequesterMessage = requesterMessage;
        }

        public static Result<Exchange> CreateSwap(Guid requesterId ,
                                                   Guid providerId ,
                                                   Guid skillOfferedId ,
                                                   Guid skillRequestedId ,
                                                   string? requesterMessage)
        {

            if (skillOfferedId == skillRequestedId)
                return ExchangeErrors.CannotExchangeSameSkill;

            if (skillOfferedId == Guid.Empty)
                return ExchangeErrors.SkillOfferedIdRequired;

            var validation = Validate(requesterId ,
                                      providerId ,
                                      skillRequestedId ,
                                      requesterMessage);

            if (validation.IsFailure)
                return validation.Errors;

            return new Exchange(requesterId , providerId , skillRequestedId , ExchangeType.Swap , requesterMessage);

        }

        public static Result<Exchange> CreateTeaching(Guid requesterId ,
                                                      Guid providerId ,
                                                      Guid skillRequestedId ,
                                                      int duration ,
                                                      int PointsValue ,
                                                      string? requesterMessage)
        {


            var validation = Validate(requesterId ,
                                     providerId ,
                                     skillRequestedId ,
                                     duration ,
                                     PointsValue ,
                                     requesterMessage);

            return new Exchange(requesterId , providerId , skillRequestedId , ExchangeType.Teaching , duration , PointsValue , requesterMessage);
        }


        #region
        public Result<Updated> AcceptExchange(Guid providerId)
        {

            if (ExchangeStatus != ExchangeStatus.Pending)
                return ExchangeErrors.CanOnlyApprovePendingExchanges;

            if (ProviderId != providerId)
                return ExchangeErrors.Unauthorized;

            ExchangeStatus = ExchangeStatus.Accepted;

            return Result.Updated;
        }

        public Result<Updated> CancelExchange()
        {
            if (ExchangeStatus == ExchangeStatus.Cancelled)
                return ExchangeErrors.ExchangeAlreadyCancelled;

            if (ExchangeStatus == ExchangeStatus.Completed)
                return ExchangeErrors.ExchangeAlreadyCompleted;

            if (ExchangeStatus == ExchangeStatus.Accepted)
                return ExchangeErrors.ExchangeAlreadyAccepted;

            if (PointsValue is int value)
            {
                var cuttOff = value - 5;
                Requester.AddPoints(cuttOff);
            }

            ExchangeStatus = ExchangeStatus.Cancelled;
            return Result.Updated;
        }
        public Result<Updated> RejectExchange(Guid providerId)
        {

            if (ExchangeStatus != ExchangeStatus.Pending)
                return ExchangeErrors.CanOnlyRejectPendingExchanges;

            if (ProviderId != providerId)
                return ExchangeErrors.Unauthorized;

            if (PointsValue is int value)
                Requester.AddPoints(value);

            ExchangeStatus = ExchangeStatus.Rejected;

            return Result.Updated;
        }
        public Result<Updated> CompleteExchange()
        {
            if (ExchangeStatus != ExchangeStatus.Accepted)
                return ExchangeErrors.CanOnlyCompleteAcceptedExchanges;

            if (PointsValue is int value)
                Provider.AddPoints(value);

            var skill = Provider.UserSkills.FirstOrDefault(x => x.SkillId == SkillRequestedId);

            skill!.IncrementStudentCount();

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

            if (_ratings.Any(r => r.RatedUserId == ratedUserId && r.ExchangeId == this.Id))
                return ExchangeErrors.AlreadyRatedExchange;

            var ratingresult = Rating.Create(Id , raterId , ratedUserId , score , comment);

            if (ratingresult.IsFailure)
                return ratingresult.Errors;

            _ratings.Add(ratingresult.Value);

            return ratingresult.Value;

        }
        public Result<Message> AddMessage(Guid senderId , string content)
        {
            if (ExchangeStatus != ExchangeStatus.Accepted)
                return ExchangeErrors.ChatOnlyInAcceptedExchanges;

            if (senderId != RequesterId && senderId != ProviderId)
                return ExchangeErrors.Unauthorized;

            var msgResult = Message.Create(Id , senderId , content);
            if (msgResult.IsFailure)
                return msgResult.Errors;

            _message.Add(msgResult.Value);
            return msgResult.Value;
        }
        #endregion

        private static Result<Success> Validate(Guid requesterId ,
                                                Guid providerId ,
                                                Guid skillRequestedId ,
                                                int duration ,
                                                int pointValue ,
                                                string? requesterMessage)


        {

            if (duration <= 0)
                return ExchangeErrors.InvalidDuration;

            if (pointValue < 0)
                return ExchangeErrors.PointsValueInvalid;

            return Validate(requesterId , providerId , skillRequestedId , requesterMessage);

        }

        private static Result<Success> Validate(Guid requesterId ,
                                              Guid providerId ,
                                              Guid skillRequestedId ,
                                              string? requesterMessage)
        {
            if (requesterId == Guid.Empty)
                return ExchangeErrors.RequesterIdRequired;

            if (providerId == Guid.Empty)
                return ExchangeErrors.ProviderIdRequired;



            if (skillRequestedId == Guid.Empty)
                return ExchangeErrors.SkillRequestedIdRequired;



            if (requesterId == providerId)
                return ExchangeErrors.CannotExchangeWithSelf;




            if (requesterMessage != null && requesterMessage.Length > 1000)
                return ExchangeErrors.RequesterMessageTooLong;


            return Result.Success;
        }

        public static Result<int> CalculateTotalPoints(int PointPerHour , int totalPointsEarned , int duration)
        {

            var requiredPoints = PointPerHour * duration;

            if (requiredPoints > totalPointsEarned)
                return ExchangeErrors.NotEnoughPoints(requiredPoints);

            return requiredPoints;
        }

    }
}
