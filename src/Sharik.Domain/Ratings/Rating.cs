using Sharik.Domain.Common;
using Sharik.Domain.Exchanges;
using Sharik.Domain.Ratings.Enums;
using System.ComponentModel.DataAnnotations;

namespace Sharik.Domain.Ratings
{
    public sealed class Rating : AuditableEntity
    {
        public Guid ExchangeId { get; private set; }

        public Guid RaterId { get; private set; }

        public Guid RatedUserId { get; private set; }

        public int Score { get; set; }

        public string? Comment { get; set; }

        public RatingType Type { get; set; }
        public Exchange Exchange { get; set; } = null!;

        private Rating() { }
    }
}
