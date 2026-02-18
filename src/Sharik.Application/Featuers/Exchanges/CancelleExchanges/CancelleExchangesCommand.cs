using MediatR;
using Sharik.Domain.Common.Results;

namespace Sharik.Application.Featuers.Exchanges.CancelleExchanges
{
    public sealed record CancelleExchangesCommand(Guid RequesterId , Guid ExchangeId) : IRequest<Result<Updated>>;

}
