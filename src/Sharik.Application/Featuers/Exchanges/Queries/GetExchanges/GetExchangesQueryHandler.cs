using MediatR;
using Microsoft.EntityFrameworkCore;
using Sharik.Application.Common.Interfaces;
using Sharik.Application.Featuers.Exchanges.Dtos;
using Sharik.Application.Featuers.Exchanges.Mapper;
using Sharik.Domain.Common.Results;

namespace Sharik.Application.Featuers.Exchanges.Queries.GetExchanges
{
    public sealed class GetExchangesQueryHandler(IAppDbContext _context) : IRequestHandler<GetExchangesQuery , Result<List<ExchangeDto>>>
    {
        public async Task<Result<List<ExchangeDto>>> Handle(GetExchangesQuery request , CancellationToken ct)
        {

            var data = await _context.Exchanges.
                 Include(e => e.Provider)
                .Include(e => e.SkillOffered)
                .Include(e => e.SkillRequested).ToListAsync(ct);


            return data.ToDtos();
        }
    }
}
