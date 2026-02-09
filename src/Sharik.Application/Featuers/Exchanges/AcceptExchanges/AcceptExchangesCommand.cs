using MediatR;
using Sharik.Domain.Common.Results;

namespace Sharik.Application.Featuers.Exchanges.AcceptExchanges
{
    public sealed record class AcceptExchangesCommand(Guid ExchangeId , Guid ProviderId) : IRequest<Result<Success>>;


}
