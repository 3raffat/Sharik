using Sharik.Application.Featuers.UserSkills.Dtos;
using Sharik.Domain.Skills.UserSkills;

namespace Sharik.Application.Featuers.UserSkills.Mapper
{
    public static class UserSkillMapper
    {
        extension(UserSkill userSkill)
        {
            public UserSkillDto ToDto()
                => new(userSkill.UserId, userSkill.SkillId, userSkill.SkillLevel, userSkill.PointPerHour);
        }

        extension(IEnumerable<UserSkill> userSkillDto)
        {
            public List<UserSkillDto> ToDtos()
                => [.. userSkillDto.Select(us => us.ToDto())];
        }
    }
}
