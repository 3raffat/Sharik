using Sharik.Domain.Common;
using Sharik.Domain.Common.Results;

namespace Sharik.Domain.Skills.SkillCategories
{
    public sealed class SkillCategory : AuditableEntity
    {
        public string Name { get; private set; } = string.Empty;

        private readonly List<Skill> _skills = new();
        public IEnumerable<Skill> Skills => _skills.AsReadOnly();
        private SkillCategory()
        { }
        private SkillCategory(Guid id,
                              string name) : base(id)
        {
            Name = name;
        }
        public static Result<SkillCategory> Create(Guid id,
                                                   string name)
        {
            if (id == Guid.Empty)
                return SkillCategoryErrors.SkillCategoryIdRequired;

            if (string.IsNullOrWhiteSpace(name))
                return SkillCategoryErrors.SkillCategoryNameRequired;

            if (name.Length < 3)
                return SkillCategoryErrors.SkillCategoryNameTooShort;

            if (name.Length > 20)
                return SkillCategoryErrors.SkillCategoryNameTooLong;


            return new SkillCategory(id, name.Trim());
        }
        public Result<Updated> Update(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return SkillCategoryErrors.SkillCategoryNameRequired;

            if (name.Length < 3)
                return SkillCategoryErrors.SkillCategoryNameTooShort;

            if (name.Length > 20)
                return SkillCategoryErrors.SkillCategoryNameTooLong;

            Name = name.Trim();

            return Result.Updated;
        }

    }
}
