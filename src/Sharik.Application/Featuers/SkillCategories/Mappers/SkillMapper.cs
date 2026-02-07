using Sharik.Application.Featuers.SkillCategories.Dtos;
using Sharik.Application.Featuers.SkillCategories.Mappers;
using Sharik.Domain.Skills;

namespace Sharik.Application.Featuers.SkillCategories.Mappers
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
