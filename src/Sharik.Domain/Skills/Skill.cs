using Sharik.Domain.Common;
using Sharik.Domain.Common.Results;
using Sharik.Domain.Exchanges;
using Sharik.Domain.Skills.SkillCategories;
using Sharik.Domain.Skills.UserSkills;

namespace Sharik.Domain.Skills
{
    public sealed class Skill : AuditableEntity
    {
        public string Name { get; private set; } = string.Empty;
        public Guid SkillCategoryId { get; set; }
        public SkillCategory SkillCategory { get; set; } = null!;
        private readonly List<Exchange> _offeredExchanges = new();
        public IEnumerable<Exchange> OfferedExchanges => _offeredExchanges;

        private readonly List<Exchange> _requestedExchanges = new();
        public IEnumerable<Exchange> RequestedExchanges => _requestedExchanges;

        private readonly List<UserSkill> _userSkills = new();
        public IEnumerable<UserSkill> UserSkills => _userSkills.AsReadOnly();

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

        public static Result<Skill> Create(Guid categoryId,
                                           string name)
        {
            var validation = Validate(name, categoryId);

            if (validation.IsFailure)
                return validation.Errors;

            return new Skill(Guid.NewGuid(), categoryId, name.Trim());
        }

        public Result<Updated> Update(string name,
                                      Guid categoryId)
        {

            var validation = Validate(name, categoryId);

            if (validation.IsFailure)
                return validation.Errors;

            Name = name.Trim();
            SkillCategoryId = categoryId;

            return Result.Updated;
        }
        private static Result<Success> Validate(string name,
                                      Guid categoryId)
        {
            if (categoryId == Guid.Empty)
                return SkillCategoryErrors.SkillCategoryIdRequired;

            if (string.IsNullOrWhiteSpace(name))
                return SkillErrors.SkillNameRequired;

            if (name.Length < 3)
                return SkillErrors.SkillNameTooShort;

            if (name.Length > 100)
                return SkillErrors.SkillNameTooLong;

            return Result.Success;
        }
    }
}
