using MediatR;
using Microsoft.EntityFrameworkCore;
using Sharik.Application.Common.Interfaces;
using Sharik.Application.Featuers.SkillCategories.Dtos;
using Sharik.Domain.Common.Results;
using Sharik.Application.Featuers.SkillCategories.Mappers;
namespace Sharik.Application.Featuers.SkillCategories.Queries.GetSkillsQuery
{
    public sealed class GetSkillsQueryHandler(IAppDbContext _context) : IRequestHandler<GetSkillsQuery , Result<List<SkillDto>>>
    {
        public async Task<Result<List<SkillDto>>> Handle(GetSkillsQuery request , CancellationToken ct)
        {

            var data = await _context.Skills.AsNoTracking().ToListAsync(ct);


            return data.ToDtos();
        }
    }
}
