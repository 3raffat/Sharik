using Sharik.Domain.Exchanges.Enums;

namespace Sharik.Application.Featuers.Exchanges.Dtos
{
    public sealed record ExchangeDto(string? providerName , string requesteName , string SkillOffered , string SkillRequested , string type , int? duration , int? pointsValue);
    public sealed record ProviderExchangeDto(Guid exchangeId,string requesteName , string SkillOffered , string SkillRequested , string ExchangeStatue , string type , string? requesterMessage , int? duration , int? pointsValue);

    public sealed record CreateExchangeRequest(Guid providerId ,
                                               Guid skillOfferedId ,
                                               Guid skillRequestedId ,
                                               ExchangeType type ,
                                               int? duration ,
                                               int? pointsValue ,
                                               string? requesterMessage);

}
