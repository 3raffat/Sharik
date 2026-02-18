using MediatR;
using Sharik.Domain.Common.Results;

namespace Sharik.Application.Featuers.Exchanges.CreateTeachingExchanges
{

    public sealed record CreateTeachingExchangesCommand(Guid requesterId ,
                                                        Guid providerId ,
                                                        Guid skillRequestedId ,
                                                        int duration ,
                                                        string? requesterMessage) : IRequest<Result<Success>>;
}
