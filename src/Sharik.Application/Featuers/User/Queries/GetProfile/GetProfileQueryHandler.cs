using MediatR;
using Microsoft.EntityFrameworkCore;
using Sharik.Application.Common.Interfaces;
using Sharik.Application.Featuers.User.Dtos;
using Sharik.Application.Featuers.UserSkills.Dtos;
using Sharik.Application.Featuers.UserSkills.Mapper;
using Sharik.Domain.Common.Results;
using Sharik.Domain.Notifications;
using Sharik.Domain.Notifications.Enums;
using static Sharik.Application.Common.Caching.CacheKeys;

namespace Sharik.Application.Featuers.User.Queries.GetProfile
{
    public sealed class GetProfileQueryHandler(IAppDbContext _context) : IRequestHandler<GetProfileQuery , Result<CompleteUserProfileDto>>
    {
        public async Task<Result<CompleteUserProfileDto>> Handle(GetProfileQuery request , CancellationToken ct)
        {

            var data = await _context.Users.Where(u => u.Id == request.UserId)
                .Select(u =>
                 new CompleteUserProfileDto(
                        u.UserName ,
                        u.fullName ,
                        u.Bio ,
                        u.TotalPointsEarned ,
                        u.Rating ,
                        u.UserSkills
                        .Select(us => new UserSkillsDto(
                            us.Skill.Id ,
                            us.Skill.Name ,
                            us.SkillLevel ,
                            us.PointPerHour,
                            us.StudentsCount
                        )).ToList(),
                        u.ReceivedRatings
                        .Select(r=> new RatingDto(
                            r.Rater.fullName,
                            r.Score,
                            r.Comment)).ToList()
                        )).FirstOrDefaultAsync(ct);
         
            return data;
        }
    }
}
