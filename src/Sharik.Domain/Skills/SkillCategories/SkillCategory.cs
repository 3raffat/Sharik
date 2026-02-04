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
        public static Result<SkillCategory> Create(string name)
        {

            var validation = Validate(name);

            if (validation.IsFailure)
                return validation.Errors;

            return new SkillCategory(Guid.NewGuid(), name.Trim());
        }
        public Result<Updated> Update(string name)
        {
            var validation = Validate(name);

            if (validation.IsFailure)
                return validation.Errors;

            Name = name.Trim();

            return Result.Updated;
        }

        private static Result<Success> Validate(string name)
        {

            if (string.IsNullOrWhiteSpace(name))
                return SkillCategoryErrors.SkillCategoryNameRequired;

            if (name.Length < 3)
                return SkillCategoryErrors.SkillCategoryNameTooShort;

            if (name.Length > 20)
                return SkillCategoryErrors.SkillCategoryNameTooLong;

            return Result.Success;
        }
    }
}
