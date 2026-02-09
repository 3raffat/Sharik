using MediatR;
using Sharik.Domain.Common.Results;

namespace Sharik.Application.Featuers.Exchanges.CancelleExchanges
{
    public sealed record CancelleExchangesCommand(Guid ProviderId , Guid ExchangeId , string? cancellationReason) : IRequest<Result<Updated>>;

    public sealed record CancelleExchangesRequest(string? cancellationReason);
}
