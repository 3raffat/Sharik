using MediatR;
using Microsoft.EntityFrameworkCore;
using Sharik.Application.Common.Interfaces;
using Sharik.Application.Featuers.SkillCategories.Dtos;
using Sharik.Application.Featuers.SkillCategories.Mappers;
using Sharik.Domain.Common.Results;

namespace Sharik.Application.Featuers.SkillCategories.Queries.GetSkills
{
    public sealed class GetSkillsQueryHandler(IAppDbContext _context) : IRequestHandler<GetSkillsQuery , Result<List<SkillDto>>>
    {
        public async Task<Result<List<SkillDto>>> Handle(GetSkillsQuery request , CancellationToken ct)
        {
            var data = await _context.Skills.ToListAsync(ct);


            return data.ToDtos();
        }
    }
}
