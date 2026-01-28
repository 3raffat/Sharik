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

        public static Error SkillNameTooShort=>
            Error.Validation(
                code: "Skill.SkillName.TooShort",
                description: "Skill name must be at least 3 characters long."
            );

        public static Error SkillNameTooLong =>
            Error.Validation(
                code: "Skill.SkillName.TooLong",
                description: "Skill name cannot exceed 100 characters."
            );

    }
}
