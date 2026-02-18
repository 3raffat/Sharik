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

        private UserSkill(Guid id,
                          Guid userId,
                          Guid skillId,
                          SkillLevel skillLevel,
                          int pointPerHour) : base(id)
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

            var validation = Validate(userId, skillId, skillLevel, pointPerHour);

            if (validation.IsFailure)
                return validation.Errors;

            return new UserSkill(Guid.NewGuid(), userId, skillId, skillLevel, pointPerHour);
        }

        public Result<Updated> Update(SkillLevel skillLevel,
                                      int pointPerHour)
        {
            var validation = Validate(skillLevel, pointPerHour);

            if (validation.IsFailure)
                return validation.Errors;

            SkillLevel = skillLevel;
            PointPerHour = pointPerHour;
            return Result.Updated;
        }
        private static Result<Success> Validate(SkillLevel skillLevel,
                                                int pointPerHour)

        {

            if (!Enum.IsDefined(skillLevel))
                return UserSkillErrors.InvalidSkillLevel;

            if (pointPerHour <= 0 || pointPerHour > 100)
                return UserSkillErrors.PointPerHourInvalid;

            return Result.Success;
        }

        public Result<Success> IncrementStudentCount()
        {
            StudentsCount++;
            return Result.Success;
        }

        private static Result<Success> Validate(Guid userId,
                                                Guid skillId,
                                                SkillLevel skillLevel,
                                                int pointPerHour)
        {

            if (userId == Guid.Empty)
                return UserSkillErrors.UserIdRequired;

            if (skillId == Guid.Empty)
                return SkillErrors.SkillIdRequired;

            return Validate(skillLevel, pointPerHour);

        }
    }
}
