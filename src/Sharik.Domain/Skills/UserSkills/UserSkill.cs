using Sharik.Domain.Common;
using Sharik.Domain.Common.Results;
using Sharik.Domain.Skills.UserSkills.Enums;
using Sharik.Infrastructure.Auth;

namespace Sharik.Domain.Skills.UserSkills
{
    public sealed class UserSkill : AuditableEntity
    {
        public Guid UserId { get; private set; }
        public Guid SkillId { get; private set; }
        public SkillLevel SkillLevel { get; private set; }
        public int StudentsCount { get; private set; }
        public int TotalEarnings { get; private set; }
        public int PointPerHour { get; private set; }


        public AppUser User { get; set; } = null!;
        public Skill Skill { get; set; } = null!;
        private UserSkill()
        { }

        private UserSkill(Guid userId,
                          Guid skillId,
                          SkillLevel skillLevel,
                          int pointPerHour)
        {
            UserId = userId;
            SkillId = skillId;
            SkillLevel = skillLevel;
            PointPerHour = pointPerHour;
        }

        public static Result<UserSkill> Create(Guid userId,
                                       Guid skillId,
                                       SkillLevel skillLevel,
                                       int pointPerHour)
        {

            if (userId == Guid.Empty)
                return UserSkillErrors.UserIdRequired;

            if (skillId == Guid.Empty)
                return SkillErrors.SkillIdRequired;

            if (!Enum.IsDefined(skillLevel))
                return UserSkillErrors.InvalidSkillLevel;

            if (pointPerHour <= 0 || pointPerHour > 100)
                return UserSkillErrors.PointPerHourInvalid;

            return new UserSkill(userId, skillId, skillLevel, pointPerHour);
        }

        public Result<Updated> Update(SkillLevel skillLevel,
                                      int pointPerHour)
        {
            if (!Enum.IsDefined(skillLevel))
                return UserSkillErrors.InvalidSkillLevel;

            if (pointPerHour <= 0 || pointPerHour > 100)
                return UserSkillErrors.PointPerHourInvalid;

            SkillLevel = skillLevel;
            PointPerHour = pointPerHour;
            return Result.Updated;
        }
    }
}
