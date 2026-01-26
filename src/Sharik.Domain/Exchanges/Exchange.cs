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

        private Exchange(Guid requesterId,
                         Guid providerId,
                         Guid skillOfferedId,
                         Guid skillRequestedId,
                         ExchangeType type,
                         int? duration,
                         int? pointsValue,
                         string? requesterMessage,
                         ExchangeStatus exchangeStatus)
        {
            RequesterId = requesterId;
            ProviderId = providerId;
            SkillOfferedId = skillOfferedId;
            SkillRequestedId = skillRequestedId;
            Type = type;
            Duration = duration;
            PointsValue = pointsValue;
            RequesterMessage = requesterMessage;
            ExchangeStatus = exchangeStatus;
        }

        public static Result<Exchange> Create(Guid requesterId,
                                              Guid providerId,
                                              Guid skillOfferedId,
                                              Guid skillRequestedId,
                                              ExchangeType type,
                                              int? duration,
                                              int? pointsValue,
                                              string? requesterMessage,
                                              ExchangeStatus exchangeStatus)
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

            if (type == ExchangeType.Swap && (duration == null || duration <= 0))
                return ExchangeErrors.DurationRequiredForSwap;

            if (type == ExchangeType.Points && (pointsValue == null || pointsValue <= 0))
                return ExchangeErrors.PointsValueRequiredForPointsExchange;

            if (requesterMessage != null && requesterMessage.Length > 1000)
                return ExchangeErrors.RequesterMessageTooLong;

            return new Exchange(requesterId, providerId, skillOfferedId, skillRequestedId, type, duration, pointsValue, requesterMessage, exchangeStatus);
        }

    }
}
