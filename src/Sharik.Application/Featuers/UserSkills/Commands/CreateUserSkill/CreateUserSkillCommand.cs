using MediatR;
using Sharik.Application.Featuers.UserSkills.Dtos;
using Sharik.Domain.Common.Results;
using Sharik.Domain.Skills.UserSkills.Enums;

namespace Sharik.Application.Featuers.UserSkills.Commands.CreateUserSkill
{
    public sealed record CreateUserSkillCommand(Guid userId,
                                                Guid skillId,
                                                SkillLevel skillLevel,
                                                int pointPerHour) : IRequest<Result<UserSkillDto>>;

}
