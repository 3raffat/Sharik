using FluentAssertions;
using Sharik.Domain.Skills;

namespace Sharik.Domain.UnitTests.Skills
{
    public sealed class UpdateSkillTests
    {
        private readonly Guid _guid = Guid.NewGuid();

        [Fact]
        public void Update_WithValidName_ShouldSucceed()
        {

            var categoryId = _guid;
            var name = "Csharp";
            var skill = Skill.Create(categoryId , name).Value;
            var newName = "New Valid Name";

            var result = skill.Update(newName);

            result.IsSuccess.Should().BeTrue();
            skill.Name.Should().Be(newName);

        }

        [Fact]
        public void Update_WhenNameTooShort_ShouldFail()
        {

            var categoryId = _guid;
            var name = "Csharp";
            var skill = Skill.Create(categoryId , name).Value;
            var newName = "c#";

            var result = skill.Update(newName);

            result.IsFailure.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Code == SkillErrors.SkillNameTooShort.Code);

        }

        [Fact]
        public void Update_WhenNameTooLong_ShouldFail()
        {

            var categoryId = _guid;
            var name = "Csharp";
            var skill = Skill.Create(categoryId , name).Value;
            var newName = new string('a' , 101);

            var result = skill.Update(newName);

            result.IsFailure.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Code == SkillErrors.SkillNameTooLong.Code);

        }

        [Theory]
        [InlineData(null)]
        [InlineData("  ")]
        public void Update_WhenNameIsNullOrWhiteSpace_ShouldFail(string? invalidName)
        {

            var categoryId = _guid;
            var name = "Csharp";
            var skill = Skill.Create(categoryId , name).Value;
            var newName = invalidName;

            var result = skill.Update(newName);

            result.IsFailure.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Code == SkillErrors.SkillNameRequired.Code);

        }

    }
}
