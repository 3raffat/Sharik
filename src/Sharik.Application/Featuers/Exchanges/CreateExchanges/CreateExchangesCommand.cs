using MediatR;
using Sharik.Domain.Common.Results;
using Sharik.Domain.Exchanges.Enums;

namespace Sharik.Application.Featuers.Exchanges.CreateExchanges
{
    public sealed record CreateExchangesCommand(Guid requesterId ,
                                                Guid providerId ,
                                                Guid skillOfferedId ,
                                                Guid skillRequestedId ,
                                                ExchangeType type ,
                                                int? duration ,
                                                int? pointsValue ,
                                                string? requesterMessage) : IRequest<Result<Success>>;

}
