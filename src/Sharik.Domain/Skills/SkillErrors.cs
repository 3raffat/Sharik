using Sharik.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sharik.Domain.Skills
{
    public static class SkillErrors
    {
        public static Error SkillIdRequired
         => Error.Validation(
             code: "Skill.SkillId.Required",
             description: "Skill ID cannot be empty."
         );
        public static Error SkillNameRequired
            => Error.Validation(
                code: "Skill.SkillName.Required",
                description: "Skill name cannot be empty."
            );

    }
}
