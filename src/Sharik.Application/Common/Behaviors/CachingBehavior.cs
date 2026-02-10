using MediatR;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using Sharik.Application.Common.Interfaces;
using Sharik.Domain.Common.Results.Abstraction;

namespace Sharik.Application.Common.Behaviors
{
    public sealed class CachingBehavior<TRequest, TResponse>(
     HybridCache _cache , ILogger<CachingBehavior<TRequest , TResponse>> _logger) : IPipelineBehavior<TRequest , TResponse>
     where TRequest : notnull
     where TResponse : notnull
    {
        public async Task<TResponse> Handle(TRequest request , RequestHandlerDelegate<TResponse> next , CancellationToken ct)
        {

            if (request is not ICachedQuery cachedRequest)
                return await next();

            _logger.LogInformation("Handling cached request {RequestType} with cache key {CacheKey}" , typeof(TRequest).Name , cachedRequest.CacheKey);

            var result = await _cache.GetOrCreateAsync<TResponse>(
                cachedRequest.CacheKey ,
                _ => new ValueTask<TResponse>((TResponse)(object)null!) ,
                new HybridCacheEntryOptions
                {
                    Flags = HybridCacheEntryFlags.DisableUnderlyingData
                } , cancellationToken: ct);


            if (result is null)
            {

                result = await next(ct);

                _logger.LogInformation("Request {RequestType} with cache key {CacheKey} is not cached, caching the result" , typeof(TRequest).Name , cachedRequest.CacheKey);

                if (result is IResult res && res.IsSuccess)
                {
                    _logger.LogInformation("Caching result for request {RequestType} with cache key {CacheKey}" , typeof(TRequest).Name , cachedRequest.CacheKey);
                    await _cache.SetAsync(
                        cachedRequest.CacheKey ,
                        result ,
                        new HybridCacheEntryOptions
                        {
                            Expiration = cachedRequest.Expiration
                        } , cancellationToken: ct);
                }
            }
            return result;
        }
    }
}
