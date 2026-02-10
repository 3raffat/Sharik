using MediatR;
using Sharik.Domain.Common.Results;
using Sharik.Domain.Skills.UserSkills.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sharik.Application.Featuers.UserSkills.Dtos
{
    public sealed record UserSkillDto(Guid userId,
                                       Guid skillId,
                                       SkillLevel skillLevel,
                                       int pointPerHour);

    public sealed record CreateUserSkillRequest(Guid skillId,
                                                SkillLevel skillLevel,
                                                int pointPerHour);
    public sealed record UpdateUserSkillRequest(SkillLevel skillLevel,
                                                int pointPerHour);

    public sealed record DeleteUserSkillRequest(Guid skillId);

    public sealed record UserSkillsDto(string skillName ,
                                   SkillLevel skillLevel ,
                                   int pointPerHour);

}
