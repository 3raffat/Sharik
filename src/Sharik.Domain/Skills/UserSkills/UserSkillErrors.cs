using Sharik.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sharik.Domain.Skills.UserSkills
{
    public static class UserSkillErrors
    {

        public static readonly Error SkillLevelRequired = Error.Validation(
            code:"UserSkill.SkillLevel.Required",
            description:"Skill level is required."
        );
        public static readonly Error PointPerHourInvalid = Error.Validation(
            code:"UserSkill.PointPerHour.Invalid",
            description:"Point per hour must be greater than zero and less than or equal 100."
        );

        public static readonly Error UserIdRequired = Error.Validation(
            code: "UserSkill.UserId.Required",
            description: "User id is required."
        );

        public static readonly Error InvalidSkillLevel = Error.Validation(
            code: "UserSkill.SkillLevel.Invalid",
            description: "Invalid skill level."
        );
    }
}
