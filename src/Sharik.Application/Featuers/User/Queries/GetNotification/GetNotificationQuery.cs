using Sharik.Application.Common.Caching;
using Sharik.Application.Common.Interfaces;
using Sharik.Application.Featuers.User.Dtos;
using Sharik.Domain.Common.Results;

namespace Sharik.Application.Featuers.User.Queries.GetNotification
{
    public sealed record GetNotificationQuery(Guid userId) : ICachedQuery<Result<List<NotificationDto>>>
    {
        public string CacheKey => CacheKeys.Notification.NotficationByUserId(userId);

        public TimeSpan Expiration => CacheKeys.ShortExpiration;
    }
}
