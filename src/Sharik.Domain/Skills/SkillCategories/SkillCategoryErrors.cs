using Sharik.Domain.Common.Results;

namespace Sharik.Domain.Skills.SkillCategories
{
    public static class SkillCategoryErrors
    {

        public static Error SkillCategoryNameRequired
            => Error.Validation(
                code: "SkillCategory.Name.Required",
                description: "Skill category name cannot be empty."
            );
        public static Error SkillCategoryIdRequired
           => Error.Validation(
               code: "SkillCategory.Id.Required",
               description: "Skill category ID cannot be empty."
           );

        public static Error SkillCategoryNameTooShort
            => Error.Validation(
                code: "Skill.SkillName.TooShort",
                description: "Category name must be at least 3 characters long."
            );

        public static Error SkillCategoryNameTooLong
           => Error.Validation(
                  code: "Skill.SkillName.TooLong",
                  description: "Category name cannot exceed 20 characters."
           );
    }
}
