using Sharik.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
