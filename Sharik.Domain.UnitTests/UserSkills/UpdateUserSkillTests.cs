using FluentAssertions;
using Sharik.Domain.Skills.UserSkills;
using Sharik.Domain.Skills.UserSkills.Enums;

namespace Sharik.Domain.UnitTests.UserSkills
{
    public sealed class UpdateUserSkillTests
    {
        private readonly Guid _guid = Guid.NewGuid();
        private readonly Guid _empty = Guid.Empty;

        [Fact]
        public void Update_WithValidData_ValuesUpdated()
        {
            var userSkill = UserSkill.Create(Guid.NewGuid() , Guid.NewGuid() , SkillLevel.Beginner , 10).Value;
            var newLevel = SkillLevel.Expert;
            var newPph = 50;

            var result = userSkill.Update(newLevel , newPph);

            result.IsSuccess.Should().BeTrue();
            userSkill.SkillLevel.Should().Be(newLevel);
            userSkill.PointPerHour.Should().Be(newPph);
        }

        [Fact]
        public void Update_WhenInvalidPointPerHour_ShouldFail()
        {
            var userSkill = UserSkill.Create(Guid.NewGuid() , Guid.NewGuid() , SkillLevel.Beginner , 10).Value;

            var result = userSkill.Update(SkillLevel.Expert , 0);

            result.IsFailure.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Code == UserSkillErrors.PointPerHourInvalid.Code);
        }

        [Fact]
        public void Update_WhenInvalidSkillLevel_ShouldFail()
        {
            var userSkill = UserSkill.Create(Guid.NewGuid() , Guid.NewGuid() , SkillLevel.Beginner , 10).Value;

            var result = userSkill.Update((SkillLevel)99 , 0);

            result.IsFailure.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Code == UserSkillErrors.InvalidSkillLevel.Code);
        }

        [Fact]
        public void IncrementStudentCount_Called_CountIncreased()
        {
            var userSkill = UserSkill.Create(Guid.NewGuid() , Guid.NewGuid() , SkillLevel.Beginner , 10).Value;
            var initialCount = userSkill.StudentsCount;

            userSkill.IncrementStudentCount();

            userSkill.StudentsCount.Should().Be(initialCount + 1);
        }
    }
}
