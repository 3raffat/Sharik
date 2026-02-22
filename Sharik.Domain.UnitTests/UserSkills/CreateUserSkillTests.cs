using FluentAssertions;
using Sharik.Domain.Skills;
using Sharik.Domain.Skills.UserSkills;
using Sharik.Domain.Skills.UserSkills.Enums;

namespace Sharik.Domain.UnitTests.UserSkills
{
    public sealed class CreateUserSkillTests
    {
        private readonly Guid _guid = Guid.NewGuid();
        private readonly Guid _empty = Guid.Empty;

        [Fact]
        public void Create_WithValidData_ShouldSuccess()
        {
            var userId = _guid;
            var skillId = _guid;
            var level = SkillLevel.Intermediate;
            var pph = 25;

            var result = UserSkill.Create(userId , skillId , level , pph);

            result.IsSuccess.Should().BeTrue();
            result.Value.UserId.Should().Be(userId);
            result.Value.SkillId.Should().Be(skillId);
            result.Value.SkillLevel.Should().Be(level);
            result.Value.PointPerHour.Should().Be(pph);
        }

        [Fact]
        public void Create_WhenEmptyUserId_ShouldFail()
        {
            var userId = _empty;
            var skillId = _guid;
            var level = SkillLevel.Intermediate;
            var pph = 25;

            var result = UserSkill.Create(userId , skillId , level , pph);

            result.IsFailure.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Code == UserSkillErrors.UserIdRequired.Code);
        }

        [Fact]
        public void Create_WhenEmptySkillId_ShouldFail()
        {
            var userId = _guid;
            var skillId = _empty;
            var level = SkillLevel.Intermediate;
            var pph = 25;

            var result = UserSkill.Create(userId , skillId , level , pph);

            result.IsFailure.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Code == SkillErrors.SkillIdRequired.Code);
        }

        [Fact]
        public void Create_WhenInvalidSkillLevel_ShouldFail()
        {
            var userId = _guid;
            var skillId = _guid;
            var level = (SkillLevel)200;
            var pph = 25;

            var result = UserSkill.Create(userId , skillId , level , pph);

            result.IsFailure.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Code == UserSkillErrors.InvalidSkillLevel.Code);
        }

        [Fact]
        public void Create_WhenPointPerHourTooLow_ShouldFail()
        {
            var userId = _guid;
            var skillId = _guid;
            var level = SkillLevel.Advanced;
            var pph = 0;

            var result = UserSkill.Create(userId , skillId , level , pph);

            result.IsFailure.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Code == UserSkillErrors.PointPerHourInvalid.Code);
        }

        [Fact]
        public void Create_WhenPointPerHourTooHigh_ShouldFai()
        {
            var userId = _guid;
            var skillId = _guid;
            var level = SkillLevel.Expert;
            var pph = 101;

            var result = UserSkill.Create(userId , skillId , level , pph);

            result.IsFailure.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Code == UserSkillErrors.PointPerHourInvalid.Code);
        }
    }
}