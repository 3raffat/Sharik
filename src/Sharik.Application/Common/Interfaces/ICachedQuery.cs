using MediatR;

namespace Sharik.Application.Common.Interfaces
{
    public interface ICachedQuery
    {
        string CacheKey { get; }
        TimeSpan Expiration { get; }
    }

    public interface ICachedQuery<TRequest> : IRequest<TRequest>, ICachedQuery
    {

    }
}