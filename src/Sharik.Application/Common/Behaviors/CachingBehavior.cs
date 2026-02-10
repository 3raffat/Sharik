using MediatR;

namespace Sharik.Application.Common.Behaviors
{
    public sealed class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest , TResponse>
     where TRequest : notnull
     where TResponse : notnull
    {
        public Task<TResponse> Handle(TRequest request , RequestHandlerDelegate<TResponse> next , CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
