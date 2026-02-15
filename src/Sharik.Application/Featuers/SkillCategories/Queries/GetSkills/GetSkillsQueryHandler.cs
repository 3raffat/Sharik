using MediatR;
using Microsoft.EntityFrameworkCore;
using Sharik.Application.Common.Interfaces;
using Sharik.Application.Featuers.SkillCategories.Dtos;
using Sharik.Application.Featuers.SkillCategories.Mappers;
using Sharik.Application.Featuers.UserSkills.Dtos;
using Sharik.Domain.Common.Results;
namespace Sharik.Application.Featuers.SkillCategories.Queries.GetSkillsQuery
{
    public sealed class GetSkillsQueryHandler(IAppDbContext _context) : IRequestHandler<GetSkillsQuery , Result<List<SkillsDto>>>
    {
        public async Task<Result<List<SkillsDto>>> Handle(GetSkillsQuery request , CancellationToken ct)
        {

            var data = await _context.Skills.Select(s=> new SkillsDto(s.Id,s.Name,s.SkillCategoryId)).ToListAsync(ct);


            return data;
        }
    }
}
