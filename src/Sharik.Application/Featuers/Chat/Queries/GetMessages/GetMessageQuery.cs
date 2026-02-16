using Sharik.Application.Common.Caching;
using Sharik.Application.Common.Interfaces;
using Sharik.Application.Featuers.Chat.Dtos;
using Sharik.Domain.Common.Results;

namespace Sharik.Application.Featuers.Chat.Queries.GetMessages
{
    public record GetMessagesQuery(Guid ExchangeId , Guid UserId)
   : ICachedQuery<Result<List<MessageDto>>>
    {
        public string CacheKey => CacheKeys.Message.MessagesByExchangeId(ExchangeId);

        public TimeSpan Expiration => CacheKeys.ShortExpiration;
    }
}
