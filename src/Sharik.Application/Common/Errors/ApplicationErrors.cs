using Sharik.Domain.Common.Results;

namespace Sharik.Application.Common.Errors
{
    public static class ApplicationErrors
    {

        public static Error SkillCategoryNotFound => Error.NotFound(
            code: "SkillCategory.NotFound",
            description: "The specified skill category was not found.");

        public static Error SkillAlreadyExists => Error.Conflict(
            code: "Skill.AlreadyExists",
            description: "A skill with the specified name already exists.");

        public static Error SkillNotFound => Error.NotFound(
            code: "Skill.NotFound",
            description: "The specified skill was not found.");

        public static Error SkillCategoryAlreadyExists => Error.Conflict(
            code: "SkillCategory.AlreadyExists",
            description: "A Category with the specified name already exists.");

        public static Error UserNotFound => Error.NotFound(
             code: "User.NotFound",
             description: "The user with the specified ID was not found.");

        public static Error SkillAlreadyExistsForUser => Error.Conflict(
             code: "UserSkill.AlreadyExists",
             description: "This skill is already assigned to the user.");

        public static Error UserSkillNotFound => Error.NotFound(
            code: "UserSkill.NotFound",
            description: "The Skill with the specified ID was not found.");

        public static Error ProfileIncomplete => Error.Conflict(
            code: "AppUser.Profile.Incomplete",
            description: "Profile is still incomplete."
       );
    }
}
