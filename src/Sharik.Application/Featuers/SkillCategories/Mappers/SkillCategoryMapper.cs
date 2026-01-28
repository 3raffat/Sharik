using Sharik.Application.Featuers.SkillCategories.Dtos;
using Sharik.Domain.Skills.SkillCategories;

namespace Sharik.Application.Featuers.SkillCategories.Mappers
{
    public static class SkillCategoryMapper
    {
        extension(SkillCategory category)
        {
            public SkillCategoryDto ToDto()
            => new(category.Id, category.Name);
        }

        extension(IEnumerable<SkillCategory> skillCategories)
        {
            public List<SkillCategoryDto> ToDtos()
            => [.. skillCategories.Select(sc => sc.ToDto())];
        }
    }
}
