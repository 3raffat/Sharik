using MediatR;
using Sharik.Application.Common.Caching;
using Sharik.Application.Common.Interfaces;
using Sharik.Application.Featuers.Exchanges.Dtos;
using Sharik.Domain.Common.Results;

namespace Sharik.Application.Featuers.Exchanges.Queries.GetExchangesByProviderId
{
    public sealed record GetExchangesByProviderIdQuery(Guid providerId) : ICachedQuery<Result<List<ProviderExchangeDto>>>
    {
        string ICachedQuery.CacheKey => CacheKeys.Exchange.ExchangeByProviderId(providerId);

        TimeSpan ICachedQuery.Expiration => CacheKeys.ShortExpiration;
    }
}
