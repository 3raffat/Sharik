using MediatR;
using Sharik.Application.Featuers.UserSkills.Dtos;
using Sharik.Domain.Common.Results;
using Sharik.Domain.Skills.UserSkills.Enums;

namespace Sharik.Application.Featuers.UserSkills.Commands.UpdateUserSkill
{
    public sealed record UpdateUserSkillCommand(Guid userId,
                                               Guid skillId,
                                               SkillLevel skillLevel,
                                               int pointPerHour) : IRequest<Result<Updated>>;

}
