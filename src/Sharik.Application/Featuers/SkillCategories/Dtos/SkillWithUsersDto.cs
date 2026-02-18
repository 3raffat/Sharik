using Sharik.Domain.Skills.UserSkills.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sharik.Application.Featuers.SkillCategories.Dtos
{
    public sealed record SkillWithProvidersDto(Guid Id ,
                                           string Name ,
                                           List<SkillUserDto> providers);

    public sealed record SkillUserDto(Guid UserId ,
                                      string Name ,
                                      SkillLevel SkillLevel ,
                                      int PointsPerHour);

}
