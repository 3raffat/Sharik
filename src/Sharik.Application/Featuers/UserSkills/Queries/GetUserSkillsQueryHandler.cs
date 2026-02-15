using MediatR;
using Microsoft.EntityFrameworkCore;
using Sharik.Application.Common.Interfaces;
using Sharik.Application.Featuers.UserSkills.Dtos;
using Sharik.Domain.Common.Results;

namespace Sharik.Application.Featuers.UserSkills.Queries
{
    public sealed class GetUserSkillsQueryHandler(IAppDbContext _context) : IRequestHandler<GetUserSkillsQuery , Result<List<UserSkillsDto>>>
    {
        public async Task<Result<List<UserSkillsDto>>> Handle(GetUserSkillsQuery request , CancellationToken ct)
        {
            var data = await _context.UserSkills
                .Where(us => us.UserId == request.userId)
                .Select(us => new UserSkillsDto(us.Skill.Id,us.Skill.Name , us.SkillLevel , us.PointPerHour , us.StudentsCount))
                .ToListAsync(ct);

            return data;
        }
    }
}
