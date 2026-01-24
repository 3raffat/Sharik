using Sharik.Domain.Common;
using Sharik.Domain.Common.Results;
using Sharik.Domain.Skills.SkillCategories;

namespace Sharik.Domain.Skills
{
    public sealed class Skill : AuditableEntity
    {
        public string Name { get; private set; } = string.Empty;
        public Guid SkillCategoryId { get; set; }
        public SkillCategory SkillCategory { get; set; } = null!;
        private Skill()
        { }
        private Skill(Guid id,
                      Guid categoryId,
                      string name)
            : base(id)
        {
            SkillCategoryId = categoryId;
            Name = name;
        }

        public static Result<Skill> Create(Guid id,
                                           Guid categoryId,
                                           string name)
        {
            if (id == Guid.Empty)
                return SkillErrors.SkillIdRequired;

            if (categoryId == Guid.Empty)
                return SkillCategoryErrors.SkillCategoryIdRequired;

            if (string.IsNullOrWhiteSpace(name))
                return SkillErrors.SkillNameRequired;

            return new Skill(id, categoryId, name);
        }

        public Result<Updated> Update(string name,
                                      Guid categoryId)
        {

            if (string.IsNullOrWhiteSpace(name))
                return SkillErrors.SkillNameRequired;

            if (categoryId == Guid.Empty)
                return SkillCategoryErrors.SkillCategoryIdRequired;

            Name = name;
            SkillCategoryId = categoryId;

            return Result.Updated;
        }
    }
}
