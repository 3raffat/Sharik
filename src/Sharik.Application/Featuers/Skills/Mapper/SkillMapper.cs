using Sharik.Application.Featuers.Skills.Dtos;
using Sharik.Domain.Skills;

namespace Sharik.Application.Featuers.Skills.Mapper
{
    public static class SkillMapper
    {
        extension(Skill skill)
        {
            public SkillDto ToDto()
            => new(skill.Id, skill.Name);
        }

        extension(IEnumerable<Skill> skills)
        {
            public List<SkillDto> ToDtos()
            => [..skills.Select(s=>s.ToDto())];
        }
    }
}
