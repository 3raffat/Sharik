using Sharik.Domain.Common;
using Sharik.Domain.Exchanges.Enums;
using Sharik.Domain.Skills;

namespace Sharik.Domain.Exchanges
{
    public sealed class Exchange : AuditableEntity
    {
        public Guid RequesterId { get; private set; } // id user need to learn
        public Guid ProviderId { get; private set; } // id learnar user

        public Guid SkillOfferedId { get; private set; }

        public Guid  SkillRequestedId { get; private set; }

        public int PointsValue { get; set; } = 10;

        public string? RequesterMessage { get; set; }

        public string? ProviderResponse { get; set; }

        public DateTime? AcceptedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public DateTime? CancelledAt { get; set; }

        public string? CancellationReason { get; set; }


        public ExchangeStatus MyProperty = ExchangeStatus.Pending;
        public Skill SkillOffered { get; set; } = null!;
        public Skill SkillRequested { get; set; } = null!;
        private Exchange() { }

    }
}
