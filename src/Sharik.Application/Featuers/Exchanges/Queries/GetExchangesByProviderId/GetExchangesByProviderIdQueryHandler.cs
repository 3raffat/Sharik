using MediatR;
using Microsoft.EntityFrameworkCore;
using Sharik.Application.Common.Interfaces;
using Sharik.Application.Featuers.Exchanges.Dtos;
using Sharik.Domain.Common.Results;

namespace Sharik.Application.Featuers.Exchanges.Queries.GetExchangesByProviderId
{
    public sealed class GetExchangesByProviderIdQueryHandler(IAppDbContext _context) : IRequestHandler<GetExchangesByProviderIdQuery , Result<List<UserExchangeDto>>>
    {
        public async Task<Result<List<UserExchangeDto>>> Handle(GetExchangesByProviderIdQuery request , CancellationToken ct)
        {


            var data = await _context.Users
    .Where(u => u.Id == request.providerId)
    .Select(u => new
    {
        ProvidedExchanges = u.ProvidedExchanges.Select(e => new UserExchangeDto(
            e.Id ,
            u.UserName ,
            e.Requester.UserName ,
            e.SkillOffered.Name ,
            e.SkillRequested.Name ,
            e.ExchangeStatus.ToString() ,
            e.Type.ToString() ,
            e.RequesterMessage ,
            e.Duration ,
            e.PointsValue ,
            "Provider"
        )) ,
        RequestedExchanges = u.RequestedExchanges.Select(e => new UserExchangeDto(
            e.Id ,
            e.Provider.UserName ,
            u.UserName ,
            e.SkillOffered.Name ,
            e.SkillRequested.Name ,
            e.ExchangeStatus.ToString() ,
            e.Type.ToString() ,
            e.RequesterMessage ,
            e.Duration ,
            e.PointsValue ,
            "Requester"
        ))
    })
    .FirstOrDefaultAsync();

            var result = data.ProvidedExchanges
                .Concat(data.RequestedExchanges)
                .ToList();


            return result;
        }
    }
}
