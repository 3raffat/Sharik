using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Sharik.Domain.Skills.UserSkills.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SkillLevel
    {
        Beginner,
        Intermediate ,
        Advanced ,
        Expert 
    }
}
