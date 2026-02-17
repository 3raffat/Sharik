using MediatR;
using Microsoft.EntityFrameworkCore;
using Sharik.Application.Common.Interfaces;
using Sharik.Application.Featuers.Exchanges.Dtos;
using Sharik.Domain.Common.Results;

namespace Sharik.Application.Featuers.Exchanges.Queries.GetExchangesByProviderId
{
    public sealed class GetExchangesByProviderIdQueryHandler(IAppDbContext _context) : IRequestHandler<GetExchangesByProviderIdQuery , Result<List<ProviderExchangeDto>>>
    {
        public async Task<Result<List<ProviderExchangeDto>>> Handle(GetExchangesByProviderIdQuery request , CancellationToken ct)
        {


            var data = await _context.Exchanges.Where(us => us.ProviderId == request.providerId)
                .Select(e =>
                new ProviderExchangeDto(
                            e.Id,
                            e.Requester.fullName ,
                            e.SkillOffered.Name ,
                            e.SkillRequested.Name ,
                            e.ExchangeStatus.ToString() ,
                            e.Type.ToString() ,
                            e.RequesterMessage,
                            e.Duration ,
                            e.PointsValue)).ToListAsync(ct);

            return data;
        }
    }
}
