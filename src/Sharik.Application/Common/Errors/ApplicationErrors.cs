using Sharik.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
