using MediatR;
using Sharik.Domain.Common.Results;
using Sharik.Domain.Exchanges.Enums;

namespace Sharik.Application.Featuers.Exchanges.CreateExchanges
{
    public sealed record CreateSwapExchangesCommand(Guid requesterId ,
                                                    Guid providerId ,
                                                    Guid skillOfferedId ,
                                                    Guid skillRequestedId ,
                                                    string? requesterMessage) : IRequest<Result<Success>>;

}
