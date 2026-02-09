using Sharik.Domain.Exchanges.Enums;

namespace Sharik.Application.Featuers.Exchanges.Dtos
{
    public sealed record ExchangeDto();
    public sealed record CreateExchangeRequest(Guid providerId ,
                                               Guid skillOfferedId ,
                                               Guid skillRequestedId ,
                                               ExchangeType type ,
                                               int? duration ,
                                               int? pointsValue ,
                                               string? requesterMessage);

}
