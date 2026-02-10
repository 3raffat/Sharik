using Sharik.Application.Featuers.Exchanges.Dtos;
using Sharik.Domain.Exchanges;

namespace Sharik.Application.Featuers.Exchanges.Mapper
{
    public static class ExchangeMapper
    {
        extension(Exchange exchange)
        {
            public ExchangeDto ToDto()

            => new(exchange?.Provider?.fullName ,
                   exchange?.Requester?.fullName ,
                     exchange?.SkillOffered.Name ,
                     exchange?.SkillRequested.Name ,
                     exchange?.Type.ToString() ,
                     exchange.Duration ,
                     exchange.PointsValue);
        }

        extension(IEnumerable<Exchange> exchanges)
        {
            public List<ExchangeDto> ToDtos()
            => [.. exchanges.Select(e => e.ToDto())];
        }
    }
}
