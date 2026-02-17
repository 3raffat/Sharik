using MediatR;
using Microsoft.EntityFrameworkCore;
using Sharik.Application.Common.Interfaces;
using Sharik.Application.Featuers.SkillCategories.Dtos;
using Sharik.Domain.Common.Results;
namespace Sharik.Application.Featuers.SkillCategories.Queries.GetSkillsQuery
{
    public sealed class GetSkillsQueryHandler(IAppDbContext _context) : IRequestHandler<GetSkillsQuery , Result<List<SkillWithUsersDto>>>
    {
        public async Task<Result<List<SkillWithUsersDto>>> Handle(GetSkillsQuery request , CancellationToken ct)
        {

            var data = await _context.Skills.
                Select(s => new SkillWithUsersDto(s.Id , s.Name , s.UserSkills
               .Select(us => new SkillUserDto(
                us.UserId ,
                us.User.fullName ,
                us.SkillLevel ,
                us.PointPerHour
                    )).ToList())).ToListAsync(ct);


            return data;
        }
    }
}
