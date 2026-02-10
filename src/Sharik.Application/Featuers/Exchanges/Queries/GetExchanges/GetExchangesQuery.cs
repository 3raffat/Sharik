using Sharik.Application.Common.Caching;
using Sharik.Application.Common.Interfaces;
using Sharik.Application.Featuers.Exchanges.Dtos;
using Sharik.Domain.Common.Results;

namespace Sharik.Application.Featuers.Exchanges.Queries.GetExchanges
{
    public sealed record GetExchangesQuery() : ICachedQuery<Result<List<ExchangeDto>>>
    {
        public string CacheKey => CacheKeys.Exchange.AllExchanges;

        public TimeSpan Expiration => CacheKeys.LongExpiration;
    }
}

